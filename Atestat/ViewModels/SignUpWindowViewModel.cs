using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Atestat.Windows;

namespace Atestat.ViewModels
{
    public class SignUpWindowViewModel
    {
        private string _email;
        private string _password;
        private string _username;

        public string Password { get { return _password; } set { _password = value; } }
        public string Username { get { return _username; } set { _username = value; } }
        public string Email { get { return _email; } set { _email = value; } }


        public ICommand UserRegistration { get; set; }

        public SignUpWindowViewModel()
        {
            UserRegistration = new RelayCommand(RegisterUser, CheckButton);
        }

        //Insert new row in sql table for new user registration
        private void RegisterUser()
        { 

            #region Check if the Email,Username and Password are valid

            // Check if the username is valid
            if (String.IsNullOrEmpty(Username))
            {
                MessageBox.Show("The username cannot be empty!");
                return;
            }

            else
                if (!Regex.IsMatch(Username, @"[a-zA-Z]"))
                { 
                    MessageBox.Show("Username must contain at least one alphabet character.");
                    return;
                }

            // Check if the email address is valid
            if (String.IsNullOrEmpty(Email))
            {
                MessageBox.Show("The email cannot pe empty!");
                return;
            }
            else
                if(!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("The email is incorrect!");
                return;
            }

            // Check if the password is valid
            if (String.IsNullOrEmpty(Password))
            {
                MessageBox.Show("The password cannot be empty!");
                return;
            }

            #endregion


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Name = @Username";
            string insertQuery = "INSERT INTO Users (Name, Password, Email) VALUES (@Username, @Password, @Email)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if username already exists
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Username", Username);
                int existingUserCount = (int)checkCommand.ExecuteScalar();

                if (existingUserCount == 0)
                {
                    // Username does not exist
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Username", Username);
                    insertCommand.Parameters.AddWithValue("@Password", Password);
                    insertCommand.Parameters.AddWithValue("@Email", Email);
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        //Close the window after the registration
                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is SignUpWindow)
                            {
                                window.Close();
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("User registration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Username already exists
                    MessageBox.Show("The username already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                connection.Close();
            }


        }

        //clickable/unclickable button
        private bool CheckButton()
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(Email))
                return false;

            else
                return true;
        }

    }
}
