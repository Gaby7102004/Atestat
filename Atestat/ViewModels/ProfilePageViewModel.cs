using Atestat.Windows;

using GalaSoft.MvvmLight;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat.ViewModels
{
    public class ProfilePageViewModel: ViewModelBase,INotifyPropertyChanged
    {
        private string _userPassword;
        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; OnPropertyChanged(); }

        }

        private string _userUsername;
        public string UserUsername
        {
            get { return _userUsername; }
            set { _userUsername = value; OnPropertyChanged(); }

        }

        private string _userEmail;
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; OnPropertyChanged(); }

        }


        public ProfilePageViewModel()
        {
            GetUserEmail();

            UserPassword = App.UserPassword;
            UserUsername = App.CurrentUser;
            UserEmail = App.UserEmail;
        }


        private void GetUserEmail()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string queryString = "SELECT Email FROM Users WHERE Name = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@UserName", App.CurrentUser);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    App.UserEmail = reader.GetString(0);
                    // use the avatarValue variable as needed
                }

                reader.Close();
            }

        }



        public new event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
