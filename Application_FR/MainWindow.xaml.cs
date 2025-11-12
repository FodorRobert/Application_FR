using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Application_FR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Database db = new Database();

        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = usernameTextBox.Text.Trim();
                var pass = passbox.Password;
                var again = passbox_ujra.Password;

                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                    return;

                if (pass != again)
                {
                    MessageBox.Show("A jelszavak nem egyeznek.");
                    return;
                }

                if (db.RegisterUser(user, pass))
                    MessageBox.Show("Sikeres regisztráció!");

                usernameTextBox.Clear();
                passbox.Clear();
                passbox_ujra.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void LoadUsers()
        {
            dataGrid1.ItemsSource = db.GetAllUsers().DefaultView;
        }
    }
}
