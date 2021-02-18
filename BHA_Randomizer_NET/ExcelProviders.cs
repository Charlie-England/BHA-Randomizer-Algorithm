using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace BHA_Randomizer_NET
{
    public class ExcelProviders
    {
        public static List<ProviderModel> getExcelData()
        {
            List<ProviderModel> providers = new List<ProviderModel>();

            string dirLoc = Directory.GetCurrentDirectory();
            dirLoc += "\\providers.xlsx";

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(dirLoc);
            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            for (int i = 1; i < rowCount+1; i++)
            {
                List<string> provInfo = new List<string>();
                if (i == 1)
                {
                    continue;
                }
                for (int j = 1; j < colCount+1; j++)
                {
                    provInfo.Add(xlRange.Cells[i, j].Value2.ToString());
                }
                providers.Add(convertProviderInfo(provInfo));

            }
            xlWorkbook.Close();
            xlApp.Quit();

            return providers;
        }

        private static ProviderModel convertProviderInfo(List<string> provInfo)
        {
            //Converts the provider info taken from excel from the List<string> into a providerModel
            List<bool> daysWorked = new List<bool>();
            for (int i = 3; i < 8; i++)
            {
                if (Convert.ToInt32(provInfo[i]) == 1)
                {
                    daysWorked.Add(true);
                } else
                {
                    daysWorked.Add(false);
                }
            }

            return new ProviderModel(provInfo[0], daysWorked[0], daysWorked[1], daysWorked[2], daysWorked[3], daysWorked[4], Convert.ToDouble(provInfo[2]), provInfo[1]);
        }


    }
}
