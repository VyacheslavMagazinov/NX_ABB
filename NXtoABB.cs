using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using NXOpen;
using NXOpenUI;
using NXOpen.UF;
using System.Numerics;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;

namespace NxToQuaternion
{
    public class NXtoABB
    {
        private static Session theSession;
        private static UFSession theUFSession;
        private static ListingWindow lw;
        public static String t1;
        public static String t2;
        public static String t3;
        public static String t4;
        public static String t5;
        public static String t6;
        static String outputFile;
        public static String FullPath;
        public static List<String> TabExport = new List<String>();

        public class robtarget
        {
            public string name;
            public translation XYZ;
            public rotation WXYZ;
            public robconf RC;
            public extax EA;
            public string VSpeed;
            public string VZone;

        }

        public class MoveJ
        {
            public translation XYZ;
            public rotation WXYZ;
            public robconf RC;
            public extax EA;
            public string VSpeed;
            public string VZone;

        }

        public class MoveL
        {
            public translation XYZ;
            public rotation WXYZ;
            public robconf RC;
            public extax EA;
            public string VSpeed;
            public string VZone;

        }

        public class translation
        {
            public String X;
            public String Y;
            public String Z;

        }

        public class rotation
        {
            public String W;
            public String X;
            public String Y;
            public String Z;

        }

        public class extax
        {
            public String e1 = "9E+09"; 
            public String e2 = "9E+09";
            public String e3 = "9E+09";
            public String e4 = "9E+09";
            public String e5 = "9E+09";
            public String e6 = "9E+09";

        }

        public class robconf
        {
            public String cf1;
            public String cf4;
            public String cf6;
            public String cfx;

        }

        public class PartNX
        { 
            public String PartName = "";
            public String Material = "";
            public String Description = "";

            public Int32 Count = 0;
        }

        public class AssemblyNX
        {
            public List<PartNX> PartsList = new List<PartNX>();
            public List<AssemblyNX> AssemblyList = new List<AssemblyNX>();

            public String AssemblyName = "";
            public String Description = "";

            public Int32 Count = 0;
        }

        public static List<AssemblyNX> AssemblyListMain = new List<AssemblyNX>();

        public static void Main()
        {
            theSession = Session.GetSession();
            theUFSession = UFSession.GetUFSession();
            lw = theSession.ListingWindow;

            Part dispPart = theSession.Parts.Display;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XLSX files(*.xlsx)|*.xlsx";
            saveFileDialog.ShowDialog();
            outputFile = saveFileDialog.FileName;

            ExcelExport XLSX_Expor = new ExcelExport();

            lw.Open();

            AssemblyList(dispPart);

            XLSX_Expor.ExcelExportT(TabExport, outputFile);

            lw.Close();
        }

        public static void AssemblyList(Part inputPart) 
        {
            //Взять имя целевой сборки. Вывести его в информационное окно
            String inputPartName = inputPart.Name;
            lw.WriteLine(inputPartName + "_inputPartName");

            string name = "";
            //Создать массив компонентов из всех потомков первого уровня у целевой сборки
            NXOpen.Assemblies.Component[] allComps = inputPart.ComponentAssembly.RootComponent.GetChildren();

            //???????
            AssemblyListMain.Add(new AssemblyNX());

            //Заголовок таблицы
            TabExport.Add("Имя;Координаты;Вращение");

            AllCompsFun(allComps, ref AssemblyListMain, inputPart);

        }

