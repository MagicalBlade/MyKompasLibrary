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
using System.Diagnostics;
using Kompas6Constants3D;
using System.Text.RegularExpressions;
using MyKompasLibrary.Windows;

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
        
        /// <summary>
        /// Создать чертеж
        /// </summary>
        private void CreatDrawing()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentDrawing);
        }
        
        /// <summary>
        /// Создать фрагмент
        /// </summary>
        private void CreatFragment()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentFragment);
        }
        
        /// <summary>
        /// Создать 3D деталь
        /// </summary>
        private void CreatPart()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentPart);
        }
        
        /// <summary>
        /// Создать 3D сборку
        /// </summary>
        private void CreatAssemble()
        {
            IDocuments documents = Application.Documents;
            documents.Add(DocumentTypeEnum.ksDocumentAssembly);
        }

        /// <summary>
        /// Создать точку в центре окружности
        /// </summary>
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
        
        /// <summary>
        /// Копирование названия чертежа из штампа
        /// </summary>
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

        /// <summary>
        /// Создать 3D деталь из деталировки
        /// </summary>
        private void CreatPartFromPos()
        {
            bool saveYesNo = true;
            string pathSavePDF = "";
            if (Application.ActiveDocument.Type != KompasAPIObjectTypeEnum.ksObjectDrawingDocument) return;
            IDocuments documents = Application.Documents;
            IKompasDocument kompasDocument = Application.ActiveDocument;
            ksDocument2D ksdocument2D = Kompas.ActiveDocument2D();

            #region Получение толщины и имени позиции из штампа
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

            const string pattern = "[^\\d\\.,-]";
            string namePos = stamp.Text[2].Str; //Ячейка позиции
            namePos = Regex.Replace(namePos, pattern, "");
            namePos = namePos.Trim('.');

            string stampid3 = stamp.Text[3].Str;//Ячейка с толщиной, материалом и т.д.
            if (stampid3 != "")
            {
                string[] profile = stampid3.Split("$dsm; ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (profile.Length > 4)
                {
                    Data_CreatPartFromPos.Thickness_str = profile[1];
                }
            }
            #endregion

            #region Получание адреса папки 3D
            ILibraryManager libraryManager = Application.LibraryManager;
            string pathlibrary = $"{Path.GetDirectoryName(libraryManager.CurrentLibrary.PathName)}"; //Получить путь к папке библиотеки
            string pathAdressesFolderBZMMK = $"{pathlibrary}\\Resources\\Адреса основных папок БЗММК.txt";
            if (!File.Exists(pathAdressesFolderBZMMK))
            {
                MessageBox.Show("Не найден файл с адресом к папке \"3D\" Обратитесь к разработчику.");
                return;
            }
            string readAdresses = "";
            using (StreamReader sr = new StreamReader(pathAdressesFolderBZMMK))
            {
                readAdresses = sr.ReadToEnd();
            }
            if (readAdresses == "")
            {
                MessageBox.Show($"Неудалось прочитать файл с адресами папок. Обратитесь к разработчику.");
                return;
            }
            Dictionary<string, string> adresess = new Dictionary<string, string>();
            foreach (string line in readAdresses.Split('\n'))
            {
                string[] temp = line.Split(':').Select(x => x.Trim()).ToArray();
                if (temp.Length != 2) break;
                adresess.Add(temp[0], temp[1]);
            }
            if (!adresess.ContainsKey("3D"))
            {
                MessageBox.Show($"Не найден путь к папке \"3D\". Обратитесь к разработчику.");
                return;
            }
            if (!adresess.ContainsKey("3D архив"))
            {
                saveYesNo = false;
                return;
            }
            #endregion

            #region Создание пути детали и проверка существование файла по этому пути
            string nameorder = Array.Find(kompasDocument.PathName.Split('\\'), x => x.IndexOf("З.з.№", StringComparison.CurrentCultureIgnoreCase) != -1);
            string pathFolderSavePDF = "";
            if (Directory.Exists($"{adresess["3D"]}\\{nameorder}"))
            {
                pathFolderSavePDF = $"{adresess["3D"]}\\{nameorder}";
            }
            else if (Directory.Exists($"{adresess["3D архив"]}\\{nameorder}"))
            {
                pathFolderSavePDF = $"{adresess["3D архив"]}\\{nameorder}";
            }
            else
            {
                MessageBox.Show($"Не найдена папка заказа в 3D. 3D деталь не сохранена.");
                return;
            }
            pathSavePDF = $"{pathFolderSavePDF}\\2_Деталировка\\{namePos}.m3d";
            if (File.Exists(pathSavePDF))
            {
                if (Kompas.ksYesNo($"Файл с именем {pathSavePDF} уже существует. Продолжить создание? Файл будет заменен!") != 1) return;
            } 
            #endregion

            //Получение координат нулевой точки детали
            double selectX = 0;
            double selectY = 0;
            if (ksdocument2D.ksCursor(null, ref selectX, ref selectY, null) != -1) return;
            //Получаем контур детали который будет передан в 3D деталь
            ksInertiaParam ksinertiaParam = Kompas.GetParamStruct(83); //Параметры МЦХ
            int group = ksdocument2D.ksViewGetObjectArea(); //Контур площади
            if (group == 0)
            {
                return;
            }
            //Перемещаем группу, за указанную точку, в начало координат
            ksdocument2D.ksMoveObj(group, - selectX, - selectY);
            //Копируем группу в буфер обмена
            ksdocument2D.ksWriteGroupToClip(group, true);

            Form_CreatPartFromPos form_CreatPartFromPos = new Form_CreatPartFromPos();
            //Задание начальных параметров
            form_CreatPartFromPos.tb_Thickness.Text = Data_CreatPartFromPos.Thickness_str;
            form_CreatPartFromPos.tb_Front.Checked = true;
            if (form_CreatPartFromPos.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            //Получение толщины детали и приведение к числу
            Data_CreatPartFromPos.Thickness_str = form_CreatPartFromPos.tb_Thickness.Text;
            if (!double.TryParse(Data_CreatPartFromPos.Thickness_str, out Data_CreatPartFromPos.Thickness))
            {
                MessageBox.Show("Не верно указана толщина детали!");
                return;
            }
            switch (form_CreatPartFromPos.groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Top":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeXOY;
                    Data_CreatPartFromPos.LeftHandedCS = false;
                    break;
                case "tb_Bottom":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeXOZ;
                    Data_CreatPartFromPos.LeftHandedCS = true;
                    break;
                case "tb_Front":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeYOZ;
                    Data_CreatPartFromPos.LeftHandedCS = true;
                    break;
                case "rb_Backside":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeXOZ;
                    Data_CreatPartFromPos.LeftHandedCS = false;
                    break;
                case "rb_Left":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeXOZ;
                    Data_CreatPartFromPos.LeftHandedCS = true;
                    break;
                case "tb_Right":
                    Data_CreatPartFromPos.Plan = ksObj3dTypeEnum.o3d_planeXOZ;
                    Data_CreatPartFromPos.LeftHandedCS = false;
                    break;
                default:
                    break;
            }



            //Создаем 3D деталь
            IKompasDocument kompasDocumentCreated = documents.AddWithDefaultSettings(DocumentTypeEnum.ksDocumentPart, true);
            IKompasDocument3D kompasDocument3D = kompasDocumentCreated as IKompasDocument3D;
            IPart7 part7 = kompasDocument3D.TopPart;
            part7.Name = namePos;
            IModelContainer modelContainer = (IModelContainer)part7;
            ISketchs sketchs = modelContainer.Sketchs;
            Sketch sketch = sketchs.Add();
            //TODO Запрос у пользователя на какой плоскости выдавливание делать и какое направление или симметрия
            //Выбор плоскости выдавливания
            sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOX] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOY];
            sketch.LeftHandedCS = Data_CreatPartFromPos.LeftHandedCS;
            sketch.Plane = part7.DefaultObject[Data_CreatPartFromPos.Plan] as IPlane3D;
            part7.Update();
            //Начало формирования эскиза
            IKompasDocument sketch_KD = sketch.BeginEdit();
            IKompasDocument2D1 sketch_2D1 = sketch_KD as IKompasDocument2D1;
            IDrawingGroups drawingGroups = sketch_2D1.DrawingGroups;
            IDrawingGroup drawingGroup = drawingGroups.Add(true, "");
            //Считываем из буфера обмена в группу
            drawingGroup.ReadFromClip(false, false);
            //Вставляем группу
            drawingGroup.Store();
            //Закончили формировать эскиз
            sketch.EndEdit();
            sketch.Update();
            IExtrusions extrusions = modelContainer.Extrusions;
            IExtrusion extrusion = extrusions.Add(ksObj3dTypeEnum.o3d_bossExtrusion);

            switch (form_CreatPartFromPos.groupBox2.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Straight":
                    extrusion.Direction = ksDirectionTypeEnum.dtNormal;
                    break;
                case "rb_Back":
                    extrusion.Direction = ksDirectionTypeEnum.dtReverse;
                    break;
                case "rb_Symmetrically":
                    extrusion.Direction = ksDirectionTypeEnum.dtMiddlePlane;
                    break;
                default:
                    break;
            }
            extrusion.Name = $"t{Data_CreatPartFromPos.Thickness}";
            extrusion.Sketch = sketch;
            if (Data_CreatPartFromPos.Thickness == 0)
            {
                MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                extrusion.Depth[true] = Data_CreatPartFromPos.Thickness; //Толщина выдавливания
            }
            else
            {
                extrusion.Depth[true] = Data_CreatPartFromPos.Thickness; //Толщина выдавливания
            }

            if (!extrusion.Update())
            {
                Application.MessageBoxEx("Не удалось выдавить", "Ошибка", 64);
                return;
            }
            if (saveYesNo)
            {
                kompasDocument3D.SaveAs(pathSavePDF);
                if (kompasDocument3D.Name == "")
                {
                    MessageBox.Show("Не удалось сохранить файл. Файл или открыть или нет прав на его изменение");
                }
            }
            else
            {
                Application.MessageBoxEx("Не найдет папка с заказом в 3D", "Ошибка", 64);
            }
            Application.MessageBoxEx("Создание детали завершено", "Готово", 64);
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


        /// <summary>
        /// Открытие файла помощи
        /// </summary>
        private void OpenHelp()
        {
            ILibraryManager libraryManager = Application.LibraryManager;
            string path = $"{Path.GetDirectoryName(libraryManager.CurrentLibrary.PathName)}\\Help\\index.html"; //Получить путь к папке библиотеки
            if (File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                Application.MessageBoxEx("Файл помощи не найден. Обратитесь к разработчику", "Ошибка", 64);
            }
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
                case 8: CreatPartFromPos(); break;
                case 9: TestEvent(); break;
                
                
                case 999: OpenHelp(); break;
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
