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

namespace MyKompasLibrary
{
    public class Main
    {
        KompasObject kompas;
        IApplication application;
        IKompasDocument activeDocument;
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

        /// <summary>
        /// Закрыть документ не сохраняясь
        /// </summary>
        private void CloseNoSave()
        {
            activeDocument.Close(DocumentCloseOptions.kdDoNotSaveChanges);
        }
        private void CreatDrawing()
        {
            IDocuments documents = application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentDrawing);
        }
        private void CreatFragment()
        {
            IDocuments documents = application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentFragment);
        }
        private void CreatPart()
        {
            IDocuments documents = application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentPart);
        }
        private void CreatAssemble()
        {
            IDocuments documents = application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentAssembly);
        }
        private void PointCenterCircle()
        {
            ksDocument2D document2DAPI5 = kompas.ActiveDocument2D();
            IKompasDocument2D kompasDocument2D = (IKompasDocument2D)activeDocument;
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
            IKompasDocument kompasDocument = application.ActiveDocument;
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

        // Головная функция библиотеки
        public void ExternalRunCommand([In] short command, [In] short mode, [In, MarshalAs(UnmanagedType.IDispatch)] object kompas_)
        {
            kompas = (KompasObject)kompas_;
            application = (IApplication)kompas.ksGetApplication7();
            activeDocument = application.ActiveDocument;
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
            }
        }



        public object ExternalGetResourceModule()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

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
