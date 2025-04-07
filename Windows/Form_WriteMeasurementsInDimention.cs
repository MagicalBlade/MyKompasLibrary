using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKompasLibrary.Windows
{
    public partial class Form_WriteMeasurementsInDimention: Form
    {
        public Form_WriteMeasurementsInDimention()
        {
            InitializeComponent();
        }

        private void nud_TextUnder_Enter(object sender, EventArgs e)
        {
            nud_TextUnder.Select(0, nud_TextUnder.Value.ToString().Length);
        }

        private void nud_Suffix_Enter(object sender, EventArgs e)
        {
            nud_Suffix.Select(0, nud_Suffix.Value.ToString().Length);
        }
    }
}
