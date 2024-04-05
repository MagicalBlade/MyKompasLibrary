namespace MyKompasLibrary.Windows
{
    partial class Form_CreatPartFromPos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.b_Ok = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.tb_Thickness = new System.Windows.Forms.TextBox();
            this.l_Thickness = new System.Windows.Forms.Label();
            this.gb_plane = new System.Windows.Forms.GroupBox();
            this.rb_Right = new System.Windows.Forms.RadioButton();
            this.rb_Left = new System.Windows.Forms.RadioButton();
            this.rb_Backside = new System.Windows.Forms.RadioButton();
            this.rb_Front = new System.Windows.Forms.RadioButton();
            this.rb_Bottom = new System.Windows.Forms.RadioButton();
            this.rb_Top = new System.Windows.Forms.RadioButton();
            this.gb_Direction = new System.Windows.Forms.GroupBox();
            this.rb_Symmetrically = new System.Windows.Forms.RadioButton();
            this.rb_Back = new System.Windows.Forms.RadioButton();
            this.rb_Straight = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gb_plane.SuspendLayout();
            this.gb_Direction.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_Ok
            // 
            this.b_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.b_Ok.Location = new System.Drawing.Point(50, 203);
            this.b_Ok.Name = "b_Ok";
            this.b_Ok.Size = new System.Drawing.Size(75, 23);
            this.b_Ok.TabIndex = 1;
            this.b_Ok.Text = "Применить";
            this.b_Ok.UseVisualStyleBackColor = true;
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(131, 203);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 2;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            // 
            // tb_Thickness
            // 
            this.tb_Thickness.Location = new System.Drawing.Point(154, 177);
            this.tb_Thickness.Name = "tb_Thickness";
            this.tb_Thickness.Size = new System.Drawing.Size(46, 20);
            this.tb_Thickness.TabIndex = 0;
            // 
            // l_Thickness
            // 
            this.l_Thickness.AutoSize = true;
            this.l_Thickness.Location = new System.Drawing.Point(57, 181);
            this.l_Thickness.Name = "l_Thickness";
            this.l_Thickness.Size = new System.Drawing.Size(94, 13);
            this.l_Thickness.TabIndex = 3;
            this.l_Thickness.Text = "Толщина детали:";
            // 
            // gb_plane
            // 
            this.gb_plane.Controls.Add(this.rb_Right);
            this.gb_plane.Controls.Add(this.rb_Left);
            this.gb_plane.Controls.Add(this.rb_Backside);
            this.gb_plane.Controls.Add(this.rb_Front);
            this.gb_plane.Controls.Add(this.rb_Bottom);
            this.gb_plane.Controls.Add(this.rb_Top);
            this.gb_plane.Location = new System.Drawing.Point(12, 12);
            this.gb_plane.Name = "gb_plane";
            this.gb_plane.Size = new System.Drawing.Size(123, 159);
            this.gb_plane.TabIndex = 4;
            this.gb_plane.TabStop = false;
            this.gb_plane.Text = "Плоскость эскиза";
            // 
            // rb_Right
            // 
            this.rb_Right.AutoSize = true;
            this.rb_Right.Location = new System.Drawing.Point(6, 134);
            this.rb_Right.Name = "rb_Right";
            this.rb_Right.Size = new System.Drawing.Size(62, 17);
            this.rb_Right.TabIndex = 2;
            this.rb_Right.TabStop = true;
            this.rb_Right.Text = "Справа";
            this.toolTip1.SetToolTip(this.rb_Right, "Горячая клавиша Y");
            this.rb_Right.UseVisualStyleBackColor = true;
            // 
            // rb_Left
            // 
            this.rb_Left.AutoSize = true;
            this.rb_Left.Location = new System.Drawing.Point(6, 111);
            this.rb_Left.Name = "rb_Left";
            this.rb_Left.Size = new System.Drawing.Size(56, 17);
            this.rb_Left.TabIndex = 2;
            this.rb_Left.TabStop = true;
            this.rb_Left.Text = "Слева";
            this.toolTip1.SetToolTip(this.rb_Left, "Горячая клавиша E");
            this.rb_Left.UseVisualStyleBackColor = true;
            // 
            // rb_Backside
            // 
            this.rb_Backside.AutoSize = true;
            this.rb_Backside.Location = new System.Drawing.Point(6, 88);
            this.rb_Backside.Name = "rb_Backside";
            this.rb_Backside.Size = new System.Drawing.Size(56, 17);
            this.rb_Backside.TabIndex = 2;
            this.rb_Backside.TabStop = true;
            this.rb_Backside.Text = "Сзади";
            this.toolTip1.SetToolTip(this.rb_Backside, "Горячая клавиша T");
            this.rb_Backside.UseVisualStyleBackColor = true;
            // 
            // rb_Front
            // 
            this.rb_Front.AutoSize = true;
            this.rb_Front.Location = new System.Drawing.Point(6, 65);
            this.rb_Front.Name = "rb_Front";
            this.rb_Front.Size = new System.Drawing.Size(68, 17);
            this.rb_Front.TabIndex = 2;
            this.rb_Front.TabStop = true;
            this.rb_Front.Text = "Спереди";
            this.toolTip1.SetToolTip(this.rb_Front, "Горячая клавиша W");
            this.rb_Front.UseVisualStyleBackColor = true;
            // 
            // rb_Bottom
            // 
            this.rb_Bottom.AutoSize = true;
            this.rb_Bottom.Location = new System.Drawing.Point(6, 42);
            this.rb_Bottom.Name = "rb_Bottom";
            this.rb_Bottom.Size = new System.Drawing.Size(55, 17);
            this.rb_Bottom.TabIndex = 1;
            this.rb_Bottom.Text = "Снизу";
            this.toolTip1.SetToolTip(this.rb_Bottom, "Горячая клавиша R");
            this.rb_Bottom.UseVisualStyleBackColor = true;
            // 
            // rb_Top
            // 
            this.rb_Top.AutoSize = true;
            this.rb_Top.Checked = true;
            this.rb_Top.Location = new System.Drawing.Point(6, 19);
            this.rb_Top.Name = "rb_Top";
            this.rb_Top.Size = new System.Drawing.Size(60, 17);
            this.rb_Top.TabIndex = 0;
            this.rb_Top.TabStop = true;
            this.rb_Top.Text = "Сверху";
            this.toolTip1.SetToolTip(this.rb_Top, "Горячая клавиша Q");
            this.rb_Top.UseVisualStyleBackColor = true;
            // 
            // gb_Direction
            // 
            this.gb_Direction.Controls.Add(this.rb_Symmetrically);
            this.gb_Direction.Controls.Add(this.rb_Back);
            this.gb_Direction.Controls.Add(this.rb_Straight);
            this.gb_Direction.Location = new System.Drawing.Point(141, 13);
            this.gb_Direction.Name = "gb_Direction";
            this.gb_Direction.Size = new System.Drawing.Size(106, 158);
            this.gb_Direction.TabIndex = 5;
            this.gb_Direction.TabStop = false;
            this.gb_Direction.Text = "Направление";
            this.toolTip1.SetToolTip(this.gb_Direction, "Направление выдавливания эскиза");
            // 
            // rb_Symmetrically
            // 
            this.rb_Symmetrically.AutoSize = true;
            this.rb_Symmetrically.Checked = true;
            this.rb_Symmetrically.Location = new System.Drawing.Point(6, 64);
            this.rb_Symmetrically.Name = "rb_Symmetrically";
            this.rb_Symmetrically.Size = new System.Drawing.Size(94, 17);
            this.rb_Symmetrically.TabIndex = 0;
            this.rb_Symmetrically.TabStop = true;
            this.rb_Symmetrically.Text = "Симметрично";
            this.toolTip1.SetToolTip(this.rb_Symmetrically, "Горячая клавиша D");
            this.rb_Symmetrically.UseVisualStyleBackColor = true;
            // 
            // rb_Back
            // 
            this.rb_Back.AutoSize = true;
            this.rb_Back.Location = new System.Drawing.Point(6, 41);
            this.rb_Back.Name = "rb_Back";
            this.rb_Back.Size = new System.Drawing.Size(74, 17);
            this.rb_Back.TabIndex = 0;
            this.rb_Back.TabStop = true;
            this.rb_Back.Text = "Обратное";
            this.toolTip1.SetToolTip(this.rb_Back, "Горячая клавиша S");
            this.rb_Back.UseVisualStyleBackColor = true;
            // 
            // rb_Straight
            // 
            this.rb_Straight.AutoSize = true;
            this.rb_Straight.Location = new System.Drawing.Point(6, 19);
            this.rb_Straight.Name = "rb_Straight";
            this.rb_Straight.Size = new System.Drawing.Size(65, 17);
            this.rb_Straight.TabIndex = 0;
            this.rb_Straight.TabStop = true;
            this.rb_Straight.Text = "Прямое";
            this.toolTip1.SetToolTip(this.rb_Straight, "Горячая клавиша A");
            this.rb_Straight.UseVisualStyleBackColor = true;
            // 
            // Form_CreatPartFromPos
            // 
            this.AcceptButton = this.b_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(257, 236);
            this.Controls.Add(this.gb_Direction);
            this.Controls.Add(this.gb_plane);
            this.Controls.Add(this.l_Thickness);
            this.Controls.Add(this.tb_Thickness);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Form_CreatPartFromPos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Задание параметров выдавливания";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_CreatPartFromPos_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form_CreatPartFromPos_KeyPress);
            this.gb_plane.ResumeLayout(false);
            this.gb_plane.PerformLayout();
            this.gb_Direction.ResumeLayout(false);
            this.gb_Direction.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_Ok;
        private System.Windows.Forms.Button b_Cancel;
        internal System.Windows.Forms.TextBox tb_Thickness;
        private System.Windows.Forms.Label l_Thickness;
        private System.Windows.Forms.RadioButton rb_Left;
        private System.Windows.Forms.RadioButton rb_Backside;
        internal System.Windows.Forms.RadioButton rb_Front;
        internal System.Windows.Forms.GroupBox gb_plane;
        internal System.Windows.Forms.RadioButton rb_Top;
        internal System.Windows.Forms.RadioButton rb_Bottom;
        private System.Windows.Forms.RadioButton rb_Symmetrically;
        internal System.Windows.Forms.RadioButton rb_Back;
        internal System.Windows.Forms.RadioButton rb_Straight;
        internal System.Windows.Forms.GroupBox gb_Direction;
        internal System.Windows.Forms.RadioButton rb_Right;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}