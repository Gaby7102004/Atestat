using GalaSoft.MvvmLight;
using Atestat.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Windows;
using Atestat.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat.ViewModels
{
    public class FavoritesPageViewModel: ViewModelBase, INotifyPropertyChanged
    {


        public ObservableCollection<ItemModel> FavoriteItemsList { get; set; }



        public FavoritesPageViewModel()
        {

            FavoriteItemsList = new ObservableCollection<ItemModel>();

            LoadDataFromSql(App.CurrentUser);


        }





        public void LoadDataFromSql(string username)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string sqlQuery = @"SELECT DISTINCT i.ID, i.Image, i.Title, i.Description, i.Price
                                FROM Items i
                                JOIN UserItems ui ON i.ID = ui.ItemID
                                WHERE ui.UserName = @Username COLLATE Latin1_General_CS_AS AND ui.IsChecked = 'DA'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ItemModel item = new ItemModel()
                    {
                        ID = (int)reader["ID"],
                        Image = (string)reader["Image"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (int)reader["Price"]
                    };
                    FavoriteItemsList.Add(item);
                }

                reader.Close();
                connection.Close();
            }
        }



        public new event PropertyChangedEventHandler PropertyChanged;

        //private new void RaisePropertyChanged(string property)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        //}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
