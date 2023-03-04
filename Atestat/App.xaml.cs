using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Atestat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentUser { get; set; }
        public static string UserEmail { get; set; }
        public static string UserPassword { get; set; }
        public static string ResetPassEmail { get; set; }
        public static int AddToCart_ItemID { get; set; }
        public static int AddToFavorites_ItemID { get; set; }
        public static string UserAvatar { get; set; }
        public static string IsFavoriteButtonClicked { get; set; }
        public static int ItemsInMarket { get; set; }

        public App()
        {
            AddToFavorites_ItemID = 0;
            UserAvatar = "/Images/avatar.png";
            GetNrOfItems();
        }

        private void GetNrOfItems()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // Define the SQL statement to count the number of rows in the Items table
            string countSql = "SELECT COUNT(*) FROM Items";

            // Create a SqlConnection object using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the database connection
                connection.Open();

                // Create a SqlCommand object to execute the count SQL statement
                using (SqlCommand command = new SqlCommand(countSql, connection))
                {
                    // Execute the count SQL statement and store the result in a variable
                    ItemsInMarket = (int)command.ExecuteScalar();
                }

                // Close the database connection
                connection.Close();
            }

        }
    }
}
