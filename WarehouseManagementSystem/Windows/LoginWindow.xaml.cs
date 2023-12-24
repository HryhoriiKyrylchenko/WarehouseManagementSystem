using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WarehouseManagementSystem.Services;

namespace WarehouseManagementSystem.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginService loginService;

        public LoginWindow(LoginService loginService)
        {
            InitializeComponent();
            this.loginService = loginService;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text;
            string password = TextBoxPassword.Text;
            if (IsValidInput(username, password))
            {
                if(loginService.Login(username, password))
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong username or password");
                    TextBoxUsername.Text = string.Empty;
                    TextBoxPassword.Text = string.Empty;
                }
            }
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public bool IsValidInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password fields are required");
                return false;
            }

            return true;
        }
    }
}
