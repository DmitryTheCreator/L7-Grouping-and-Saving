using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace L7_Grouping_and_Saving
{
    public partial class Form1 : Form
    {
        Storage storage;
        Group group;

        public Form1()
        {
            InitializeComponent();
            storage = new Storage();
            storage.AddObserver(tree);
            storage.observers += new EventHandler(UpdateFormModel);
        }

        TreeViews tree = new TreeViews();

        public interface IObservable
        {   // Наблюдаемый объект
            void AddObserver(IObserver o);
            void RemoveObserver(IObserver o);
            void NotifyObservers();
        }
        public interface IObserver
        {   // Наблюдатель
            void Update(ref TreeView treeView, Storage storage);
        }
        public class Storage : IObservable// Класс-хранилище фигур
        {
            public LinkedList<Figure> storage = new LinkedList<Figure>();
            public TreeView treeView;
            public List<IObserver> observerss;
            
            private string status;
            public string Status { set { status = value; } get { return status; } }
            // Добавление фигур в хранилище
            public Storage()
            {
                observerss = new List<IObserver>();
            }
            public void add(Figure figure)
            {
                storage.AddLast(figure);
                observers.Invoke(this, null);
                NotifyObservers();
            }
            public void intit_tree(ref TreeView treeView)
            {
                this.treeView = treeView;
            }
            public void NotifyObservers()
            {
                foreach (IObserver observer in observerss)
                    observer.Update(ref treeView, this);
            }
            public void draw(Form form, SolidBrush cr, Pen br)
            {
                foreach(Figure figure in storage)
                {                   
                    cr = new SolidBrush(figure.Color);
                    br = new Pen(figure.Border, figure.BorderSize);
                    if (figure.IsSelected == true)
                        br = new Pen(figure.Selection, figure.BorderSize);
                    if (figure.Shape == "Square") drawSquare(form, cr, br, figure);
                    if (figure.Shape == "Circle") drawCircle(form, cr, br, figure);
                    if (figure.Shape == "Triangle") drawTriangle(form, cr, br, figure);
                    if (figure.Shape == "Group") drawGroup(form, cr, br, figure);
                }
            }
            public void unGroup()
            {
                for(int i = 0; i < storage.Count; ++i)
                {
                    for(var it = storage.First; it != null; it = it.Next)
                    {
                        if(it.Value.IsSelected == true)
                        {
                            if(it.Value.Shape == "Group")
                            {
                                figureSearch5(it.Value);
                                del(it.Value);
                            }
                        }
                    }
                }
                observers.Invoke(this, null);
            }
            public string save(Figure figure)
            {   // Функция сохранения
                return figure.Shape + "\n" + figure.X + "\n" + figure.Y + "\n" + figure.Radius + "\n" + figure.Color.ToArgb();
            }
            public string saveGroup(Figure figure)
            {   // Функция сохранения
                string str = "Group" + "\n" + "2";
                figureSearch6(figure, ref str);
                return str;
            }
            public void load(ref StreamReader sr, ref Figure figure, CreateFigure createFigure, Storage storage, Group group)
            {   // Функция загрузки
                int chislo = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < chislo; ++i)
                {
                    createFigure.caseswitch(ref sr, ref figure, createFigure, storage, group);
                    //group.take(storage);
                    group.add(figure);               
                }
            }
            public void AddObserver(IObserver o)
            {
                observerss.Add(o);
            }
            public void RemoveObserver(IObserver o)
            {
                observerss.Remove(o);
            }

            public void load(ref Figure figure, string x, string y, string lenght, string fillcolor)
            {   // Функция загрузки
                figure.X = Convert.ToInt32(x);
                figure.Y = Convert.ToInt32(y);
                figure.Radius = Convert.ToInt32(lenght);
                figure.Color = Color.FromArgb(Convert.ToInt32(fillcolor));
            }
            // Удаление фигур из хранилища
            public void del()
            {
                for (int i = 0; i < storage.Count; ++i)
                {
                    var item = storage.First;
                    while (item != null)
                    {
                        if (item.Value.IsSelected == true)
                        {
                            storage.Remove(item);
                            i--;
                        }
                        item = item.Next;
                    }
                }
                observers.Invoke(this, null);
                NotifyObservers();
            }
            public void del(Figure figure)
            {
                var item = storage.First;
                while (item != null)
                {
                    if (item.Value == figure)
                        storage.Remove(item);
                    item = item.Next;
                }
            }
            public int count() { return storage.Count; }
            // Перемещение фигур
            public void move(int key, (int FWigth, int FHeight, int PHeight, int PWidth) size)
            {
                foreach (Figure figure in storage)
                {
                    if (figure.IsSelected == true)
                    {
                        if (figure.Shape == "Group")
                        {
                            figureSearch(figure, key, false);
                        }
                        else transite(figure, key, false);
                    }
                    if (exception(figure, size) == true)
                    {
                        if (figure.Shape == "Group")
                        {
                            figureSearch(figure, key, true);
                        }
                        else transite(figure, key, true);                       
                    }   
                }
                observers.Invoke(this, null);
                NotifyObservers();
            }

            public void transite(Figure it, int key, bool undo)
            {
                if(undo == false)
                {
                    if (key == 87) it.Y -= 5;
                    if (key == 83) it.Y += 5;
                    if (key == 65) it.X -= 5;
                    if (key == 68) it.X += 5;
                }
                else
                {
                    if (key == 87) it.Y += 5;
                    if (key == 83) it.Y -= 5;
                    if (key == 65) it.X += 5;
                    if (key == 68) it.X -= 5;
                }
            }
            // Проверка на нахождение фигуры в пределах формы
            public bool exception(Figure figure, (int FWigth, int FHeight, int PHeight, int PWigth) size)
            {
                if (figure.Shape == "Group")
                {
                    if (figureSearch2(figure, size) == true)
                        return true;
                }
                else if (figure.X - figure.Radius < 0 || figure.Y - figure.Radius < size.PHeight ||
                         figure.X + figure.Radius > size.FWigth - size.PWigth || figure.Y + figure.Radius > size.FHeight)
                    return true;
                return false;
            }
            public void figureSearch(Figure it, int key, bool undo)
            {   // Отображение группы
                for (var i = it.first(); i != null; i = it.next(i))
                {
                    if (i.Shape != "Group") transite(i, key, undo);                   
                    else figureSearch(i, key, undo);
                }
            }
            public bool figureSearch2(Figure it, (int FWigth, int FHeight, int PHeight, int PWigth) size)
            {   // Отображение группы
                for (var i = it.first(); i != null; i = it.next(i))
                {
                    if(i.Shape != "Group")
                        if (i.X - i.Radius < 0 || i.Y - i.Radius < size.PHeight ||
                            i.X + i.Radius > size.FWigth - size.PWigth || i.Y + i.Radius > size.FHeight)
                            return true;
                    else figureSearch2(i, size);
                }
                return false;
            }          
            public void figureSearch3(Figure it, int x0, int y0, ref bool check)
            {   // Отображение группы
                
                for (var i = it.first(); i != null; i = it.next(i))
                {
                    if (i.Shape != "Group")
                    {
                        if (i.Shape == "Square")
                            if (inside_square(i, x0, y0) == true)
                            {
                                check = true;
                                //return i;
                            }
                        if (i.Shape == "Triangle")
                            if (inside_triangle(i, x0, y0) == true)
                            {
                                check = true;
                                //return i;
                            }
                        if (i.Shape == "Circle")
                            if (inside_circle(i, x0, y0) == true)
                            {
                                check = true;
                                //return i;
                            }
                    }
                    else figureSearch3(i, x0, y0, ref check);
                }
            }
            public void figureSearch4(ref Figure it, int state)
            {   // Отображение группы
                for (var i = it.first(); i != null; i = it.next(i))
                {
                    if (i.Shape != "Group")
                    {
                        if (state == 187) i.Radius += 3;
                        if (state == 189) i.Radius -= 3;
                    }
                    else figureSearch4(ref i, state);
                }
            }
            public void figureSearch5(Figure it)
            {   // Отображение группы
                for (var i = it.first(); i != null; i = it.next(i))
                    if (i.Shape != "Group")
                        storage.AddLast(i);
                    else figureSearch5(i);
            }
            public void figureSearch6(Figure it, ref string str)
            {   // Отображение группы
                for (var i = it.first(); i != null; i = it.next(i))
                    if (i.Shape != "Group")
                    {
                        str += "\n" + save(i);
                    }
                    else figureSearch6(i, ref str);
            }
            // Изменение размера фигуры
            public void size(int state)
            {
                for (var figure = storage.First; figure != null; figure = figure.Next)
                //foreach (Figure figure in storage)
                {
                    if (figure.Value.IsSelected == true)
                    {
                        if (figure.Value.Shape == "Group")
                        {
                            Figure fig = figure.Value;
                            figureSearch4(ref fig, state);
                            figure.Value = fig;
                        }
                        else
                        {
                            if (state == 187) figure.Value.Radius += 3;
                            if (state == 189) figure.Value.Radius -= 3;
                        }
                    }
                }
                observers.Invoke(this, null);

            }
            // Получение первого элемента хранилища
            public Figure first()
            {
                if (storage.Count != 0)
                    return storage.First();
                return null;
            }

            // Получение следующего элемента хранилища
            public Figure next(Figure figure)
            {
                bool check = false;
                foreach (Figure fig in storage)
                {
                    if (check == true) return fig;
                    if (figure == fig)
                        check = true;
                }
                return null;
            }

            public void drawSquare(Form form, SolidBrush cr, Pen br, Figure it)
            {
                form.CreateGraphics().FillRectangle(cr, new Rectangle(it.X - it.Radius,
                           it.Y - it.Radius, 2 * it.Radius, 2 * it.Radius));

                form.CreateGraphics().DrawRectangle(br, new Rectangle(it.X - it.Radius - it.BorderSize / 2,
                    it.Y - it.Radius - it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2));
            }
            public void drawCircle(Form form, SolidBrush cr, Pen br, Figure it)
            {
                form.CreateGraphics().FillEllipse(cr, it.X - it.Radius,
                           it.Y - it.Radius, 2 * it.Radius, 2 * it.Radius);

                form.CreateGraphics().DrawEllipse(br, it.X - it.Radius - it.BorderSize / 2,
                    it.Y - it.Radius - it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2);
            }
            public void drawTriangle(Form form, SolidBrush cr, Pen br, Figure it)
            {
                List<Point> line1 = new List<Point>();
                List<Point> line2 = new List<Point>();
                List<Point> line3 = new List<Point>();
                line1.Add(new Point(it.X, it.Y - it.Radius - it.BorderSize));
                line1.Add(new Point(it.X + it.Radius + it.BorderSize, it.Y + it.Radius / 2 + it.BorderSize / 2));
                line2.Add(new Point(it.X, it.Y - it.Radius - it.BorderSize));
                line2.Add(new Point(it.X - it.Radius - it.BorderSize, it.Y + it.Radius / 2 + it.BorderSize / 2));
                line3.Add(new Point(it.X - it.Radius - it.BorderSize, it.Y + it.Radius / 2 + it.BorderSize / 2));
                line3.Add(new Point(it.X + it.Radius + it.BorderSize, it.Y + it.Radius / 2 + it.BorderSize / 2));

                GraphicsPath myPath = new GraphicsPath();
                myPath.StartFigure();
                myPath.AddLines(line1.ToArray());
                myPath.AddLines(line2.ToArray());
                myPath.AddLines(line3.ToArray());
                myPath.CloseFigure();

                form.CreateGraphics().FillPath(cr, myPath);
                form.CreateGraphics().DrawPath(br, myPath);
            }
            public void drawGroup(Form form, SolidBrush cr, Pen br, Figure it)
            {   // Отображение группы
                for(var i = it.first(); i != null; i = it.next(i))
                {
                    if(i.Shape != "Group")
                    {
                        if (i.Shape == "Square") drawSquare(form, cr, br, i);
                        if (i.Shape == "Circle") drawCircle(form, cr, br, i);
                        if (i.Shape == "Triangle") drawTriangle(form, cr, br, i);
                    }
                    else drawGroup(form, cr, br, i);
                }              
            }
            // Проверка на выделенность фигуры
            public bool is_selected()
            {
                foreach (Figure figure in storage)
                {
                    if (figure.IsSelected == true)
                        return true;
                }
                return false;
            }
            public Figure is_adhesive()
            {
                foreach (Figure figure in storage)
                {
                    if (figure.IsSelected == true)
                    {
                        figure.IsAdhesive = true;
                        return figure;
                    }
                }
                return null;
            }
            // Проверка на попадание щелчка мыши в фигуру
            public Figure is_inside(int x0, int y0)
            {
                int index = 0;
                foreach (Figure figure in storage)
                {
                    switch (figure.Shape)
                    {
                        case "Square":
                            {                  
                                if (inside_square(figure, x0, y0) == true)
                                    return figure;
                                break;
                            }
                        case "Triangle":
                            {
                                if (inside_triangle(figure, x0, y0) == true)
                                    return figure;
                                break;
                            }
                        case "Circle":
                            {
                                if (inside_circle(figure, x0, y0) == true)
                                    return figure;
                                break;
                            }
                        case "Group":
                            {
                                bool check = false;
                                figureSearch3(figure, x0, y0, ref check);
                                if (check == true)
                                    return figure;
                                break;
                            }
                    }
                    index++;
                }
                return null;
            }

            public bool inside_square(Figure figure, int x0, int y0)
            {
                int x1 = figure.X - figure.Radius;
                int y1 = figure.Y - figure.Radius;
                int x3 = figure.X + figure.Radius;
                int y3 = figure.Y + figure.Radius;
                if (x0 > x1 && y0 > y1 && x0 < x3 && y0 < y3)
                    return true;
                return false;
            }
            public bool inside_triangle(Figure figure, int x0, int y0)
            {
                int x1 = figure.X;
                int y1 = figure.Y - figure.Radius;
                int x2 = figure.X + figure.Radius;
                int y2 = figure.Y + figure.Radius / 2;
                int x3 = figure.X - figure.Radius;
                int y3 = figure.Y + figure.Radius / 2;
                int alpha = (x1 - x0) * (y2 - y1) - (x2 - x1) * (y1 - y0);
                int beta = (x2 - x0) * (y3 - y2) - (x3 - x2) * (y2 - y0);
                int gamma = (x3 - x0) * (y1 - y3) - (x1 - x3) * (y3 - y0);
                if (((alpha > 0) && (beta > 0) && (gamma > 0)) || ((alpha < 0) && (beta < 0) && (gamma < 0)))
                    return true;
                return false;
            }
            public bool inside_circle(Figure figure, int x0, int y0)
            {
                int sum = Convert.ToInt32(Math.Pow(x0 - figure.X, 2) + Math.Pow(y0 - figure.Y, 2));
                int rad = Convert.ToInt32(Math.Pow(figure.Radius, 2));
                if (sum <= rad) return true;
                return false;
            }
            // Изменение цвета фигур
            public void change_color(Color color)
            {
                foreach (Figure figure in storage)
                    figure.Color = color;
                observers.Invoke(this, null);
            }
            // Изменение цвета выделения фигур
            public void change_selection(Color color)
            {
                foreach (Figure figure in storage)
                {
                    if(figure.Shape != "Group")
                        figure.Selection = color;
                }
                observers.Invoke(this, null);
            }

            public void change_group_selection(Color color, Group group)
            {
                foreach (Figure figure in storage)
                {
                    if (figure.Shape == "Group") 
                    {
                        var it = group.first();
                        while(it != null)
                        {
                            it.Border = color;
                            it = group.next(it);
                        }
                    }
                }
                observers.Invoke(this, null);
            }
            // Удаление всех объектов из хранилища
            public void remove_all() { storage.Clear(); }
            // Вызов обновления формы
            public EventHandler observers;
        }

        // Класс фигур
        public class Figure
        {
            protected int x, y;
            public int X { set { x = value; } get { return x; } }
            public int Y { set { y = value; } get { return y; } }

            protected int radius = 30;
            public int Radius { set { radius = value; } get { return radius; } }

            protected Color color;
            public Color Color { set { color = value; } get { return color; } }

            protected Color border = Color.Black;
            public Color Border { set { border = value; } get { return border; } }

            protected Color selection;
            public Color Selection { set { selection = value; } get { return selection; } }

            protected int borderSize = 3;
            public int BorderSize { set { borderSize = value; } get { return borderSize; } }

            protected bool isSelected = false;
            public bool IsSelected { set { isSelected = value; } get { return isSelected; } }

            protected bool isGroupSelected = false;
            public bool IsGroupSelected { set { isGroupSelected = value; } get { return isGroupSelected; } }

            protected string shape;
            public string Shape { set { shape = value; } get { return shape; } }

            protected bool isAdhesive = false;
            public bool IsAdhesive { set { isAdhesive = value; } get { return isAdhesive; } }


            public virtual Figure first() { return null; }
            public virtual Figure next(Figure figure) { return null; }

            public virtual void load(string x, string y, string c, string fillcolor) { }
            public virtual void load(ref StreamReader sr, Figure figure, CreateFigure createFigure) { }
            public virtual void caseswitch(ref StreamReader sr, ref Figure figure, CreateFigure createFigure, Storage storage, Group group) { }
        }

        
        public class Group : Figure
        {
            public LinkedList<Figure> group = new LinkedList<Figure>();
            
            public void take(Storage storage)
            {               
                for (int i = 0; i < storage.count(); ++i)
                {
                    var it = storage.first();
                    while (it != null)
                    {
                        if (it.IsSelected == true)
                        {
                            group.AddLast(it);
                            storage.del(it);
                            i--;
                        }
                        it = storage.next(it);
                    }
                }             
            }         
            public void add(Figure figure)
            {
                group.AddLast(figure);
            }
            public int count() { return group.Count; }
            public override Figure first()
            {
                if (group.Count != 0)
                    return group.First();
                return null;
            }
            // Получение следующего элемента хранилища
            public override Figure next(Figure figure)
            {
                bool check = false;
                foreach (Figure fig in group)
                {
                    if (check == true) return fig;
                    if (figure == fig)
                        check = true;
                }
                return null;
            }
        }

        public class CreateFigure : Figure
        {   // Используем Factory Method
            public override void caseswitch(ref StreamReader sr, ref Figure figure, CreateFigure createFigure, Storage storage, Group group)
            {
                string str = sr.ReadLine();
                switch (str)
                {   // В зависимости какая фигура выбрана
                    case "Circle":
                        figure.Shape = "Circle";
                        storage.load(ref figure, sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Triangle":
                        figure.Shape = "Triangle";
                        storage.load(ref figure, sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Square":
                        figure.Shape = "Square";
                        storage.load(ref figure, sr.ReadLine(), sr.ReadLine(), sr.ReadLine(), sr.ReadLine());
                        break;
                    case "Group":
                        figure.Shape = "Group";
                        storage.load(ref sr, ref figure, createFigure, storage, group);
                        break;
                }
            }
        }

        class TreeViews : IObserver
        {
            public TreeViews() { }
            public void Update(ref TreeView treeView, Storage storage)
            {   // Перерисовка treeView
                treeView.Nodes.Clear();
                treeView.Nodes.Add("Figures");
                foreach(Figure figure in storage.storage)
                {
                    fillnode(treeView.Nodes[0], figure);
                }
                
                treeView.ExpandAll();
            }
            public void treeSelect(ref TreeView treeView, int index) //выбор узла
            {   // Выделяем узел
                treeView.SelectedNode = treeView.Nodes[0].Nodes[index];
                treeView.Focus();
            }
            public void fillnode(TreeNode treeNode, Figure figure)
            {   // Заполняем treeView
                TreeNode nodes = treeNode.Nodes.Add(figure.Shape);
                if (figure.Shape == "Group")
                {
                    for (var fig = figure.first(); fig != null; fig = figure.next(fig))
                    {
                        fillnode(nodes, fig);
                    }    
                }                
            }
        }

        class Adhesive : IObserver
        {
            public Adhesive() { }
            public bool checkCircle(Storage stg, Figure one, Figure two)
            {
                if (stg.inside_circle(one, two.X, two.Y))
                    return true;
                return false;
            }
            public bool checkTriangle(Storage stg, Figure one, Figure two)
            {
                if (stg.inside_triangle(one, two.X, two.Y))
                    return true;
                return false;

            }
            public bool checkSquare(Storage stg, Figure one, Figure two)
            {
                if (stg.inside_square(one, two.X, two.Y))
                    return true;
                return false;
            }
            public bool FigureCheck(Storage stg, Figure one, Figure two, string b, int d)
            {
                switch (two.Shape)
                {
                    case "Circle":
                        if (checkCircle(stg, one, two))
                            return true;
                        break;

                    case "Triangle":
                        if (checkTriangle(stg, one, two))
                            return true;
                        break;

                    case "Square":
                        if (checkSquare(stg, one, two))
                            return true;
                        break;
                }
                return false;

            }
            public void Update(ref TreeView treeView, Storage stg)
            {
                Figure fi = stg.is_adhesive();
                foreach (Figure figure in stg.storage)
                {
                    if(figure == fi)
                    {
                        continue;
                    }
                    if(FigureCheck(stg, fi, figure, "", 0))
                    {
                        figure.Color = Color.Yellow;
                        figure.IsSelected = true;
                    }
                }
            }
        }

        public void UpdateFormModel(object sender, EventArgs e) // Обновление модели
        {
            draw();         
        }

        public void draw() // Рисование объектов
        {
            Graphics g = CreateGraphics();
            g.Clear(BackColor);
            SolidBrush cr = new SolidBrush(Color.White);
            Pen br = new Pen(Color.Red, 10);
            storage.draw(this, cr, br);
        }

        // Обработчик нажатия на клавишу мыши
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        Figure figure = new Figure();
                        storage.intit_tree(ref treeView1);
                        figure.X = e.X;
                        figure.Y = e.Y;
                        var size = (FWidth: ClientSize.Width, FHeight: ClientSize.Height, 
                                    PHeight: panelUp.Height, PWidth: paneRight.Width);
                        // Проверка на нахождение фигуры в пределах формы
                        if (storage.exception(figure, size) == true)
                        {
                            if (e.X - figure.Radius < 0) figure.X = figure.Radius;
                            if (e.X + figure.Radius > size.FWidth) figure.X = size.FWidth - figure.Radius;
                            if (e.Y - figure.Radius < size.PHeight) figure.Y = size.PHeight + figure.Radius;
                            if (e.Y - figure.Radius < size.PHeight) figure.Y = size.PHeight + figure.Radius;
                            if (e.Y + figure.Radius > size.FHeight) figure.Y = size.FHeight - figure.Radius;
                        }
                        
                        figure.Shape = storage.Status;
                        figure.Color = pbColor.BackColor;
                        figure.Border = Color.Black;
                        figure.Selection = pbSelection.BackColor;

                        // Проверка на попадание щелчка мыши в фигуру
                        if (storage.is_inside(e.X, e.Y) != null)
                        {
                            if (storage.is_inside(e.X, e.Y).IsSelected == false)
                                storage.is_inside(e.X, e.Y).IsSelected = true;
                            else storage.is_inside(e.X, e.Y).IsSelected = false;
                            
                            int ind = 0;
                            foreach(Figure fig in storage.storage)
                            {
                                if(storage.is_inside(figure.X, figure.Y) == fig)
                                    break;
                                ind++;
                            }
                            tree.treeSelect(ref treeView1, ind);
                            storage.observers.Invoke(this, null);
                        }
                        else storage.add(figure);
                        break;
                    }
            }
        }

        public void Update(ref TreeView treeView, Storage stg)
        {
            Graphics g = CreateGraphics();
            g.Clear(BackColor);
            SolidBrush cr = new SolidBrush(Color.White);
            Pen br = new Pen(Color.Red, 10);
            storage.draw(this, cr, br);

        }

        private void pbColor_Click(object sender, EventArgs e)
        {
            if (colorShape.ShowDialog() == DialogResult.Cancel)
                return;
            pbColor.BackColor = colorShape.Color;
            storage.change_color(pbColor.BackColor);
        }

        private void cmbxShape_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbxShape.SelectedItem == cmbxShape.Items[0]) storage.Status = "Square";
            else if (cmbxShape.SelectedItem == cmbxShape.Items[1]) storage.Status = "Triangle";
            else if (cmbxShape.SelectedItem == cmbxShape.Items[2]) storage.Status = "Circle";
            panelUp.Focus();
        }

        private void pbSelection_Click(object sender, EventArgs e)
        {
            if (colorBorder.ShowDialog() == DialogResult.Cancel)
                return;
            pbSelection.BackColor = colorBorder.Color;
            storage.change_selection(pbSelection.BackColor);
        }

        private void pbGroupSelection_Click(object sender, EventArgs e)
        {
            if (colorGroupBorder.ShowDialog() == DialogResult.Cancel)
                return;
            pbGroupSelection.BackColor = colorGroupBorder.Color;
            storage.change_group_selection(pbGroupSelection.BackColor, group);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Graphics g = CreateGraphics();
            g.Clear(BackColor);
            storage.remove_all();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            storage.del();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (storage.is_selected() == true && (e.KeyCode == Keys.W || e.KeyCode == Keys.S ||
                e.KeyCode == Keys.A || e.KeyCode == Keys.D))
            {
                var size = (FWidth: ClientSize.Width, FHeight: ClientSize.Height, 
                            PHeight: panelUp.Height, PWidth: paneRight.Width);
                storage.move((int)e.KeyCode, size);
            }
            if (storage.is_selected() == true && e.KeyCode == Keys.Oemplus)
                storage.size((int)e.KeyCode);
            if (storage.is_selected() == true && e.KeyCode == Keys.OemMinus)
                storage.size((int)e.KeyCode);
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if(storage.is_selected() == true)
            {
                group = new Group();
                group.Shape = "Group";
                group.take(storage);
                group.Color = pbColor.BackColor;
                group.Border = Color.Black;
                group.IsSelected = true;
                group.Selection = pbGroupSelection.BackColor;
                storage.add(group);
            }

        }

        private void btnUnGroup_Click(object sender, EventArgs e)
        {
            storage.unGroup();
        }
        string path = @"C:\Users\ПК\OneDrive\Документы\ООП\Лабы\L7 Grouping and Saving\L7 Grouping and Saving\File.txt";
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(storage.count().ToString());
                for(var i = storage.first(); i != null; i = storage.next(i))
                {
                    if(i.Shape == "Group") sw.WriteLine(storage.saveGroup(i));
                    else sw.WriteLine(storage.save(i));
                }               
            }
            storage.observers.Invoke(this, null);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            {
                string str = sr.ReadLine();
                int strend = Convert.ToInt32(str);
                for (int i = 0; i < strend; ++i)
                {
                    Figure figure = new Figure();
                    group = new Group();
                    group.Shape = "Group";
                    group.Color = pbColor.BackColor;
                    group.Border = Color.Black;
                    figure.Selection = pbSelection.BackColor;
                    CreateFigure create = new CreateFigure();
                    create.caseswitch(ref sr, ref figure, create, storage, group);
                    if (group.count() != 0) storage.add(group);
                    else storage.add(figure);
                }
                sr.Close();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int g;
            if (e.Node.Level != 1)
                g = e.Node.Parent.Index;
            else
                g = e.Node.Index;
            storage.observers.Invoke(this, null);
        }

        private void btnAdhesive_Click(object sender, EventArgs e)
        {
            Adhesive adhesive = new Adhesive();
            storage.AddObserver(adhesive);           
        }
    }
}
