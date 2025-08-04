namespace MyKompasLibrary.Windows.OpenPart
{
    partial class SearchFile
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
            this.label1 = new System.Windows.Forms.Label();
            this.tb_search = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_Files = new System.Windows.Forms.ListBox();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.b_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Наименование марки/позиции:";
            // 
            // tb_search
            // 
            this.tb_search.Location = new System.Drawing.Point(13, 31);
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(300, 20);
            this.tb_search.TabIndex = 6;
            this.tb_search.TextChanged += new System.EventHandler(this.tb_search_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 57);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Список файлов:";
            // 
            // lb_Files
            // 
            this.lb_Files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Files.DisplayMember = "FileName";
            this.lb_Files.FormattingEnabled = true;
            this.lb_Files.HorizontalScrollbar = true;
            this.lb_Files.Location = new System.Drawing.Point(13, 76);
            this.lb_Files.Name = "lb_Files";
            this.lb_Files.Size = new System.Drawing.Size(300, 173);
            this.lb_Files.TabIndex = 7;
            this.lb_Files.ValueMember = "FileName";
            this.lb_Files.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lb_Files_MouseDoubleClick);
            // 
            // b_Cancel
            // 
            this.b_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(166, 266);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 10;
            this.b_Cancel.Text = "Отмена";
            this.b_Cancel.UseVisualStyleBackColor = true;
            // 
            // b_OK
            // 
            this.b_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.b_OK.Location = new System.Drawing.Point(85, 266);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(75, 23);
            this.b_OK.TabIndex = 9;
            this.b_OK.Text = "OK";
            this.b_OK.UseVisualStyleBackColor = true;
            // 
            // SearchFile
            // 
            this.AcceptButton = this.b_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(327, 301);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_OK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_Files);
            this.Controls.Add(this.tb_search);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(343, 264);
            this.Name = "SearchFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск файла";
            this.Shown += new System.EventHandler(this.SearchFile_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ListBox lb_Files;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Button b_OK;
        internal System.Windows.Forms.TextBox tb_search;
    }
}