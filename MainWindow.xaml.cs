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
        List<(DateTime Date, int ActualValue, int NormalValue, int Offset)> report;
        public MainWindow()
        {
            InitializeComponent();
            report = new List<(DateTime Date, int ActualValue, 
                int NormalValue, int Offset)>();
        }

        private void FormClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FormSaveReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FormLoadData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FormMakeReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
