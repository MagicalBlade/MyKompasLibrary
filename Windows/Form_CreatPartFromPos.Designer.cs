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
            this.b_Ok = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.tb_Thickness = new System.Windows.Forms.TextBox();
            this.l_Thickness = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_Top = new System.Windows.Forms.RadioButton();
            this.tb_Bottom = new System.Windows.Forms.RadioButton();
            this.tb_Front = new System.Windows.Forms.RadioButton();
            this.rb_Backside = new System.Windows.Forms.RadioButton();
            this.rb_Left = new System.Windows.Forms.RadioButton();
            this.tb_Right = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_Straight = new System.Windows.Forms.RadioButton();
            this.rb_Back = new System.Windows.Forms.RadioButton();
            this.rb_Symmetrically = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_Ok
            // 
            this.b_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.b_Ok.Location = new System.Drawing.Point(156, 266);
            this.b_Ok.Name = "b_Ok";
            this.b_Ok.Size = new System.Drawing.Size(75, 23);
            this.b_Ok.TabIndex = 0;
            this.b_Ok.Text = "Применить";
            this.b_Ok.UseVisualStyleBackColor = true;
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(290, 265);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 1;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            // 
            // tb_Thickness
            // 
            this.tb_Thickness.Location = new System.Drawing.Point(336, 209);
            this.tb_Thickness.Name = "tb_Thickness";
            this.tb_Thickness.Size = new System.Drawing.Size(100, 20);
            this.tb_Thickness.TabIndex = 2;
            // 
            // l_Thickness
            // 
            this.l_Thickness.AutoSize = true;
            this.l_Thickness.Location = new System.Drawing.Point(239, 216);
            this.l_Thickness.Name = "l_Thickness";
            this.l_Thickness.Size = new System.Drawing.Size(91, 13);
            this.l_Thickness.TabIndex = 3;
            this.l_Thickness.Text = "Толщина детали";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_Right);
            this.groupBox1.Controls.Add(this.rb_Left);
            this.groupBox1.Controls.Add(this.rb_Backside);
            this.groupBox1.Controls.Add(this.tb_Front);
            this.groupBox1.Controls.Add(this.tb_Bottom);
            this.groupBox1.Controls.Add(this.rb_Top);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 159);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
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
            this.rb_Top.UseVisualStyleBackColor = true;
            // 
            // tb_Bottom
            // 
            this.tb_Bottom.AutoSize = true;
            this.tb_Bottom.Location = new System.Drawing.Point(6, 42);
            this.tb_Bottom.Name = "tb_Bottom";
            this.tb_Bottom.Size = new System.Drawing.Size(55, 17);
            this.tb_Bottom.TabIndex = 1;
            this.tb_Bottom.Text = "Снизу";
            this.tb_Bottom.UseVisualStyleBackColor = true;
            // 
            // tb_Front
            // 
            this.tb_Front.AutoSize = true;
            this.tb_Front.Location = new System.Drawing.Point(6, 65);
            this.tb_Front.Name = "tb_Front";
            this.tb_Front.Size = new System.Drawing.Size(68, 17);
            this.tb_Front.TabIndex = 2;
            this.tb_Front.TabStop = true;
            this.tb_Front.Text = "Спереди";
            this.tb_Front.UseVisualStyleBackColor = true;
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
            this.rb_Backside.UseVisualStyleBackColor = true;
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
            this.rb_Left.UseVisualStyleBackColor = true;
            // 
            // tb_Right
            // 
            this.tb_Right.AutoSize = true;
            this.tb_Right.Location = new System.Drawing.Point(6, 134);
            this.tb_Right.Name = "tb_Right";
            this.tb_Right.Size = new System.Drawing.Size(62, 17);
            this.tb_Right.TabIndex = 2;
            this.tb_Right.TabStop = true;
            this.tb_Right.Text = "Справа";
            this.tb_Right.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_Symmetrically);
            this.groupBox2.Controls.Add(this.rb_Back);
            this.groupBox2.Controls.Add(this.rb_Straight);
            this.groupBox2.Location = new System.Drawing.Point(219, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 158);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
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
            this.rb_Straight.UseVisualStyleBackColor = true;
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
            this.rb_Back.UseVisualStyleBackColor = true;
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
            this.rb_Symmetrically.UseVisualStyleBackColor = true;
            // 
            // Form_CreatPartFromPos
            // 
            this.AcceptButton = this.b_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(570, 336);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.l_Thickness);
            this.Controls.Add(this.tb_Thickness);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Form_CreatPartFromPos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Задание параметров выдавливания";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_Ok;
        private System.Windows.Forms.Button b_Cancel;
        internal System.Windows.Forms.TextBox tb_Thickness;
        private System.Windows.Forms.Label l_Thickness;
        private System.Windows.Forms.RadioButton tb_Right;
        private System.Windows.Forms.RadioButton rb_Left;
        private System.Windows.Forms.RadioButton rb_Backside;
        internal System.Windows.Forms.RadioButton tb_Front;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.RadioButton rb_Top;
        internal System.Windows.Forms.RadioButton tb_Bottom;
        private System.Windows.Forms.RadioButton rb_Symmetrically;
        internal System.Windows.Forms.RadioButton rb_Back;
        internal System.Windows.Forms.RadioButton rb_Straight;
        internal System.Windows.Forms.GroupBox groupBox2;
    }
}