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

namespace NaturalSelection
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double value = (sender as Slider).Value;

            if (e.Delta < 0)
            {
                if (++value > (sender as Slider).Maximum)
                    value = (sender as Slider).Maximum;
            }
            else
            {
                if (--value < (sender as Slider).Minimum)
                    value = (sender as Slider).Minimum;
            }

            (sender as Slider).Value = value;
        }
    }
}
