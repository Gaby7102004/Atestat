using GalaSoft.MvvmLight;
using Atestat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Atestat.ViewModels
{
    public class HomePageViewModel: ViewModelBase, INotifyPropertyChanged
    {

        public ObservableCollection<ItemModel> YourItemsList { get; set; }


        public HomePageViewModel()
        {

            YourItemsList = new ObservableCollection<ItemModel>();

            LoadUserItems(App.CurrentUser);

        }


        private void LoadUserItems(string username)
        {

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string sqlQuery = @"SELECT i.ID, i.Image, i.Title, i.Description, i.Price, i.IsChecked
                                FROM Items i
                                JOIN OwnUserItems ui ON i.ID = ui.ItemID
                                WHERE ui.UserName = @Username COLLATE Latin1_General_CS_AS";


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
                    YourItemsList.Add(item);
                    }

                    reader.Close();
                    connection.Close();
                }
            
        }






        //public new event PropertyChangedEventHandler PropertyChanged;

        //private new void RaisePropertyChanged(string property)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        //}

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

    }
}
