using Kompas6Constants;
using Kompas6API5;
using KompasAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Win32;
using System.Windows.Forms;
using MyKompasLibrary.Data;
using MyKompasLibrary.Event;
using System.IO;

namespace MyKompasLibrary
{
    public class Main
    {
        KompasObject Kompas;
        KompasObject KompasEvent;
        IApplication Application;
        IKompasDocument ActiveDocument;
        // Имя библиотеки
        [return: MarshalAs(UnmanagedType.BStr)]
        public string GetLibraryName()
        {
            return "Моя библиотека";
        }

        [return: MarshalAs(UnmanagedType.BStr)]

        #region Формируем меню команд
        public string ExternalMenuItem(short number, ref short itemType, ref short command)
        {
            string result = string.Empty;
            itemType = 1; // "MENUITEM"
            switch (number)
            {
                case 1:
                    result = "Закрыть не сохраняясь";
                    command = 1;
                    break;
                case 2:
                    result = "Создать Чертеж";
                    command = 1;
                    break;
                case 3:
                    result = "Создать Фрагмент";
                    command = 1;
                    break;
                case 4:
                    result = "Создать Деталь";
                    command = 1;
                    break;
                case 5:
                    result = "Создать Сборку";
                    command = 1;
                    break;
                case 6:
                    result = "Точка в центре окружности";
                    command = 1;
                    break;
                case 7:
                    result = "Скопировать название из штампа";
                    command = 1;
                    break;
                case 8:
                    command = -1;
                    itemType = 8; // "ENDMENU"
                    break;
            }
            return result;
        }

        #endregion

        #region Команды

