using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Atestat.Models;
using Atestat.ViewModels;
using GalaSoft.MvvmLight.Messaging;

namespace Atestat.Views
{
    /// <summary>
    /// Interaction logic for FavoritesPageView.xaml
    /// </summary>
    public partial class FavoritesPageView : Page
    {


        public FavoritesPageView()
        {

            InitializeComponent();

        }



        //idk
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            // Traverse the visual tree to find the first ancestor of the specified type
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        //Saves the id value of the lisbox intem selected
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the Button that was clicked
            Button button = (Button)sender;

            // Traverse the visual tree to get the parent ListBoxItem
            ListBoxItem listBoxItem = FindAncestor<ListBoxItem>(button);

            // Access the ID property of the ItemModel object
            if (listBoxItem != null)
            {
                // Get the ItemModel object that the ListBoxItem is bound to

                // Access the ID property of the ItemModel object
                if (listBoxItem.DataContext is ItemModel itemModel)
                {
                    App.AddToCart_ItemID = itemModel.ID;
                }
            }
        }

        //Removes the item from the favorites page
        private void RemoveFavItem(object sender, RoutedEventArgs e)
        {
            int favItemID = 0;

            // Get the Button that was clicked
            Button button = (Button)sender;
            var item = button.DataContext; // get the data context of the button (i.e. the item in the list)


            // Traverse the visual tree to get the parent ListBoxItem
            ListBoxItem listBoxItem = FindAncestor<ListBoxItem>(button);

            // Access the ID property of the ItemModel object
            if (listBoxItem != null)
            {
                // Get the ItemModel object that the ListBoxItem is bound to

                // Access the ID property of the ItemModel object
                if (listBoxItem.DataContext is ItemModel itemModel)
                {
                    favItemID = itemModel.ID;
                }

            }



                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string deleteQuery = "UPDATE UserItems SET IsChecked = @IsChecked WHERE UserName = @UserName and ItemID = @ItemID";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int rowsAffected;

                    // Insert into table
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                   
                    deleteCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                    deleteCommand.Parameters.AddWithValue("@ItemID", favItemID);
                    deleteCommand.Parameters.AddWithValue("@IsChecked", "NU");


                if (favItemID > 0)
                    {
                        if (listBox.ItemsSource is ObservableCollection<ItemModel> FavoriteItemsList)
                        {
                            FavoriteItemsList.Remove((ItemModel)item);
                            rowsAffected = deleteCommand.ExecuteNonQuery();
                        }

                    }

                connection.Close();
                }
        }


    }
}
