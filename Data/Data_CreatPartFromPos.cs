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
        public static double Thickness = 10;

        private static ksObj3dTypeEnum plan = ksObj3dTypeEnum.o3d_planeXOY;
        private static string thickness_str = "10";
        private static bool leftHandedCS = false;

        public static ksObj3dTypeEnum Plan { get => plan; set => plan = value; }
        public static string Thickness_str { get => thickness_str; set => thickness_str = value; }
        public static bool LeftHandedCS { get => leftHandedCS; set => leftHandedCS = value; }
    }
}
