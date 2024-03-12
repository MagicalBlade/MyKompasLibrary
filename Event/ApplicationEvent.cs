using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKompasLibrary.Event
{
    static internal class ApplicationEvent
    {
        static public bool opendocument = false;

        static public void OpenDocumentSubscribe()
        {
            opendocument = true;
        }
        static public void OpenDocumentUnSubscribe()
        {
            opendocument = false;
        }
    }
}
