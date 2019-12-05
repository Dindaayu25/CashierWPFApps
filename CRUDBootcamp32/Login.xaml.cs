using CRUDBootcamp32.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CRUDBootcamp32
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        MyContext myContext = new MyContext();
        public Login()
        {
            InitializeComponent();
        }

       
        private void btnlogin_Click_1(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    var email = myContext.Users.Where(u => u.Email == Txtuseremail.Text).FirstOrDefault();

                    if ((Txtuseremail.Text == "") || (Txtpass.Password == ""))
                    {
                        if (Txtuseremail.Text == "")
                        {
                            MessageBox.Show("Email is Required!", "Caution", MessageBoxButton.OK);
                            Txtuseremail.Focus();
                        }
                        else if (Txtpass.Password == "")
                        {
                            MessageBox.Show("Password is Required!", "Caution", MessageBoxButton.OK);
                            Txtpass.Focus();
                        }
                    }
                    else
                    {
                        if (email != null)
                        {
                            var psw = email.Password;
                            psw = Txtpass.Password;
                            if (Txtpass.Password == psw)
                            {
                                MessageBox.Show("Login Successfully!", "Login Succes", MessageBoxButton.OK);
                                MainWindow dashboard = new MainWindow();
                                dashboard.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Email and Password are wrong!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Email and Password is invalid");
                        }

                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword dashboard2 = new ForgotPassword();
            dashboard2.Show();
        }

        private void Txtuseremail_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
