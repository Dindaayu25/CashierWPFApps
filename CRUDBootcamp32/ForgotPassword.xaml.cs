using CRUDBootcamp32.Context;
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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CRUDBootcamp32
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        MyContext myContext = new MyContext();
        string newpass;
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnsubforgot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txtemailforgot.Text == "")
            {
                MessageBox.Show("Fill Email !");
                Txtemailforgot.Focus();
            }
            else
            {
                var emailcheck = myContext.Users.Where(em => em.Email == Txtemailforgot.Text).FirstOrDefault();
                 
                if (emailcheck != null)
                {

                    var email = emailcheck.Email;
                    if (Txtemailforgot.Text == email)
                    {
                       
                            newpass = Guid.NewGuid().ToString();
                            var checkemail = myContext.Users.Where(m => m.Email == Txtemailforgot.Text).FirstOrDefault();
                            checkemail.Password = newpass;
                            myContext.SaveChanges();
                            MessageBox.Show("Your password has been update");
                            Outlook._Application _app = new Outlook.Application();
                            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            //sesuaikan dengan content yang di xaml
                            mail.To = Txtemailforgot.Text;
                            mail.Subject = "[Forgot Password Notification]" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                            mail.Body = "Hi " + Txtemailforgot.Text + " This Is Your New Password :" + newpass;
                            mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Check your email for your new password.", "Message", MessageBoxButton.OK);
                    }
                       
                }
                    
            }
            }
            catch (Exception)
            {
                MessageBox.Show("Your email not registered !", "Message", MessageBoxButton.OK);
            }
        }
    }
}
