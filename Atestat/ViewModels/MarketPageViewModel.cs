using Atestat.Models;
using Atestat.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Atestat.ViewModels
{
    public class MarketPageViewModel: ViewModelBase,INotifyPropertyChanged
    {

        public ObservableCollection<ItemModel> ItemsList { get; set; }


        public MarketPageViewModel() 
        {
            ItemsList = new ObservableCollection<ItemModel>();

            LoadDataFromSql();



        }




        public void LoadDataFromSql()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string sqlQuery = "SELECT * from [Items]";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlQuery, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ItemModel item = new ItemModel();

                    item.ID = (int)reader["ID"];
                    item.Image = (string)reader["Image"];
                    item.Title = (string)reader["Title"];
                    item.Description = (string)reader["Description"];
                    item.Price = (int)reader["Price"];
                    ItemsList.Add(item);
                }

                reader.Close();
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