        public static void AllCompsFun(NXOpen.Assemblies.Component[] AllComps, ref List<AssemblyNX> AssemblyListMainIn, Part inputPart)
        {

            Point3d p3d;
            Matrix3x3 m3d;
            Matrix3x3 m3d1;
            Quaternion Qt;

            for (int i = 0; i < AllComps.Length; i++)
            {
                if (AllComps[i].GetChildren().Length > 0)
                {

                    try
                    {
                        if (AllComps[i].GetStringAttribute("EXPORT") == "TRUE")
                        {

                            //t1 = AllComps[i].DisplayName;
                            //CoordinateSystem wcs1;
                            //t1 = inputPart.WCS.CoordinateSystem.Orientation.Element.ToString();
                            t1 = inputPart.WCS.CoordinateSystem.Orientation.Element.Xx.ToString("f3");
                            //t1 = inputPart.WCS.CoordinateSystem.Origin. .Orientation.Element.Xx.ToString("f3");
                            //t1 = string.Format("{0:f4}", inputPart.WCS.CoordinateSystem.Orientation.Element.Xx ) ;

                            AllComps[i].GetPosition(out p3d, out m3d);

                            /* m3d.Xx = inputPart.WCS.CoordinateSystem.Orientation.Element.Xx;
                             m3d.Xy = inputPart.WCS.CoordinateSystem.Orientation.Element.Xy;
                             m3d.Xz = inputPart.WCS.CoordinateSystem.Orientation.Element.Xz;

                             m3d.Yx = inputPart.WCS.CoordinateSystem.Orientation.Element.Yx;
                             m3d.Yy = inputPart.WCS.CoordinateSystem.Orientation.Element.Yy;
                             m3d.Yz = inputPart.WCS.CoordinateSystem.Orientation.Element.Yz;

                             m3d.Zx = inputPart.WCS.CoordinateSystem.Orientation.Element.Zx;
                             m3d.Zy = inputPart.WCS.CoordinateSystem.Orientation.Element.Zy;
                             m3d.Zz = inputPart.WCS.CoordinateSystem.Orientation.Element.Zz; */


                            m3d.Xx = 0;
                            m3d.Xy = 0;
                            m3d.Xz = 1;

                            m3d.Yx = -1;
                            m3d.Yy = 0;
                            m3d.Yz = 0;

                            m3d.Zx = 0;
                            m3d.Zy = -1;
                            m3d.Zz = 0; 

                            //t2 = p3d.X.ToString() + "/" + p3d.Y.ToString() + "/" + p3d.Z.ToString() + " <> " ;
                            t2 = "X=" + p3d.X.ToString("f3") + " Y=" + p3d.Y.ToString("f3") + " Z=" + p3d.Z.ToString("f3");
                            t3 = "[Xx=" + m3d.Xx.ToString("f3") + " Xy=" + m3d.Xy.ToString("f3") + " Xz=" + m3d.Xz.ToString("f3") + "]" + " [Yx=" + m3d.Yx.ToString("f3") + " Yy=" + m3d.Yy.ToString("f3") + "  Yz=" + m3d.Yz.ToString("f3") + "]" + " [Zx=" + m3d.Zx.ToString("f3") + "  Zy=" + m3d.Zy.ToString("f3") + " Zz=" + m3d.Zz.ToString("f3") + "]";

                            Qt = MathMod.MatrixToQuaternion(m3d);
                            t3 = "W=" + Qt.W.ToString("f4") + " X=" + Qt.X.ToString("f4") + " Y=" + Qt.Y.ToString("f4") + " Z=" + Qt.Z.ToString("f4");
                            //t3 = Qt.ToString();

                            lw.WriteLine(t1 + " _ " + t2);
                            TabExport.Add(t1 + ";" + t2 + ";" + t3);
                        }

                    }
                    catch 
                    {
                        lw.WriteLine(" _ ");
                    }




                    if (AssemblyListMainIn[0].Count < 1 ) 
                    {
                        AssemblyListMain[0].AssemblyList.Add(new AssemblyNX());
                    }
                    else
                    {
                        AssemblyListMainIn[0].AssemblyList[AssemblyListMainIn[0].Count - 1].AssemblyList.Add(new AssemblyNX());
                    }

                    AllCompsFun(AllComps[i].GetChildren(), ref AssemblyListMainIn, inputPart);


                }
                else
                {

                    try
                    {
                        if (AllComps[i].GetStringAttribute("EXPORT") == "TRUE")
                        {
                            //t1 = AllComps[i].DisplayName;
                            t1 = inputPart.WCS.CoordinateSystem.Orientation.Element.Xx.ToString("f3");
                            //t1 = string.Format("{0:f4}", inputPart.WCS.CoordinateSystem.Orientation.Element.Xx);
                            AllComps[i].GetPosition(out p3d, out m3d);

                            /* m3d.Xx = inputPart.WCS.CoordinateSystem.Orientation.Element.Xx;
                             m3d.Xy = inputPart.WCS.CoordinateSystem.Orientation.Element.Xy;
                             m3d.Xz = inputPart.WCS.CoordinateSystem.Orientation.Element.Xz;

                             m3d.Yx = inputPart.WCS.CoordinateSystem.Orientation.Element.Yx;
                             m3d.Yy = inputPart.WCS.CoordinateSystem.Orientation.Element.Yy;
                             m3d.Yz = inputPart.WCS.CoordinateSystem.Orientation.Element.Yz;

                             m3d.Zx = inputPart.WCS.CoordinateSystem.Orientation.Element.Zx;
                             m3d.Zy = inputPart.WCS.CoordinateSystem.Orientation.Element.Zy;
                             m3d.Zz = inputPart.WCS.CoordinateSystem.Orientation.Element.Zz; */

                            m3d.Xx = 0;
                            m3d.Xy = 0;
                            m3d.Xz = 1;

                            m3d.Yx = -1;
                            m3d.Yy = 0;
                            m3d.Yz = 0;

                            m3d.Zx = 0;
                            m3d.Zy = -1;
                            m3d.Zz = 0;

                            //t2 = m3d.Xx.ToString() + "/" + m3d.Xy.ToString() + "/" + m3d.Xz.ToString() + " <> " + m3d.Yx.ToString() + "/" + m3d.Yy.ToString() + "/" + m3d.Yz.ToString() + " <> " + m3d.Zx.ToString() + "/" + m3d.Zy.ToString() + "/" + m3d.Zz.ToString();
                            t2 = "X=" + inputPart.WCS.Origin.X.ToString("f3") + " Y=" + inputPart.WCS.Origin.Y.ToString("f3") + " Z=" + inputPart.WCS.Origin.Z.ToString("f3");
                            t3 = "[Xx=" + m3d.Xx.ToString("f3") + " Xy=" + m3d.Xy.ToString("f3") + " Xz=" + m3d.Xz.ToString("f3") + "]" + " [Yx=" + m3d.Yx.ToString("f3") + " Yy=" + m3d.Yy.ToString("f3") + "  Yz=" + m3d.Yz.ToString("f3") + "]" + " [Zx=" + m3d.Zx.ToString("f3") + "  Zy=" + m3d.Zy.ToString("f3") + " Zz=" + m3d.Zz.ToString("f3") + "]";



                            Qt = MathMod.MatrixToQuaternion(m3d);
                            t3 = "W=" + Qt.W.ToString("f4") + " X=" + Qt.X.ToString("f4") + " Y=" + Qt.Y.ToString("f4") + " Z=" + Qt.Z.ToString("f4");
                            //t3 = Qt.ToString();

                            lw.WriteLine(t1 + " _ " + t2);
                            TabExport.Add(t1 + ";" + t2 + ";" + t3);
                        }
                    }
                    catch
                    {
                        lw.WriteLine(" _ ");
                    }

                }

            }

        }

        //конец программы \/
        public static int GetUnloadOption(string arg)
        {
            return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
        }

    }

}
