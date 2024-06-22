using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKompasLibrary.Data
{
    internal class Global
    {
        private static ArrayList eventList = new ArrayList();

        public static ArrayList EventList { get => eventList; set => eventList = value; }
    }
}
