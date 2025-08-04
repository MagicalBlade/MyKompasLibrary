using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKompasLibrary.Windows.OpenPart
{
    public partial class SearchFile : Form
    {
        private string fileSearchDirectory;
        public string FileSearchDirectory { get => fileSearchDirectory; set => fileSearchDirectory = value; }
        
        public SearchFile()
        {
            InitializeComponent();
        }
        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            FilterFile();
        }
        private void SearchFile_Shown(object sender, EventArgs e)
        {
            FilterFile();
        }
        private void lb_Files_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lb_Files.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FilterFile()
        {
            if (!Directory.Exists(FileSearchDirectory))
            {
                MessageBox.Show($"Не найдена папка.\nПо адресу: {FileSearchDirectory}");
                return;
            }
            string patern = tb_search.Text.Replace("*", ".*");
            lb_Files.DataSource = Directory.GetFiles(FileSearchDirectory, "*", SearchOption.AllDirectories)
                              .Where(n => Regex.IsMatch(n, $@"\b{patern}\b.*(.cdw|.frw)", RegexOptions.IgnoreCase))
                              .Select(n => new PathFile { Path = n, FileName = Path.GetFileName(n) }).ToArray();
        }
        public class PathFile
        {
            private string _path;
            private string _fileName;

            public string Path { get => _path; set => _path = value; }
            public string FileName { get => _fileName; set => _fileName = value; }
        }

    }
}
