using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csharp_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SaveData saveData;
        public MainWindow()
        {
            InitializeComponent();
            saveData = new SaveData();
            saveData.FishType = new Fish();
        }

        private void FormClear_Click(object sender, RoutedEventArgs e)
        {
            saveData = new SaveData();
            saveData.FishType = new Fish();
            FormFishType.Text = "";
            FormMaxTemp.Text = "";
            FormMaxTempTime.Text = "";
            FormMinTemp.Text = "";
            FormMinTempTime.Text = "";
            FormReportIssues.Text = "";
            FormDate.Text = "";
            FormTemps.Text = "";
            FormReports.ItemsSource = null;
        }

        private void FormSaveReport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            if (dlg.ShowDialog() == true)
            {
                UpdateReport();
                try
                {
                    FileSaveLoad.SaveReport(saveData, dlg.FileName);
                }
                    catch
                {
                    MessageBox.Show("Incorrect file", "Incorrect file", MessageBoxButton.OK);
                    return;
                }
        }
        }

        private void FormLoadData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            if (dlg.ShowDialog() == true)
            {
                DateTime date;
                List<int> temps;
                try
                {
                    (date, temps) = FileSaveLoad.LoadData(dlg.FileName);
                }
                catch
                {
                    MessageBox.Show("Incorrect file", "Incorrect file", MessageBoxButton.OK);
                    return;
                }

                saveData.Date = date;
                saveData.Temps = temps;

                FormDate.Text = date.ToString();
                string formTemps = "";
                foreach(var temp in temps)
                {
                    formTemps += temp + " ";
                }
                FormTemps.Text = formTemps;
            }
        }

        private void FormMakeReport_Click(object sender, RoutedEventArgs e)
        {
            UpdateReport();
            FormReportIssues.Text = saveData.ReportIssues;
            FormReports.Columns.Clear();
            FormReports.ItemsSource = saveData.Reports;
        }

        private void UpdateReport()
        {
            int minTemp = 0, minTempTime = 0;
            bool isParsed1 = int.TryParse(FormMinTemp.Text, out minTemp);
            bool isParsed2 = int.TryParse(FormMinTempTime.Text, out minTempTime);
            //crutch
            if (!isParsed1) minTemp = int.MinValue;
            if (!isParsed2) minTempTime = int.MaxValue;

            saveData.Date = DateTime.Parse(FormDate.Text);
            try
            {
                saveData.FishType = new Fish()
                {
                    Name = FormFishType.Text,
                    MinTemp = minTemp,
                    MaxTemp = int.Parse(FormMaxTemp.Text),
                    MinTempTime = minTempTime,
                    MaxTempTime = int.Parse(FormMaxTempTime.Text)
                };
            }
            catch
            {
                MessageBox.Show("Error", "Fill max temp and his time", MessageBoxButton.OK);
                return;
            }
            saveData.ReportIssues = "";

            saveData.Temps = new List<int>();
            foreach(var temp in FormTemps.Text.Split(' '))
            {
                if (temp != "")
                {
                    saveData.Temps.Add(int.Parse(temp));
                }
            }

            saveData.Reports = new List<Report>();
            int i = 0;
            int time = 0;
            int issue = 0; // 0 - no issue, 1 - max temp, 2 - min temp
            var tempReport = new List<Report>();

            int maxTime = 0;
            int minTime = 0;

            foreach(var temp in saveData.Temps)
            {
                if (issue == 1 && temp <= saveData.FishType.MaxTemp)
                {
                    if (time >= saveData.FishType.MaxTempTime)
                    {
                        saveData.Reports.AddRange(tempReport);
                        maxTime += time;
                    }
                    issue = 0;
                    time = 0;
                }
                else if (issue == 2 && temp >= saveData.FishType.MinTemp)
                {
                    if (time >= saveData.FishType.MinTempTime)
                    {
                        saveData.Reports.AddRange(tempReport);
                        minTime += time;
                    }
                    issue = 0;
                    time = 0;
                }
                else if (temp > saveData.FishType.MaxTemp)
                {
                    if (issue == 0)
                    {
                        tempReport = new List<Report>();
                    }
                    tempReport.Add(new Report()
                    {
                        Date = saveData.Date.AddMinutes(10 * i),
                        ActualValue = temp,
                        NormalValue = saveData.FishType.MaxTemp,
                        Offset = temp - saveData.FishType.MaxTemp
                    });
                    time += 10;
                    issue = 1;
                }
                else if (temp < saveData.FishType.MinTemp)
                {
                    if (issue == 0)
                    {
                        tempReport = new List<Report>();
                    }
                    tempReport.Add(new Report()
                    {
                        Date = saveData.Date.AddMinutes(10 * i),
                        ActualValue = temp,
                        NormalValue = saveData.FishType.MinTemp,
                        Offset = temp - saveData.FishType.MinTemp
                    });
                    time += 10;
                    issue = 2;
                }

                i++;
            }

            if (time >= saveData.FishType.MaxTempTime && issue == 1)
            {
                saveData.Reports.AddRange(tempReport);
                maxTime += time;
            }
            else if (time >= saveData.FishType.MinTempTime && issue == 2)
            {
                saveData.Reports.AddRange(tempReport);
                minTime += time;
            }

            if (maxTime > 0)
            {
                saveData.ReportIssues += "Max temp issue for " +
                    maxTime + " minutes\n";
            }

            if (minTime > 0)
            {
                saveData.ReportIssues += "Min temp issue for " +
                    minTime + " minutes\n";
            }
        }
    }
}
