using GalaSoft.MvvmLight;
using Atestat.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System;

namespace Atestat.ViewModels
{
    public class RessetPasswordWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private object _currentResetPassPage;
        public object CurrentResetPassPage
        {
            get
            {
                return _currentResetPassPage;
            }
            set
            {
                    _currentResetPassPage = value;
                    RaisePropertyChanged("CurrentResetPassPage");
            }
        }


        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private Visibility _stackVisibility = Visibility.Visible;
        public Visibility StackVisibility
        {
            get
            {
                return _stackVisibility;
            }
            set
            {
                _stackVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _frameVisibility = Visibility.Hidden;
        public Visibility FrameVisibility
        {
            get
            {
                return _frameVisibility;
            }
            set
            {
                _frameVisibility = value;
                OnPropertyChanged();
            }
        }


        public ICommand CheckEmail { get; set; }
        public ICommand BackButton { get; set; }


        public RessetPasswordWindowViewModel()
        {

            BackButton = new RelayCommand(BackwordsButton);
            CheckEmail = new RelayCommand(CheckUserEmail);
            NavigateToUserResetPassView();

        }

        //Hide the StackPannel and show the frame
        private void PanelsVisibility()
        {

            StackVisibility = Visibility.Hidden;
            FrameVisibility = Visibility.Visible;
            App.ResetPassEmail = Email;

        }

        //The opposite for the above
        private void BackwordsButton()
        {
            StackVisibility = Visibility.Visible;
            FrameVisibility = Visibility.Hidden;
        }


        private void NavigateToUserResetPassView() => CurrentResetPassPage = new UserResetPassViewModel();


        //Checks if user email exists
        private void CheckUserEmail()
        {

            //Check if the email is null
            if (String.IsNullOrEmpty(Email))
                {
                    MessageBox.Show("The email cannot be empty!");
                    return;
                }

            // Check if the email address is valid
                if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("The email is incorrect!");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email COLLATE Latin1_General_CS_AS";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if email already exists
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Email", Email);
                int existingUserCount = (int)checkCommand.ExecuteScalar();

                if (existingUserCount == 0)
                    //Email doesnt exist
                    MessageBox.Show("The email adress does not exist!", "ok", MessageBoxButton.OK, MessageBoxImage.Information);

                else
                    PanelsVisibility();
            }
        }





        public new event PropertyChangedEventHandler PropertyChanged;

        private new void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
