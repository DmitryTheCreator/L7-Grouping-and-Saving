﻿namespace L7_Grouping_and_Saving
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelUp = new System.Windows.Forms.Panel();
            this.lblColor = new System.Windows.Forms.Label();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.pbSelection = new System.Windows.Forms.PictureBox();
            this.cmbxShape = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.paneRight = new System.Windows.Forms.Panel();
            this.btnGroup = new System.Windows.Forms.Button();
            this.colorShape = new System.Windows.Forms.ColorDialog();
            this.colorBorder = new System.Windows.Forms.ColorDialog();
            this.panelUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSelection)).BeginInit();
            this.paneRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelUp
            // 
            this.panelUp.BackColor = System.Drawing.Color.Silver;
            this.panelUp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUp.Controls.Add(this.lblColor);
            this.panelUp.Controls.Add(this.pbColor);
            this.panelUp.Controls.Add(this.pbSelection);
            this.panelUp.Controls.Add(this.cmbxShape);
            this.panelUp.Controls.Add(this.label1);
            this.panelUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUp.Location = new System.Drawing.Point(0, 0);
            this.panelUp.Name = "panelUp";
            this.panelUp.Size = new System.Drawing.Size(1114, 54);
            this.panelUp.TabIndex = 1;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.BackColor = System.Drawing.Color.Silver;
            this.lblColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblColor.ForeColor = System.Drawing.Color.Black;
            this.lblColor.Location = new System.Drawing.Point(197, 14);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(136, 25);
            this.lblColor.TabIndex = 13;
            this.lblColor.Text = "Цвет фигуры";
            // 
            // pbColor
            // 
            this.pbColor.BackColor = System.Drawing.Color.SandyBrown;
            this.pbColor.Location = new System.Drawing.Point(339, 16);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(30, 30);
            this.pbColor.TabIndex = 14;
            this.pbColor.TabStop = false;
            this.pbColor.Click += new System.EventHandler(this.pbColor_Click);
            // 
            // pbSelection
            // 
            this.pbSelection.BackColor = System.Drawing.Color.MediumPurple;
            this.pbSelection.Location = new System.Drawing.Point(561, 16);
            this.pbSelection.Name = "pbSelection";
            this.pbSelection.Size = new System.Drawing.Size(30, 30);
            this.pbSelection.TabIndex = 17;
            this.pbSelection.TabStop = false;
            this.pbSelection.Click += new System.EventHandler(this.pbSelection_Click);
            // 
            // cmbxShape
            // 
            this.cmbxShape.BackColor = System.Drawing.Color.White;
            this.cmbxShape.DropDownWidth = 108;
            this.cmbxShape.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmbxShape.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbxShape.FormattingEnabled = true;
            this.cmbxShape.Items.AddRange(new object[] {
            "Квадрат",
            "Треугольник",
            "Круг"});
            this.cmbxShape.Location = new System.Drawing.Point(11, 11);
            this.cmbxShape.Name = "cmbxShape";
            this.cmbxShape.Size = new System.Drawing.Size(160, 33);
            this.cmbxShape.TabIndex = 15;
            this.cmbxShape.Text = "Фигура";
            this.cmbxShape.SelectedIndexChanged += new System.EventHandler(this.cmbxShape_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(388, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "Цвет выделения";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(6, 123);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(160, 100);
            this.btnRemove.TabIndex = 19;
            this.btnRemove.Text = "Удалить";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(3, 17);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(160, 100);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // paneRight
            // 
            this.paneRight.BackColor = System.Drawing.Color.Silver;
            this.paneRight.Controls.Add(this.btnGroup);
            this.paneRight.Controls.Add(this.btnRemove);
            this.paneRight.Controls.Add(this.btnClear);
            this.paneRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.paneRight.Location = new System.Drawing.Point(948, 54);
            this.paneRight.Name = "paneRight";
            this.paneRight.Size = new System.Drawing.Size(166, 542);
            this.paneRight.TabIndex = 2;
            // 
            // btnGroup
            // 
            this.btnGroup.Location = new System.Drawing.Point(3, 229);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(160, 100);
            this.btnGroup.TabIndex = 20;
            this.btnGroup.Text = "Сгруппировать";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 596);
            this.Controls.Add(this.paneRight);
            this.Controls.Add(this.panelUp);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panelUp.ResumeLayout(false);
            this.panelUp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSelection)).EndInit();
            this.paneRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelUp;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.PictureBox pbSelection;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.PictureBox pbColor;
        private System.Windows.Forms.ComboBox cmbxShape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel paneRight;
        private System.Windows.Forms.ColorDialog colorShape;
        private System.Windows.Forms.ColorDialog colorBorder;
        private System.Windows.Forms.Button btnGroup;
    }
}

