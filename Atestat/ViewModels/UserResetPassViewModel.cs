using Atestat.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat.ViewModels
{
    public class UserResetPassViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Visibility _secondPassVisibility = Visibility.Hidden;
        public Visibility SecondPassVisibility
        {
            get { return _secondPassVisibility; }
            set { _secondPassVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _firstPassVisibility = Visibility.Visible;
        public Visibility FirstPassVisibility
        {
            get { return _firstPassVisibility; }
            set { _firstPassVisibility = value; OnPropertyChanged(); }
        }


        private string _fistPass;
        public string FirstPass
        {
            get { return _fistPass; }
            set { _fistPass = value; OnPropertyChanged(); }
        }

        private string _secondPass;
        public string SecondPass
        {
            get { return _secondPass; }
            set { _secondPass = value; OnPropertyChanged(); }
        }


        public ICommand ResetPassword { get; set; }
        public ICommand IsFirstPassword { get; set; }

        public UserResetPassViewModel()
        {
            ResetPassword = new RelayCommand(PassCorrespond);
            IsFirstPassword = new RelayCommand(IsFirstPass);
        }

        private void IsFirstPass()
        {
            if (!String.IsNullOrEmpty(FirstPass))
            {
                SecondPassVisibility = Visibility.Visible;
                FirstPassVisibility = Visibility.Hidden;
            }
            else
                MessageBox.Show("The password cant be empty!");
        }

        private void PassCorrespond()
        {
            if (FirstPass == SecondPass)
                UpdatePassword();
            else
                MessageBox.Show("The passwords do not correspond!");

        }


        //Insert new row in sql table for new user registration
        private void UpdatePassword()
        {

            #region Check if the Email,Username and Password are valid


            // Check if the password is valid
            if (String.IsNullOrEmpty(FirstPass))
            {
                MessageBox.Show("Password mustnt be empty!");
                return;
            }

            #endregion


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string insertQuery = "UPDATE Users SET Password = @NewPassword WHERE Email = @UserEmail";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                // Insert new password
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@UserEmail", App.ResetPassEmail);
                insertCommand.Parameters.AddWithValue("@NewPassword", FirstPass);
                int rowsAffected = insertCommand.ExecuteNonQuery();


                MessageBox.Show("The password has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);



                //Close the window after the registration
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is ResetPasswordWindow)
                    {
                        window.Close();
                        break;
                    }
                }

                connection.Close();

            }

        }



        public new event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
