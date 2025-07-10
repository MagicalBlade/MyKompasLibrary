using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKompasLibrary.Data
{
    internal static class Data_CreatPartFromPos
    {
        private static string rb_plane = "rb_Top";
        private static string rb_Direction = "rb_Symmetrically";
        public static double Thickness = 10;
        private static string thickness_str = "10";
        private static bool closeDrawing = false;
        private static bool close3D= false;
        private static string pathSave_m3d = "";

        public static string Rb_plane { get => rb_plane; set => rb_plane = value; }
        public static string Rb_Direction { get => rb_Direction; set => rb_Direction = value; }
        public static string Thickness_str { get => thickness_str; set => thickness_str = value; }
        public static bool CloseDrawing { get => closeDrawing; set => closeDrawing = value; }
        public static bool Close3D { get => close3D; set => close3D = value; }
        public static string PathSave_m3d { get => pathSave_m3d; set => pathSave_m3d = value; }
    }
}
