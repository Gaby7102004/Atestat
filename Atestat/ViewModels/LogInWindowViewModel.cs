using Atestat.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;

namespace Atestat.ViewModels
{
    public class LogInWindowViewModel: ViewModelBase
    {

        //Variables
        private string _username;
        private string _password;
        public string Username 
        { 
            get
            { 
                return _username; 
            } 
            set 
            { 
                _username = value;
            }
        }

        public string Password { get { return _password;} set { _password = value; } }


        //Commands
        public ICommand LogInCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        //Asigning the sql servers connection
        readonly SqlConnection conn = new SqlConnection();



        public LogInWindowViewModel() 
        {
           

            LogInCommand = new RelayCommand(OnLogIn, CanLogIn);
            ResetPasswordCommand = new RelayCommand(ResetPass);
            SignUpCommand = new RelayCommand(SignUp);

            //Connect to the database
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();
        }






        private void ResetPass()
        {
            ResetPasswordWindow resetPass = new ResetPasswordWindow();
            resetPass.ShowDialog();
        }

        private void SignUp()
        {
            SignUpWindow signUp = new SignUpWindow();
            signUp.ShowDialog();
        }



        //Login button click
        private void OnLogIn()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();

            if (VerifyUser(Username, Password)) 
            {
            
            //Save the username
            App.CurrentUser = Username;
            App.UserPassword = Password;

            //Initialize the MainWindow and display it
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();

             //Hide the LogInWindow
             var logInWindow = Application.Current.Windows.OfType<LogInWindow>().FirstOrDefault();
                logInWindow.Hide();
            
            }
        }


        //Check user credentials
        private bool VerifyUser(string username, string password)
        {

            // Create a SqlConnection object and open the connection
            conn.Open();

            // Create a SqlCommand object with the query and the connection
            SqlCommand cmd = new SqlCommand("SELECT name, password FROM users WHERE name = @username COLLATE Latin1_General_CS_AS AND password = @password COLLATE Latin1_General_CS_AS", conn);

            // Add the parameters to the command to prevent SQL injection attacks
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            // Execute the query and create a SqlDataReader object
            SqlDataReader reader = cmd.ExecuteReader();

            // Check if any rows were returned by the query
            if (reader.HasRows)
            {
                // User credentials are valid
                return true;

            }

            // User credentials are not valid
            MessageBox.Show("The username or password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Information);


            // Close the SqlDataReader, SqlCommand, and SqlConnection objects
            reader.Close();
            cmd.Dispose();
            conn.Close();
            return false;


        }


        //Clickable or unclickable login button
        private bool CanLogIn()
        {
            if(String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                return false;

            else
                return true;

        }



        public new event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       


    }


    //RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
