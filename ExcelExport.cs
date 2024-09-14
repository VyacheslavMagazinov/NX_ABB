using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace NxToQuaternion
{
    public class ExcelExport
    {

        public void ExcelExportT(List<string> PartList, string path) 
        {
            int Colls; //количество столбцов
            int Rows; //количество строк

            using (var p = new ExcelPackage()) 
            {
                var ws = p.Workbook.Worksheets.Add("List");

                Rows = PartList.Count; //подсчёт строк
                Colls = PartList[0].Split(';').Length; //полсчёт столбцов

                for (int a = 0; a < PartList.Count; a++)
                {
                    String[] TmpStr = PartList[a].Split(';');
                    for (int i = 0; i < TmpStr.Length; i++) 
                    {
                        ws.Cells[(a + 1) , (i + 1)].Value = TmpStr[i];
                    }

                }

                //диапазон ячеек таблицы
                var range = ws.Cells[1,1,Rows,Colls];

                //создание таблицы
                var table = ws.Tables.Add(range, "myTable");

                table.ShowHeader = true;
                table.TableStyle = TableStyles.Medium1;

                ws.Cells[1,1,Rows,Colls].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                p.SaveAs(new FileInfo(@path));

            }

        }


    }
}
