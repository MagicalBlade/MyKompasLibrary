namespace MyKompasLibrary.Windows
{
    partial class Form_WriteMeasurementsInDimention
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
            this.b_OK = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.tb_NominalText = new System.Windows.Forms.TextBox();
            this.nud_TextUnder = new System.Windows.Forms.NumericUpDown();
            this.nud_Suffix = new System.Windows.Forms.NumericUpDown();
            this.tb_Suffix1 = new System.Windows.Forms.TextBox();
            this.tb_Suffix2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_TextUnder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Suffix)).BeginInit();
            this.SuspendLayout();
            // 
            // b_OK
            // 
            this.b_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.b_OK.Location = new System.Drawing.Point(69, 110);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(75, 23);
            this.b_OK.TabIndex = 2;
            this.b_OK.Text = "Записать";
            this.b_OK.UseVisualStyleBackColor = true;
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(150, 110);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 3;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            // 
            // tb_NominalText
            // 
            this.tb_NominalText.Location = new System.Drawing.Point(8, 37);
            this.tb_NominalText.Name = "tb_NominalText";
            this.tb_NominalText.ReadOnly = true;
            this.tb_NominalText.Size = new System.Drawing.Size(100, 20);
            this.tb_NominalText.TabIndex = 4;
            // 
            // nud_TextUnder
            // 
            this.nud_TextUnder.Location = new System.Drawing.Point(8, 78);
            this.nud_TextUnder.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nud_TextUnder.Minimum = new decimal(new int[] {
            1000000000,
            0,
            0,
            -2147483648});
            this.nud_TextUnder.Name = "nud_TextUnder";
            this.nud_TextUnder.Size = new System.Drawing.Size(100, 20);
            this.nud_TextUnder.TabIndex = 0;
            this.nud_TextUnder.Enter += new System.EventHandler(this.nud_TextUnder_Enter);
            // 
            // nud_Suffix
            // 
            this.nud_Suffix.Location = new System.Drawing.Point(150, 37);
            this.nud_Suffix.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nud_Suffix.Minimum = new decimal(new int[] {
            1000000000,
            0,
            0,
            -2147483648});
            this.nud_Suffix.Name = "nud_Suffix";
            this.nud_Suffix.Size = new System.Drawing.Size(100, 20);
            this.nud_Suffix.TabIndex = 1;
            this.nud_Suffix.Enter += new System.EventHandler(this.nud_Suffix_Enter);
            // 
            // tb_Suffix1
            // 
            this.tb_Suffix1.Location = new System.Drawing.Point(114, 37);
            this.tb_Suffix1.Name = "tb_Suffix1";
            this.tb_Suffix1.Size = new System.Drawing.Size(30, 20);
            this.tb_Suffix1.TabIndex = 5;
            this.tb_Suffix1.Text = "(";
            this.tb_Suffix1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_Suffix2
            // 
            this.tb_Suffix2.Location = new System.Drawing.Point(256, 37);
            this.tb_Suffix2.Name = "tb_Suffix2";
            this.tb_Suffix2.Size = new System.Drawing.Size(30, 20);
            this.tb_Suffix2.TabIndex = 5;
            this.tb_Suffix2.Text = ")";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Номинальный";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Фактический";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(166, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Отклонение";
            // 
            // Form_WriteMeasurementsInDimention
            // 
            this.AcceptButton = this.b_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(294, 145);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_Suffix2);
            this.Controls.Add(this.tb_Suffix1);
            this.Controls.Add(this.nud_Suffix);
            this.Controls.Add(this.nud_TextUnder);
            this.Controls.Add(this.tb_NominalText);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_WriteMeasurementsInDimention";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.nud_TextUnder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Suffix)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_OK;
        private System.Windows.Forms.Button b_Cancel;
        internal System.Windows.Forms.TextBox tb_NominalText;
        internal System.Windows.Forms.NumericUpDown nud_TextUnder;
        internal System.Windows.Forms.NumericUpDown nud_Suffix;
        internal System.Windows.Forms.TextBox tb_Suffix1;
        internal System.Windows.Forms.TextBox tb_Suffix2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}