using Atestat.Models;
using Atestat.Views;

using GalaSoft.MvvmLight.Messaging;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
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

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Atestat.Views
{
    /// <summary>
    /// Interaction logic for MarketPageView.xaml
    /// </summary>
    public partial class MarketPageView : Page
    {
        public MarketPageView()
        {
            InitializeComponent();
        
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Find the ListBox control in the visual tree
            ListBox myListBox = FindName("myListBox") as ListBox;

            // Find the ListBoxItem with ID = 5
            foreach (var item in myListBox.Items)
            {
                if (item is ItemModel itemModel)
                {
                    // Find the button control inside the ListBoxItem
                    Button myButton = FindVisualChild<Button>(myListBox.ItemContainerGenerator.ContainerFromItem(itemModel));
                    if (myButton != null)
                    {
                        // Update the image source of the button
                        Image myImage = myButton.Content as Image;
                        if (myImage != null)
                        {
                            string isChecked = "";
                            int itemId = itemModel.ID;
                            GetIsChecked(ref isChecked, itemId);


                            if (isChecked =="DA")
                            {
                                myImage.Source = new BitmapImage(new Uri("/Images/heart_clicked.png", UriKind.Relative));
                            }
                            else
                            {
                                myImage.Source = new BitmapImage(new Uri("/Images/heart.png", UriKind.Relative));
                            }
                        }
                    }
                }
            }
        }


        //Save the id value of the listbox item selected (add to cart)
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


        //Add item to favorites based on user
        private void AddToFavorite_Click(object sender, RoutedEventArgs e)
        {
            // Cast the sender object to a Button control

            // Check if the button control is not null
            if (sender is Button button)
            {

                Image image = button.Content as Image;

                // Traverse the visual tree to get the parent ListBoxItem
                ListBoxItem listBoxItem = FindAncestor<ListBoxItem>(button);

                // Check if the ListBoxItem is not null
                if (listBoxItem != null)
                {
                    // Get the ItemModel object that the ListBoxItem is bound to
                    if (listBoxItem.DataContext is ItemModel itemModel)
                    {
                        // Access the ID property of the ItemModel object
                        App.AddToFavorites_ItemID = itemModel.ID;
                    }

                }
                    if(image.Source.ToString().Contains("heart_clicked"))
                    {
                        image.Source = new BitmapImage(new Uri("/Images/heart.png", UriKind.Relative));
                        UndoAddToFavorites();
                        
                    }
                else
                { 
                        image.Source = new BitmapImage(new Uri("/Images/heart_clicked.png", UriKind.Relative));
                        AddToFavorites();
                }

            }


        }
     

        public void UndoAddToFavorites()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string deleteQuery = "UPDATE UserItems SET IsChecked = @IsChecked WHERE UserName = @UserName and ItemID = @ItemID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                deleteCommand.Parameters.AddWithValue("@ItemID", App.AddToFavorites_ItemID);
                deleteCommand.Parameters.AddWithValue("@IsChecked", "NU");

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void AddToFavorites()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string checkQuery = "SELECT COUNT(*) FROM UserItems WHERE UserName = @UserName and ItemID = @ItemID";
            string insertQuery = "INSERT INTO UserItems (UserName, ItemID, IsChecked) VALUES (@UserName, @ItemID, @IsChecked)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if item already exists
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                checkCommand.Parameters.AddWithValue("@ItemID", App.AddToFavorites_ItemID);
                int existingItemCount = (int)checkCommand.ExecuteScalar();

                if (existingItemCount == 0)
                {
                    // Item does not exist
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                    insertCommand.Parameters.AddWithValue("@ItemID", App.AddToFavorites_ItemID);
                    insertCommand.Parameters.AddWithValue("@IsChecked", "DA");
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                }
                else
                {
                    string updateQuery = "UPDATE UserItems SET IsChecked = @IsChecked WHERE UserName = @UserName and ItemID = @ItemID";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@UserName", App.CurrentUser);
                    updateCommand.Parameters.AddWithValue("@ItemID", App.AddToFavorites_ItemID);
                    updateCommand.Parameters.AddWithValue("@IsChecked", "DA");

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                }

                connection.Close();
            }

        }


        private void GetIsChecked(ref string isChecked, int itemId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string isCheckedQuery = "SELECT IsChecked FROM UserItems WHERE ItemID = @ItemId and UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(isCheckedQuery, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    command.Parameters.AddWithValue("@UserName", App.CurrentUser);

                    isChecked = (string)command.ExecuteScalar();
                }

                connection.Close();
            }
        }



        //idk
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                {
                    return t;
                }
                else
                {
                    T result = FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
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
        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T child = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject childElement = VisualTreeHelper.GetChild(parent, i);

                if (childElement is T childOfType && childOfType.GetType() == typeof(T) && childOfType.GetValue(FrameworkElement.NameProperty).ToString() == childName)
                {
                    child = childOfType;
                    break;
                }

                child = FindChild<T>(childElement, childName);

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }

    }
}
