using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Atestat.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Atestat.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Atestat.Windows
{
    /// <summary>
    /// Interaction logic for ResetPasswordWindow.xaml
    /// </summary>
    public partial class ResetPasswordWindow : System.Windows.Window
    {
        public ResetPasswordWindow()
        {
            InitializeComponent();

            DataContext = new RessetPasswordWindowViewModel();

        }


        //Button to close the window
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
