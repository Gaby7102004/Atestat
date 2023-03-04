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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using static Atestat.ViewModels.LogInWindowViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Atestat.ViewModels
{
    public class MainWindowViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private string _userName;
        public string UserName
        {
            get
            {
                return _userName; 
            }
            set 
            {
                _userName=value;
                RaisePropertyChanged("UserName");
            }

        }


        private string _userAvatar;
        public string UserAvatar
        {
            get
            {
                return _userAvatar;
            }
            set
            {
                _userAvatar = value;
                OnPropertyChanged("UserName");
            }

        }


        private object _currentPage;
        public object CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }


        public ICommand HomePageCommand { get; set; }
        public ICommand MarketPageCommand { get; set; }
        public ICommand FavoritePageCommand { get; set; }
        public ICommand OrderPageCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand ProfilePageCommand { get; set; }

        public MainWindowViewModel()
        {

            UserName = App.CurrentUser;
            LoadUserAvatar();

            LogOutCommand = new RelayCommand(LogOut);
            HomePageCommand = new RelayCommand(NavigateToHomePage);
            MarketPageCommand = new RelayCommand(NavigateToMarketPage);
            FavoritePageCommand = new RelayCommand(NavigateToFavoritePage);
            ProfilePageCommand = new RelayCommand(NavigateToProfilePage);
            OrderPageCommand = new RelayCommand(NavigateToOrderPage);
            NavigateToHomePage();

        }


        private void NavigateToHomePage() => CurrentPage = new HomePageViewModel();
        private void NavigateToMarketPage() => CurrentPage = new MarketPageViewModel();
        private void NavigateToFavoritePage() => CurrentPage = new FavoritesPageViewModel();
        private void NavigateToProfilePage() => CurrentPage = new ProfilePageViewModel();
        private void NavigateToOrderPage() => CurrentPage = new OrderPageViewModel();


        private void LoadUserAvatar()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string queryString = "SELECT Avatar FROM Users WHERE Name = @userName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@userName", App.CurrentUser);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string avatar = reader.IsDBNull(0) ? null : reader.GetString(0);

                    if (avatar == null)
                        UserAvatar = App.UserAvatar;
                    else
                        UserAvatar = avatar;
                    // use the avatarValue variable as needed
                }

                reader.Close();
            }

        }


        private void LogOut()
        {
            var logInWindow = Application.Current.Windows.OfType<LogInWindow>().FirstOrDefault();
            logInWindow.Show();

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.Close();

            App.UserAvatar = "/Images/avatar.png";
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
