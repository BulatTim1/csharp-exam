using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_test
{
    public class SaveData
    {
        public Fish FishType { get; set; }
        public DateTime Date { get; set; }
        public List<int>? Temps { get; set; }
        public string? ReportIssues { get; set; }
        public List<Report> Reports
        {
            get; 
            set;
        }
    }

    public class Report
    {
        public DateTime Date { get; set; }
        public int ActualValue { get; set; }
        public int NormalValue { get; set; }
        public int Offset { get; set; }
    }
    
    public static class FileSaveLoad
    {
        static private string newLine = System.Environment.NewLine;
        //List<(DateTime Date, int ActualValue,int NormalValue, int Offset)> report
        public static void SaveReport(SaveData data, string path = @"./report.csv")
        {
            string fullReport = "";

            fullReport += "Fish type:;" + data.FishType.Name + newLine;
            fullReport += "Max temp:;" + data.FishType.MaxTemp + newLine;
            fullReport += "Max temp time:;" + data.FishType.MaxTempTime + newLine;
            fullReport += "Min temp:;" + data.FishType.MinTemp + newLine;
            fullReport += "Min temp time:;" + data.FishType.MinTempTime + newLine;

            fullReport += "Issues:;" + data.ReportIssues + newLine;

            fullReport += "Date;Actual Value;Normal Value;Offset" + newLine;
            foreach (var item in data.Reports)
            {
                fullReport += $"{item.Date};{item.ActualValue};{item.NormalValue};{item.Offset}{newLine}";
            }

            System.IO.File.WriteAllText(path, fullReport);
        }

        public static (DateTime date, List<int> temps) LoadData(string path)
        {
            string data = System.IO.File.ReadAllText(path);
            if (data == "" || data == null)
            {
                throw new NullReferenceException("File is empty");
            }

            DateTime date;
            List<int> temps = new List<int>();
            string[] datas = data.Split(newLine);

            DateTime.TryParse(datas[0], out date);

            foreach (var temp in datas[1].Split())
            {
                if (temp == "") break;
                temps.Add(int.Parse(temp));
            }

            return (date, temps);
        }
    }
}
