using Atestat.Models;
using Atestat.Views;
using Atestat.Windows;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat.ViewModels
{
    public class OrderPageViewModel
    {

        public ObservableCollection<ItemModel> OrderItemsList { get; set; }

        public ICommand RentCar { get; set; }

        public OrderPageViewModel() 
        {

            OrderItemsList = new ObservableCollection<ItemModel>();

            RentCar = new RelayCommand(BuyItem);

            LoadDataFromSql(App.AddToCart_ItemID);

        }

        private void BuyItem()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string insertQuery = "INSERT INTO OwnUserItems (UserName, ItemID) VALUES (@UserName, @ItemID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                insertCommand.Parameters.AddWithValue("@ItemID", App.AddToCart_ItemID);
                int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("The order is complete!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("The order is incomplete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                connection.Close();
            }
        }


        public void LoadDataFromSql(int id)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string sqlQuery = @"SELECT * FROM [Items] WHERE ID = @ItemID";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@ItemID", id);
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
                    OrderItemsList.Add(item);
                }

                reader.Close();
                connection.Close();
            }
        }
        

    }
}
