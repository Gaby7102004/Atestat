using Atestat.ViewModels;
using Atestat.Windows;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            if(mainWindowViewModel.UserAvatar == "/Images/avatar.png")
            {
                imgSelectedImage.Height = 32;
                imgSelectedImage.Width = 32;
            }
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                imgSelectedImage.Source = bitmap;
                imgSelectedImage.Height = 100;
                imgSelectedImage.Width = 100;
                string fullPath = bitmap.ToString();
                App.UserAvatar = fullPath;
                RegisterUser();
            }
        }


        private void RegisterUser()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string insertQuery = "UPDATE Users SET Avatar = @Avatar WHERE Name = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                    // Insert avatar for the user
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Username", App.CurrentUser);
                    insertCommand.Parameters.AddWithValue("@Avatar",App.UserAvatar);
                    int rowsAffected = insertCommand.ExecuteNonQuery();


                connection.Close();
            }


        }

    }
}
