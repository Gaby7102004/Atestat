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

namespace Atestat.Views
{
    /// <summary>
    /// Interaction logic for ProfilePageView.xaml
    /// </summary>
    public partial class ProfilePageView : Page
    {
        public ProfilePageView()
        {
            InitializeComponent();

            PasswordTextBox.IsEnabled = false;
            EmailTextBox.IsEnabled = false;
            UsernameTextBox.IsEnabled = false;
        }

        private void ChangeEmail(object sender, RoutedEventArgs e)
        {
            EmailTextBox.IsEnabled = true;
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.IsEnabled = true;
        }

        private void ChangeEmailValue(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                App.UserEmail = EmailTextBox.Text;
                ChangeUserEmail();
            }
        }

        private void ChangePasswordValue(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            { 
                App.UserPassword = PasswordTextBox.Password;
                ChangeUserPassword();
            }
        }


        private void ChangeUserPassword()
        {

            // Check if the password is valid
            if (String.IsNullOrEmpty(App.UserPassword))
            {
                MessageBox.Show("The password cannot be empty!");
                return;
            }


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string insertQuery = "UPDATE Users SET Password = @NewPassword WHERE Name = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // Insert new password
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@NewPassword", App.UserPassword);
                insertCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                int rowsAffected = insertCommand.ExecuteNonQuery();


                MessageBox.Show("The password has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                connection.Close();

            }


        }



        private void ChangeUserEmail()
        {
            // Check if the email address is valid
            if (String.IsNullOrEmpty(App.UserEmail))
            {
                MessageBox.Show("The email cannot pe empty!");
                return;
            }
            else
                if (!Regex.IsMatch(App.UserEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("The email is incorrect!");
                return;
            }



            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string insertQuery = "UPDATE Users SET Email = @NewEmail WHERE Name = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // Insert new password
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@NewEmail", App.UserEmail);
                insertCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                int rowsAffected = insertCommand.ExecuteNonQuery();


                MessageBox.Show("The email has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                connection.Close();

            }

        }

    }
}
