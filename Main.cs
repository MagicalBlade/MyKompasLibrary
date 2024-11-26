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
                MessageBox.Show($"Не найден путь к папке \"3D архив\". Обратитесь к разработчику.");
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
            //Запуск диалога
            Form_CreatPartFromPos form_CreatPartFromPos = new Form_CreatPartFromPos();
            //Задание начальных параметров
            form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Name == $"{Data_CreatPartFromPos.Rb_plane}").Checked = true;
            form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Name == $"{Data_CreatPartFromPos.Rb_Direction}").Checked = true;
            form_CreatPartFromPos.tb_Thickness.Text = Data_CreatPartFromPos.Thickness_str;
            if (form_CreatPartFromPos.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Data_CreatPartFromPos.Rb_plane = form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name;
            Data_CreatPartFromPos.Rb_Direction = form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name;
            //Получение толщины детали и приведение к числу
            Data_CreatPartFromPos.Thickness_str = form_CreatPartFromPos.tb_Thickness.Text;
            if (!double.TryParse(Data_CreatPartFromPos.Thickness_str, out Data_CreatPartFromPos.Thickness))
            {
                MessageBox.Show("Не верно указана толщина детали!");
                return;
            }
            //Создаем 3D деталь
            IKompasDocument kompasDocumentCreated = documents.AddWithDefaultSettings(DocumentTypeEnum.ksDocumentPart, true);
            IKompasDocument3D kompasDocument3D = kompasDocumentCreated as IKompasDocument3D;
            IPart7 part7 = kompasDocument3D.TopPart;
            part7.Name = namePos;
            IModelContainer modelContainer = (IModelContainer)part7;
            ISketchs sketchs = modelContainer.Sketchs;
            Sketch sketch = sketchs.Add();
            //Задание плоскости выдавливания
            switch (form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Top":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOX] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOY];
                    sketch.LeftHandedCS = false;
                    break;
                case "rb_Bottom":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOX];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Front":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeYOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Backside":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = false;
                    break;
                case "rb_Left":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Right":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = false;
                    break;
                default:
                    break;
            }
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
            //Задание направления выдавливания
            switch (form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Straight":
                    extrusion.Direction = ksDirectionTypeEnum.dtNormal;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                case "rb_Back":
                    extrusion.Direction = ksDirectionTypeEnum.dtReverse;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[false] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[false] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                case "rb_Symmetrically":
                    extrusion.Direction = ksDirectionTypeEnum.dtMiddlePlane;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                default:
                    break;
            }
            extrusion.Name = $"t{Data_CreatPartFromPos.Thickness}";
            extrusion.Sketch = sketch;
            if (!extrusion.Update())
            {
                Application.MessageBoxEx("Не удалось выдавить", "Ошибка", 64);
                return;
            }
            kompasDocument3D.SaveAs(pathSavePDF);
            if (kompasDocument3D.Name == "")
            {
                MessageBox.Show("Не удалось сохранить файл. Файл или открыть или нет прав на его изменение");
                return;
            }
            Application.MessageBoxEx("Создание детали завершено", "Готово", 64);
        }

        /// <summary>
        /// Создать 3D деталь из деталировки с панелью свойств
        /// </summary>
        private void CreatPartFromPos_PropertyTab()
        {
            string pathSavePDF = "";
            if (Application.ActiveDocument.Type != KompasAPIObjectTypeEnum.ksObjectDrawingDocument) return;
            IDocuments documents = Application.Documents;
            IKompasDocument kompasDocument = Application.ActiveDocument;
            IKompasDocument2D1 kompasDocument2D1 = Application.ActiveDocument as IKompasDocument2D1;
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
                MessageBox.Show($"Не найден путь к папке \"3D архив\". Обратитесь к разработчику.");
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

            #region Создание панели свойств
            IProcess2D process2D = kompasDocument2D1.LibProcess[ksProcess2DTypeEnum.ksProcess2DPlacement];
            IProcess process = process2D as IProcess;
            process.Run(true, true);
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
            ksdocument2D.ksMoveObj(group, -selectX, -selectY);
            //Копируем группу в буфер обмена
            ksdocument2D.ksWriteGroupToClip(group, true);
            //Запуск диалога
            Form_CreatPartFromPos form_CreatPartFromPos = new Form_CreatPartFromPos();
            //Задание начальных параметров
            form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Name == $"{Data_CreatPartFromPos.Rb_plane}").Checked = true;
            form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Name == $"{Data_CreatPartFromPos.Rb_Direction}").Checked = true;
            form_CreatPartFromPos.tb_Thickness.Text = Data_CreatPartFromPos.Thickness_str;
            if (form_CreatPartFromPos.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Data_CreatPartFromPos.Rb_plane = form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name;
            Data_CreatPartFromPos.Rb_Direction = form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name;
            //Получение толщины детали и приведение к числу
            Data_CreatPartFromPos.Thickness_str = form_CreatPartFromPos.tb_Thickness.Text;
            if (!double.TryParse(Data_CreatPartFromPos.Thickness_str, out Data_CreatPartFromPos.Thickness))
            {
                MessageBox.Show("Не верно указана толщина детали!");
                return;
            }
            //Создаем 3D деталь
            IKompasDocument kompasDocumentCreated = documents.AddWithDefaultSettings(DocumentTypeEnum.ksDocumentPart, true);
            IKompasDocument3D kompasDocument3D = kompasDocumentCreated as IKompasDocument3D;
            IPart7 part7 = kompasDocument3D.TopPart;
            part7.Name = namePos;
            IModelContainer modelContainer = (IModelContainer)part7;
            ISketchs sketchs = modelContainer.Sketchs;
            Sketch sketch = sketchs.Add();
            //Задание плоскости выдавливания
            switch (form_CreatPartFromPos.gb_plane.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Top":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOY] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOX] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOY];
                    sketch.LeftHandedCS = false;
                    break;
                case "rb_Bottom":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOX];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Front":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeYOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Backside":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = false;
                    break;
                case "rb_Left":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = true;
                    break;
                case "rb_Right":
                    sketch.Plane = part7.DefaultObject[ksObj3dTypeEnum.o3d_planeXOZ] as IPlane3D;
                    sketch.DirectingObject[ksObj3dTypeEnum.o3d_axisOY] = part7.DefaultObject[ksObj3dTypeEnum.o3d_axisOZ];
                    sketch.LeftHandedCS = false;
                    break;
                default:
                    break;
            }
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
            //Задание направления выдавливания
            switch (form_CreatPartFromPos.gb_Direction.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked).Name)
            {
                case "rb_Straight":
                    extrusion.Direction = ksDirectionTypeEnum.dtNormal;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                case "rb_Back":
                    extrusion.Direction = ksDirectionTypeEnum.dtReverse;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[false] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[false] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                case "rb_Symmetrically":
                    extrusion.Direction = ksDirectionTypeEnum.dtMiddlePlane;
                    if (Data_CreatPartFromPos.Thickness == 0)
                    {
                        MessageBox.Show("Не указана толщина. Выдавливание произведено с толщиной равной десяти.");
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    else
                    {
                        extrusion.Depth[true] = Data_CreatPartFromPos.Thickness;
                    }
                    break;
                default:
                    break;
            }
            extrusion.Name = $"t{Data_CreatPartFromPos.Thickness}";
            extrusion.Sketch = sketch;
            if (!extrusion.Update())
            {
                Application.MessageBoxEx("Не удалось выдавить", "Ошибка", 64);
                return;
            }
            kompasDocument3D.SaveAs(pathSavePDF);
            if (kompasDocument3D.Name == "")
            {
                MessageBox.Show("Не удалось сохранить файл. Файл или открыть или нет прав на его изменение");
                return;
            }
            Application.MessageBoxEx("Создание детали завершено", "Готово", 64);
        }

        private void TeklaToKompas()
        {
            int styleLine = -1;
            IKompasDocument2D kompasDocument2D = (IKompasDocument2D)Application.ActiveDocument;
            ksDocument2D activeDocumentAPI5 = Kompas.ActiveDocument2D();
            IKompasDocument2D1 kompasDocument2D1 = (IKompasDocument2D1)kompasDocument2D;
            ISelectionManager selectionManager = kompasDocument2D1.SelectionManager;
            dynamic selected = selectionManager.SelectedObjects;

            IStylesManager stylesManager = kompasDocument2D as IStylesManager;
            IStyles styles = stylesManager.CurvesStyles;

            activeDocumentAPI5.ksUndoContainer(true);
            if (selected is object[])
            {
                foreach (var item in selected)
                {
                    if (item is IDrawingObject1 drawingobject1)
                    {
                        IDrawingObject drawingobject = drawingobject1 as IDrawingObject;
                        if (drawingobject1.IsGeometryObject)
                        {
                            switch (drawingobject.Type)
                            {
                                case KompasAPIObjectTypeEnum.ksObjectArc:
                                    {
                                        IArc temp = (IArc)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectBeziers:
                                    {
                                        IBezier temp = (IBezier)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectCircle:
                                    {
                                        ICircle temp = (ICircle)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectConicCurve:
                                    {
                                        IConicCurve temp = (IConicCurve)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectDrawingContour:
                                    {
                                        IDrawingContour temp = (IDrawingContour)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEllipse:
                                    {
                                        IEllipse temp = (IEllipse)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEllipseArc:
                                    {
                                        IEllipseArc temp = (IEllipseArc)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEquidistant:
                                    {
                                        IEquidistant temp = (IEquidistant)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectLineSegment:
                                    {
                                        ILineSegment temp = (ILineSegment)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectNurbs:
                                    {
                                        INurbs temp = (INurbs)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectPolyLine2D:
                                    {
                                        IPolyLine2D temp = (IPolyLine2D)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectRectangle:
                                    {
                                        IRectangle temp = (IRectangle)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                                case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectRegularPolygon:
                                    {
                                        IRegularPolygon temp = (IRegularPolygon)drawingobject;
                                        temp.Style = GetTypeLine(temp.Style);
                                        temp.Update();
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            else if (selected == null)
            {
                Application.MessageBoxEx("Выберите элемент/элементы", "Ошибка", 64);
                return;
            }
            else
            {
                if (selected is IDrawingObject1 drawingobject1)
                {
                    IDrawingObject drawingobject = drawingobject1 as IDrawingObject;
                    if (drawingobject1.IsGeometryObject)
                    {
                        switch (drawingobject.Type)
                        {
                            case KompasAPIObjectTypeEnum.ksObjectArc:
                                {
                                    IArc temp = (IArc)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectBeziers:
                                {
                                    IBezier temp = (IBezier)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectCircle:
                                {
                                    ICircle temp = (ICircle)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectConicCurve:
                                {
                                    IConicCurve temp = (IConicCurve)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectDrawingContour:
                                {
                                    IDrawingContour temp = (IDrawingContour)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEllipse:
                                {
                                    IEllipse temp = (IEllipse)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEllipseArc:
                                {
                                    IEllipseArc temp = (IEllipseArc)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectEquidistant:
                                {
                                    IEquidistant temp = (IEquidistant)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectLineSegment:
                                {
                                    ILineSegment temp = (ILineSegment)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectNurbs:
                                {
                                    INurbs temp = (INurbs)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectPolyLine2D:
                                {
                                    IPolyLine2D temp = (IPolyLine2D)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectRectangle:
                                {
                                    IRectangle temp = (IRectangle)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                            case Kompas6Constants.KompasAPIObjectTypeEnum.ksObjectRegularPolygon:
                                {
                                    IRegularPolygon temp = (IRegularPolygon)drawingobject;
                                    styleLine = temp.Style;
                                    break;
                                }
                        }
                    }
                }
                MessageBox.Show($"{styleLine}");
                IStyle style1 = styles.StyleByApiId[styleLine];
                Clipboard.SetText($"{style1?.Name}");
            }
            activeDocumentAPI5.ksUndoContainer(false);

            int GetTypeLine(int typeTekla)
            {
                int result = typeTekla;
                IStyle style = styles.StyleByApiId[typeTekla];
                //Есть две библиотеки стилей: стандартная и документа. Стили которые необходимо заменить на стандартные получаются из IKompasDocument
                //а стандартные из IApplication. В данном при попытке получить стандартный стиль из styles вернется null.
                switch (style?.Name)
                {
                    case "Continuous_RGB_0_0_255_THICK_0.18":
                        result = 1;//Основная линия
                        break;
                    #region Тонкие линии
                    case "Continuous_RGB_0_0_0_THICK_0.18":
                        result = 2;
                        break;
                    case "Continuous_RGB_127_0_31_THICK_0.18":
                        result = 2;
                        break;
                    case "Continuous_RGB_0_127_0_THICK_0.18":
                        result = 2;
                        break;
                    case "Continuous_RGB_0_255_255_THICK_0.18":
                        result = 2;
                        break;
                    case "Continuous_RGB_0_255_0_THICK_0.18":
                        result = 2;
                        break;
                    case "POLYLINE_STYLE_0.18_RGB_0_127_0":
                        result = 2;
                        break;
                    #endregion
                        //Осевая линия
                    case "DXK_LINE_DOT3_RGB_0_255_255_SC_1_THICK_0.18":
                        result = 3;
                        break;
                    #region Штриховая линия
                    case "DXK_LINE_DOT2_RGB_0_255_255_SC_1_THICK_0.18":
                        result = 4;
                        break;
                    case "DXK_LINE_DOT1_RGB_0_255_0_SC_1_THICK_0.18":
                        result = 4;
                        break;
                    case "DXK_LINE_DOT2_RGB_0_255_0_SC_1_THICK_0.18":
                        result = 4;
                        break;
                    case "DXK_LINE_DOT2_RGB_0_0_0_SC_1_THICK_0.18":
                        result = 4;
                        break;
                    #endregion
                    default:
                        break; 
                }
                return result;
            }
            Application.MessageBoxEx("Стили заменены", "Готово", 64);
        }

        /// <summary>
        /// Запись отклонений в размер
        /// </summary>
        private void WriteToleranceDimention()
        {
            IKompasDocument2D1 kompasDocument2D1 = (IKompasDocument2D1)ActiveDocument;
            ISelectionManager selectionManager = kompasDocument2D1.SelectionManager;
            dynamic selectdynamic = selectionManager.SelectedObjects;
            if (selectdynamic == null) return;
            if (selectdynamic is object[]) return;
            IKompasAPIObject kompasobject = selectdynamic as IKompasAPIObject;
            if (kompasobject.Type != KompasAPIObjectTypeEnum.ksObjectLineDimension) return;
            ILineDimension lineDimension = kompasobject as ILineDimension;
            IDimensionText dimensionText = lineDimension as IDimensionText;
            if (dimensionText.TextUnder.Str == "") return;
            if (!double.TryParse(dimensionText.TextUnder.Str, out double under))
            {
                Application.MessageBoxEx("Не получилось преобразовать нижний текст","Ошибка", 64);
                return;
            }
            if (!double.TryParse(dimensionText.NominalText.Str, out double nominal))
            {
                Application.MessageBoxEx("Не получилось преобразовать текст размера", "Ошибка", 64);
                return;
            }
            double tolerance = Math.Round(under - nominal);
            dimensionText.Suffix.Str = $"({(nominal < 0 ? "" : "+")}{tolerance})";
            lineDimension.Update();
        }


        /// <summary>
        /// Запись отклонений в выноску
        /// </summary>
        private void WriteToleranceLeader()
        {
            IKompasDocument2D1 kompasDocument2D1 = (IKompasDocument2D1)ActiveDocument;
            ISelectionManager selectionManager = kompasDocument2D1.SelectionManager;
            dynamic selectdynamic = selectionManager.SelectedObjects;
            if (selectdynamic == null) return;
            if (!(selectdynamic is object[])) {
                Application.MessageBoxEx("Выделите две выноски","", 64);
                return;
            }
            if(selectdynamic.Length != 2)
            {
                Application.MessageBoxEx("Выделите только две выноски!", "", 64);
                return;
            }

            ILeader leader = selectdynamic[0] as ILeader;
            ILeader leader1 = selectdynamic[1] as ILeader;
            if (leader == null || leader1 == null)
            {
                Application.MessageBoxEx("Выделите линии выноски","", 64);
                return;
            }

            if (!double.TryParse(leader1.TextOnShelf.Str, out double on))
            {
                Application.MessageBoxEx("Не получилось перести в число текст над полкой", "", 64);
                return;
            }

            if (!double.TryParse(leader1.TextUnderShelf.Str, out double under))
            {
                Application.MessageBoxEx("Не получилось перести в число текст под полкой", "", 64);
                return;
            }
            double tolerance = under - on;
            leader.TextOnShelf.Str = tolerance.ToString();
            IBaseLeader baseLeader = leader as IBaseLeader;
            baseLeader.Update();
            
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
            try
            {
                if (kompas_ == null)
                {
                    MessageBox.Show("Не найден Компас");
                    return;
                }
                Kompas = (KompasObject)kompas_;
                Application = (IApplication)Kompas.ksGetApplication7();
                ActiveDocument = Application.ActiveDocument;
                if (ActiveDocument == null || (ActiveDocument.DocumentType != DocumentTypeEnum.ksDocumentDrawing
                 && ActiveDocument.DocumentType != DocumentTypeEnum.ksDocumentFragment && ActiveDocument.DocumentType != DocumentTypeEnum.ksDocumentPart && ActiveDocument.DocumentType != DocumentTypeEnum.ksDocumentAssembly))
                {
                    MessageBox.Show("Документ не активен либо не является чертежом/фрагментом. Возможно был перечитан другой чертеж. Переключитесь на любой другой чертеж и вернитесь назад, должно заработать.");
                    return;
                }
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
                    case 9: CreatPartFromPos_PropertyTab(); break;
                    case 10: TeklaToKompas(); break;
                    case 11: WriteToleranceDimention(); break;
                    case 12: WriteToleranceLeader(); break;

                    case 999: OpenHelp(); break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }

        }



        public object ExternalGetResourceModule()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
        //public bool LibInterfaceNotifyEntry(object kompas_)
        //{
        //    KompasEvent = (KompasObject)kompas_;
        //    ApplicationEvent = KompasEvent.ksGetApplication7();
        //    //KompasAPI7.ksKompasObjectNotify_Event ksKompasObjectNotify = ApplicationEvent as KompasAPI7.ksKompasObjectNotify_Event;
        //    //ksKompasObjectNotify.ApplicationDestroy += ApplicationDestroy;
        //    //ksKompasObjectNotify.OpenDocument += OpenDocument;
        //    return true;
        //}

        


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
