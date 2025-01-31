using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MyKompasLibrary.Windows
{
    public partial class Form_CreatPartFromPos : Form
    {
        private KeyEventArgs KeyEventArgs;
        public Form_CreatPartFromPos()
        {
            InitializeComponent();
        }

        private void Form_CreatPartFromPos_KeyDown(object sender, KeyEventArgs e)
        {
            KeyEventArgs = e;
        }

        private void Form_CreatPartFromPos_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (KeyEventArgs.KeyCode)
            {
                case Keys.Q:
                    rb_Top.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.W:
                    rb_Front.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.E:
                    rb_Left.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.R:
                    rb_Bottom.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.T:
                    rb_Backside.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.Y:
                    rb_Right.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.A:
                    rb_Straight.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.S:
                    rb_Back.Checked = true;
                    e.Handled = true;
                    break;
                case Keys.D:
                    rb_Symmetrically.Checked = true;
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

    }
}