        /// <summary>
        /// Закрыть документ не сохраняясь
        /// </summary>
        private void CloseNoSave()
        {
            ActiveDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
        }
        private void CreatDrawing()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentDrawing);
        }
        private void CreatFragment()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentFragment);
        }
        private void CreatPart()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentPart);
        }
        private void CreatAssemble()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentAssembly);
        }
        private void PointCenterCircle()
        {
            ksDocument2D document2DAPI5 = Kompas.ActiveDocument2D();
            IKompasDocument2D kompasDocument2D = (IKompasDocument2D)ActiveDocument;
            IKompasDocument2D1 kompasDocument2D1 = (IKompasDocument2D1)kompasDocument2D;
            ISelectionManager selectionManager = kompasDocument2D1.SelectionManager;
            Array array = selectionManager.SelectedObjects as Array;

            document2DAPI5.ksUndoContainer(true);
            if (array != null)
            {
                foreach (IDrawingObject obj in array)
                {
                    if (obj.DrawingObjectType == DrawingObjectTypeEnum.ksDrCircle)
                    {
                        ICircle circle = (ICircle)obj;
                        IViewsAndLayersManager viewsAndLayersManager = kompasDocument2D.ViewsAndLayersManager;
                        IViews views = viewsAndLayersManager.Views;
                        IView view = views.ActiveView;
                        IDrawingContainer drawingContainer = (IDrawingContainer)view;
                        IPoints points = drawingContainer.Points;
                        IPoint point = points.Add();
                        point.X = circle.Xc; point.Y = circle.Yc;
                        point.Update();
                    }
                }
            }
            document2DAPI5.ksUndoContainer(false);
        }
        private void CopyNameFromStamp()
        {
            IKompasDocument kompasDocument = Application.ActiveDocument;
            ILayoutSheets layoutSheets = kompasDocument.LayoutSheets;
            if (layoutSheets == null) return;
            if (layoutSheets.Count == 0) return;
            ILayoutSheet layoutSheet = layoutSheets.ItemByNumber[1];
            // Получение листа в старых версиях чертежа. В них видимо нет возможности получить лист по номеру листа.
            if (layoutSheet == null)
            {
                foreach (ILayoutSheet item in layoutSheets)
                {
                    layoutSheet = item;
                    break;
                }
            };
            IStamp stamp = layoutSheet.Stamp;
            if (stamp == null) return;
            IText text = stamp.Text[2];

            string plainText = $"{text.Str}";
            string htmlText = $"<table><tr><td>{text.Str}</td></tr></table>";
            Excel.CopyToExcel(plainText, htmlText);
            
        } 
        private void TestEvent()
        {
            IApplication applicationevent = KompasEvent.ksGetApplication7();
            ksDocument2D ksDocument2D = KompasEvent.ActiveDocument2D();
            
            //Kompas6API5.ksKompasObjectNotify_Event ksKompasObjectNotify = KompasEvent as Kompas6API5.ksKompasObjectNotify_Event;
            //ksKompasObjectNotify.OpenDocument += OpenDocument;

            //Kompas6API5.ksDocumentFileNotify_Event ksDocumentFileNotify = applicationevent.ActiveDocument as Kompas6API5.ksDocumentFileNotify_Event;
            //if (ksDocumentFileNotify == null) MessageBox.Show("Не подписался");
            //ksDocumentFileNotify.SaveDocument += SaveDocument;


            IKompasDocument2D kompasDocument2D = (IKompasDocument2D)applicationevent.ActiveDocument;
            IKompasDocument2D1 kompasDocument2D1 = (IKompasDocument2D1)applicationevent.ActiveDocument;
            IViewsAndLayersManager viewsAndLayersManager = kompasDocument2D.ViewsAndLayersManager;
            IViews views = viewsAndLayersManager.Views;
            IView view = views.ActiveView;
            IDrawingContainer drawingContainer = view as IDrawingContainer;
            ILineSegments lineSegments = drawingContainer.LineSegments;

            ISelectionManager selectionManager = kompasDocument2D1.SelectionManager;
            object select = selectionManager.SelectedObjects;

            //ksDrawingObjectNotify_Event ksDrawingObjectNotify_Event = select as ksDrawingObjectNotify_Event;
            //if(ksDrawingObjectNotify_Event == null) MessageBox.Show("Не подписался");
            //ksDrawingObjectNotify_Event.Delete += DeleteEvent;

            //ksObject2DNotify_Event ksObject2DNotify = ksDocument2D.GetObject2DNotify(0) as ksObject2DNotify_Event;
            //ksObject2DNotify.BeginDelete += BeginDelete;
            //applicationevent.ExecuteKompasCommand((int)ProcessTypeEnum.prPoint, true);
            ksProcess2DTypeEnum ksProcess2DTypeEnum = (ksProcess2DTypeEnum)ProcessTypeEnum.prLineSeg;
            MessageBox.Show($"{ksProcess2DTypeEnum}");

            IProcess2D process2D = kompasDocument2D1.LibProcess[ksProcess2DTypeEnum.ksProcess2DPlacement];



            IProcess process = process2D as IProcess;
            //IProcessParam processParam = applicationevent.CreateProcessParam();
            //ksProcess2DNotify_Event ksProcess2DNotify_Event = process2D as ksProcess2DNotify_Event;
            //if (ksProcess2DNotify_Event == null) MessageBox.Show("Не подписался");
            //ksProcess2DNotify_Event.Stop += RunEvent;
            process.Update();
            MessageBox.Show($"{process.Caption}");
            process.Run(true, true);

            MessageBox.Show("Подписался");
            

        }
        #endregion

        // Головная функция библиотеки
        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            Kompas = (KompasObject)kompas_;
            Application = (IApplication)Kompas.ksGetApplication7();
            ActiveDocument = Application.ActiveDocument;
            //Вызываем команды
            switch (command)
            {
                case 1: CloseNoSave(); break;
                case 2: CreatDrawing(); break;
                case 3: CreatFragment(); break;
                case 4: CreatPart(); break;
                case 5: CreatAssemble(); break;
                case 6: PointCenterCircle(); break; 
                case 7: CopyNameFromStamp(); break; 
                case 8: TestEvent(); break;
            }
        }



        public object ExternalGetResourceModule()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
        public bool LibInterfaceNotifyEntry(object kompas_)
        {
            KompasEvent = (KompasObject)kompas_;
            return true;
        }

        public string GetHelpFile()
        {
            if (File.Exists("c:\\Program Files\\ASCON\\KOMPAS-3D v20\\Libs\\MyKompasLibrary\\Help\\MyKompasLibrary.chm"))
            {
                return "c:\\Program Files\\ASCON\\KOMPAS-3D v20\\Libs\\MyKompasLibrary\\Help\\MyKompasLibrary.chm";
            }
            else
            {
                return "";
            }
        }

        #region Обработка событий

        public bool SaveDocument()
        {
            MessageBox.Show($"Попался");
            return true;
        }
        public bool DeleteEvent(object obj)
        {
            MessageBox.Show($"Попался");
            return true;
        }

        public bool RunEvent()
        {
            MessageBox.Show($"Попался");
            return true;
        }
        public bool OpenDocument(object newDoc, int docType)
        {
            MessageBox.Show("Открыт документ");
            return true;
        }
        #endregion


        public int ExternalGetToolBarId(short barType, short index)
        {
            int result = 0;

            if (barType == 0)
            {
                result = -1;
            }
            else
            {
                switch (index)
                {
                    case 1:
                        result = 3001;
                        break;
                    case 2:
                        result = -1;
                        break;
                }
            }

            return result;
        }


        #region COM Registration
        // Эта функция выполняется при регистрации класса для COM
        // Она добавляет в ветку реестра компонента раздел Kompas_Library,
        // который сигнализирует о том, что класс является приложением Компас,
        // а также заменяет имя InprocServer32 на полное, с указанием пути.
        // Все это делается для того, чтобы иметь возможность подключить
        // библиотеку на вкладке ActiveX.
        [ComRegisterFunction]
        public static void RegisterKompasLib(Type t)
        {
            try
            {
                RegistryKey regKey = Registry.LocalMachine;
                string keyName = @"SOFTWARE\Classes\CLSID\{" + t.GUID.ToString() + "}";
                regKey = regKey.OpenSubKey(keyName, true);
                regKey.CreateSubKey("Kompas_Library");
                regKey = regKey.OpenSubKey("InprocServer32", true);
                regKey.SetValue(null, System.Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\mscoree.dll");
                regKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("При регистрации класса для COM-Interop произошла ошибка:\n{0}", ex));
            }
        }

        // Эта функция удаляет раздел Kompas_Library из реестра
        [ComUnregisterFunction]
        public static void UnregisterKompasLib(Type t)
        {
            RegistryKey regKey = Registry.LocalMachine;
            string keyName = @"SOFTWARE\Classes\CLSID\{" + t.GUID.ToString() + "}";
            RegistryKey subKey = regKey.OpenSubKey(keyName, true);
            subKey.DeleteSubKey("Kompas_Library");
            subKey.Close();
        }
        #endregion
    }
}
