using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

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
            group = new Group();
            storage.observers += new EventHandler(UpdateFormModel);
        }

        public class Storage // Класс-хранилище фигур
        {
            private LinkedList<Figure> storage = new LinkedList<Figure>();
            private string status;
            public string Status { set { status = value; } get { return status; } }
            // Добавление фигур в хранилище
            public void add(Figure figure)
            {
                storage.AddLast(figure);
                observers.Invoke(this, null);
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
            public void move(int key, (int FWigth, int FHeight, int PHeight, int PWidth) size, Group group)
            {
                foreach (Figure figure in storage)
                {
                    if (figure.IsSelected == true)
                    {
                        if (figure.Shape == "Group")
                        {
                            var it = group.first();
                            while (it != null)
                            {
                                transite(it, key, false);
                                it = group.next(it);
                            }
                        }
                        else transite(figure, key, false);
                    }
                    if (exception(figure, group, size) == true)
                    {
                        if (figure.Shape == "Group")
                        {
                            var it = group.first();
                            while (it != null)
                            {
                                transite(it, key, true);
                                it = group.next(it);
                            }
                        }
                        else transite(figure, key, true);                       
                    }   
                }
                observers.Invoke(this, null);
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
            public bool exception(Figure it, Group group, (int FWigth, int FHeight, int PHeight, int PWigth) size)
            {
                if (it.Shape == "Group")
                {
                    var item = group.first();
                    while (item != null)
                    {
                        if (item.X - item.Radius < 0 || item.Y - item.Radius < size.PHeight || 
                            item.X + item.Radius > size.FWigth - size.PWigth || item.Y + item.Radius > size.FHeight)
                            return true;
                        item = group.next(item);
                    }
                }
                else if (it.X - it.Radius < 0 || it.Y - it.Radius < size.PHeight ||
                         it.X + it.Radius > size.FWigth - size.PWigth || it.Y + it.Radius > size.FHeight)
                    return true;
                return false;
            }
            // Изменение размера фигуры
            public void size(int state)
            {
                foreach (Figure figure in storage)
                {
                    if (figure.IsSelected == true)
                    {
                        if (state == 187) figure.Radius += 3;
                        if (state == 189) figure.Radius -= 3;
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
            // Проверка на попадание щелчка мыши в фигуру
            public Figure is_inside(int x0, int y0)
            {
                foreach (Figure figure in storage)
                {
                    switch (figure.Shape)
                    {
                        case "Square":
                            {
                                int x1 = figure.X - figure.Radius;
                                int y1 = figure.Y - figure.Radius;
                                int x3 = figure.X + figure.Radius;
                                int y3 = figure.Y + figure.Radius;
                                if (x0 > x1 && y0 > y1 && x0 < x3 && y0 < y3)
                                    return figure;
                                break;
                            }
                        case "Triangle":
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
                                    return figure;
                                break;
                            }
                        case "Circle":
                            {
                                int sum = Convert.ToInt32(Math.Pow(x0 - figure.X, 2) + Math.Pow(y0 - figure.Y, 2));
                                int rad = Convert.ToInt32(Math.Pow(figure.Radius, 2));
                                if (sum <= rad)
                                    return figure;
                                break;
                            }
                    }

                }
                return null;
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
                    figure.Selection = color;
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

            protected Color border;
            public Color Border { set { border = value; } get { return border; } }

            protected Color selection;
            public Color Selection { set { selection = value; } get { return selection; } }

            protected int borderSize = 3;
            public int BorderSize { set { borderSize = value; } get { return borderSize; } }

            protected bool isSelected = false;
            public bool IsSelected { set { isSelected = value; } get { return isSelected; } }

            protected string shape;
            public string Shape { set { shape = value; } get { return shape; } }

            private LinkedList<Figure> group;
        }

        public class Group : Figure
        {
            //public Figure isCircle, isSquare, isTriangle;
            private LinkedList<Figure> group = new LinkedList<Figure>();

            public void take(Storage storage)
            {
                //Group group = new Group();
               
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
            public Figure first()
            {
                if (group.Count != 0)
                    return group.First();
                return null;
            }
            // Получение следующего элемента хранилища
            public Figure next(Figure figure)
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

        public void UpdateFormModel(object sender, EventArgs e) // Обновление модели
        {
            draw();
        }

        public void draw() // Рисование объектов
        {
            Graphics g = CreateGraphics();
            g.Clear(BackColor);

            if (storage.first() == null)
                return;

            var it = storage.first();
            while (it != null)
            {
                SolidBrush cr = new SolidBrush(it.Color);
                Pen br = new Pen(it.Border, it.BorderSize);
                if (it.IsSelected == true)
                    br = new Pen(it.Selection, it.BorderSize);

                if (it.Shape == "Square")
                {
                    drawSquare(it, cr, br);
                }
                if (it.Shape == "Circle")
                {
                    drawCircle(it, cr, br);
                }

                if (it.Shape == "Triangle")
                {
                    drawTriangle(it, cr, br);
                }
               
                if(it.Shape == "Group")
                {
                    
                    var iterator = group.first();
                    while(iterator != null)
                    {
                        if (iterator.Shape == "Square") drawSquare(iterator, cr, br);
                        if (iterator.Shape == "Circle") drawCircle(iterator, cr, br);
                        if (iterator.Shape == "Triangle") drawTriangle(iterator, cr, br);
                        iterator = group.next(iterator);
                    }
                }
                it = storage.next(it);
            }

        }

        public void drawSquare(Figure it, SolidBrush cr, Pen br)
        {
            CreateGraphics().FillRectangle(cr, new Rectangle(it.X - it.Radius,
                       it.Y - it.Radius, 2 * it.Radius, 2 * it.Radius));

            CreateGraphics().DrawRectangle(br, new Rectangle(it.X - it.Radius - it.BorderSize / 2,
                it.Y - it.Radius - it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2));
        }

        public void drawCircle(Figure it, SolidBrush cr, Pen br)
        {
            CreateGraphics().FillEllipse(cr, it.X - it.Radius,
                       it.Y - it.Radius, 2 * it.Radius, 2 * it.Radius);

            CreateGraphics().DrawEllipse(br, it.X - it.Radius - it.BorderSize / 2,
                it.Y - it.Radius - it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2, 2 * it.Radius + it.BorderSize / 2);
        }

        public void drawTriangle(Figure it, SolidBrush cr, Pen br)
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

            CreateGraphics().FillPath(cr, myPath);
            CreateGraphics().DrawPath(br, myPath);
        }

        // Обработчик нажатия на клавишу мыши
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        Figure figure = new Figure();
                        figure.X = e.X;
                        figure.Y = e.Y;
                        var size = (FWidth: ClientSize.Width, FHeight: ClientSize.Height, 
                                    PHeight: panelUp.Height, PWidth: paneRight.Width);
                        // Проверка на нахождение фигуры в пределах формы
                        if (storage.exception(figure, null, size) == true)
                        {
                            if (e.X - figure.Radius < 0) figure.X = figure.Radius;
                            if (e.X + figure.Radius > size.FWidth) figure.X = size.FWidth - figure.Radius;
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
                            storage.observers.Invoke(this, null);
                        }
                        else storage.add(figure);
                        break;
                    }
            }
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
                storage.move((int)e.KeyCode, size, group);
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
                //Group group = new Group();
                group.Shape = "Group";
                group.take(storage);
                group.Color = pbColor.BackColor;
                group.Border = Color.Black;
                group.IsSelected = true;
                group.Selection = pbSelection.BackColor;
                storage.add(group);
                //storage.observers.Invoke(this, null);
                 
                //group.add(storage, group);
            }

        }
    }
}
