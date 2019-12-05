using CRUDBootcamp32.Context;
using CRUDBootcamp32.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Outlook = Microsoft.Office.Interop.Outlook;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;

namespace CRUDBootcamp32
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext myContext = new MyContext();
        int supplierid, transid, roleid;
        int itemId;
        int lasttot, laststock, totalpay, vpay;
        string pass;
        List<TransactionItem> Transcart = new List<TransactionItem>();
        string struk = "ID\t" + "Name\t" + "Price\t" + "Quantity\t" +  "\n";
        public MainWindow()
        {
            InitializeComponent();
            Showdata();
            GridSupplier.ItemsSource = myContext.Suppliers.ToList();
            GridItem.ItemsSource = myContext.Items.ToList();
            GridRole.ItemsSource = myContext.Roles.ToList();


            supp_id.ItemsSource = myContext.Suppliers.ToList();
            CbRole.ItemsSource = myContext.Roles.ToList();
            cbNameitem.ItemsSource = myContext.Items.ToList();


        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {

            // foreach (var data in myContext.Suppliers)
            //{
            //  if(data.Email != TxtEmail.Text)
            //{
            //   validEmail = true;
            // }
            //}
            //myContext.Projects.Where(p => p.ProjectID != ProjectId && p.Name == Name);

            if (TxtName.Text == "")
            {
                MessageBox.Show("Fill Name !");
                TxtName.Focus();
            }
            else if (TxtEmail.Text == "")
            {
                MessageBox.Show("Fill Email !");
                TxtEmail.Focus();
            }

            else
            {
                var checkemail = myContext.Suppliers.FirstOrDefault(email => email.Email == TxtEmail.Text);
                if (checkemail == null)
                {
                    var push = new Supplier(TxtName.Text, TxtEmail.Text);
                    myContext.Suppliers.Add(push);
                    var result = myContext.SaveChanges();

                    Showdata();
                    TxtName.Text = "";
                    TxtEmail.Text = "";
                    if (result > 0)
                    {
                        MessageBox.Show(result + " row has been inserted");
                        GridSupplier.ItemsSource = myContext.Suppliers.ToList();
                        try
                        {
                            //Outlook._Application _app = new Outlook.Application();
                            //Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            ////sesuaikan dengan content yang di xaml
                            //mail.To = TxtEmail.Text;
                            //mail.Subject = "from dinda";
                            //mail.Body = "Email has delivered !";
                            //mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            //((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Message has been sent.", "Message", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                            TxtName.Text = "";
                            TxtEmail.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Email already been used !", "Caution !", MessageBoxButton.OK);
                }
            }
        }

        public void Showdata()
        {
            GridSupplier.ItemsSource = myContext.Suppliers.ToList();
            GridItem.ItemsSource = myContext.Items.ToList();
            GridRole.ItemsSource = myContext.Roles.ToList();
            GridRegis.ItemsSource = myContext.Users.ToList();

        }

        private void GridSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = GridSupplier.SelectedItem;
            string id = (GridSupplier.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtId.Text = id;
            string name = (GridSupplier.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            TxtName.Text = name;
            string email = (GridSupplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            TxtEmail.Text = email;

        }

        private void TxtEmail_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TxtId.Text);
            var uRow = myContext.Suppliers.Where(s => s.Id == id).FirstOrDefault();
            uRow.Name = TxtName.Text;
            uRow.Email = TxtEmail.Text;
            myContext.SaveChanges();
            GridSupplier.ItemsSource = myContext.Suppliers.ToList();
            MessageBox.Show("Data Updated !");
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(TxtId.Text);
                var dRow = myContext.Suppliers.Where(s => s.Id == id).FirstOrDefault();
                myContext.Suppliers.Remove(dRow);
                myContext.SaveChanges();
                GridSupplier.ItemsSource = myContext.Suppliers.ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Data Deleted !", "Caution !", MessageBoxButton.OK);
            }
        }

        private void supp_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supplierid = Convert.ToInt32(supp_id.SelectedValue.ToString());
        }

        private void btnsubmit_Click_1(object sender, RoutedEventArgs e)
        {
            if (Txtname.Text == "")
            {
                MessageBox.Show("Fill Name !");
                Txtname.Focus();
            }
            else if (Txtstock.Text == "")
            {
                MessageBox.Show("Fill Stock !");
                Txtstock.Focus();
            }
            else if (Txtprice.Text == "")
            {
                MessageBox.Show("Fill Price !");
                Txtprice.Focus();
            }
            else
            {

                int Stock = Convert.ToInt32(Txtstock.Text);
                int Price = Convert.ToInt32(Txtprice.Text);

                var supplier = myContext.Suppliers.Where(p => p.Id == supplierid).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Name == Txtname.Text).FirstOrDefault();
                if (Txtname.Text != "")
                {
                    if (item != null)
                    {
                        var vqty = item.Stock;
                        var vprice = item.Price;
                        if (Txtprice.Text == vqty.ToString())
                        {
                            laststock = Stock + vqty;
                            item.Stock = Convert.ToInt32(laststock);
                            var result2 = myContext.SaveChanges();
                            if (result2 > 0)
                            {
                                MessageBox.Show("Stock has been inserted !");
                            }
                            else
                            {
                                MessageBox.Show("Stock can't update !");
                            }
                        }
                        else
                        {
                            var pushItem = new Item(Txtname.Text, Stock, Price, supplier);
                            myContext.Items.Add(pushItem);
                            var result = myContext.SaveChanges();
                            if (result > 0)
                            {
                                MessageBox.Show("row has been inserted");

                                GridItem.ItemsSource = myContext.Items.ToList();
                            }
                        }
                        GridItem.ItemsSource = myContext.Items.ToList();
                    }
                    else
                    {
                        var pushItem = new Item(Txtname.Text, Stock, Price, supplier);
                        myContext.Items.Add(pushItem);
                        var result = myContext.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show("row has been inserted");

                            Txtid.Text = "";
                            Txtname.Text = "";
                            Txtstock.Text = "";
                            Txtprice.Text = "";
                            GridItem.ItemsSource = myContext.Items.ToList();
                        }
                    }
                    GridItem.ItemsSource = myContext.Items.ToList();
                    Showdata();
                }
            }
        }

        private void GridItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var data = GridItem.SelectedItem;

                string id = (GridItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                Txtid.Text = id;
                string name = (GridItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                Txtname.Text = name;
                string stock = (GridItem.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                Txtstock.Text = stock;
                string price = (GridItem.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                Txtprice.Text = price;
                string supplier = (GridItem.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
                supp_id.Text = supplier;
                GridItem.ItemsSource = myContext.Items.ToList();
                // btnsubmit.IsEnabled = false;
                // btnedit.IsEnabled = true;
                //btndelete.IsEnabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Txtname_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Txtstock_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Txtprice_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btndelete_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure want to delete data?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {

                    int id = Convert.ToInt32(Txtid.Text);
                    var delRow = myContext.Items.Where(s => s.Id == id).FirstOrDefault();
                    myContext.Items.Remove(delRow);
                    myContext.SaveChanges();
                    GridItem.ItemsSource = myContext.Items.ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Data Deleted !", "Caution !", MessageBoxButton.OK);
                    Txtid.Text = "";
                    Txtname.Text = "";
                    Txtstock.Text = "";
                    Txtprice.Text = "";
                }
            }
        }

        private void btnedit_Click_1(object sender, RoutedEventArgs e)
        {
            var supplier = myContext.Suppliers.Where(s => s.Id == supplierid).FirstOrDefault();
            int id = Convert.ToInt32(Txtid.Text);
            var uRow = myContext.Items.FirstOrDefault(s => s.Id == id);
            uRow.Name = Txtname.Text;
            uRow.Stock = Convert.ToInt32(Txtstock.Text);
            uRow.Price = Convert.ToInt32(Txtprice.Text);
            uRow.Supplier = supplier;
            myContext.SaveChanges();
            GridItem.ItemsSource = myContext.Items.ToList();
            MessageBox.Show("Data Updated !");
            Showdata();
            Txtid.Text = "";
            Txtname.Text = "";
            Txtstock.Text = "";
            Txtprice.Text = "";
            supp_id.Items.Refresh();
            //  btnsubmit.IsEnabled = true;
            //  btnedit.IsEnabled = false;
            //  btndelete.IsEnabled = false;
        }

        private void cbNameitem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemId = Convert.ToInt32(cbNameitem.SelectedValue.ToString());
            var item = myContext.Items.FirstOrDefault(p => p.Id == itemId);

            Txtpricee.Text = item.Price.ToString();
            TxtStock.Text = item.Stock.ToString();
        }


        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            if (Txtquantity.Text == "")
            {
                MessageBox.Show("Quantity !");
                Txtquantity.Focus();
            }
            else
            {

                int price = Convert.ToInt32(Txtpricee.Text);
                int qty = Convert.ToInt32(Txtquantity.Text);
                int subtot = price * qty;

                transid = Convert.ToInt32(TxtID.Text.ToString());
                var trans = myContext.Transactions.Where(t => t.Id == transid).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Id == itemId).FirstOrDefault();


                if (Convert.ToInt32(Txtquantity.Text) <= item.Stock)
                {
                    item.Stock -= Convert.ToInt32(Txtquantity.Text);
                    Transcart.Add(new TransactionItem { Transaction = trans, Item = item, Quantity = Convert.ToInt32(Txtquantity.Text) });
                    GridTransItem.Items.Add(new { Name = cbNameitem.Text, Quantity = Txtquantity.Text, Price = Txtpricee.Text, subTotal = subtot.ToString() });
                    lasttot += subtot;
                    Txttotalpay.Text = Convert.ToString(lasttot);
                    Showdata();
                }
                else
                {
                    MessageBox.Show("Quantity is limit");
                }



                Txttotal.Text = "Rp. " + lasttot.ToString("n0") + ",-";
                Txttotalpay.Text = lasttot.ToString();

                Txtquantity.Text = "";
                Txtpricee.Text = "";
                TxtStock.Text = "";

            }
        }

        private void btndel_Click(object sender, RoutedEventArgs e)
        {
            if (GridTransItem.SelectedItem != null)
            {
                GridTransItem.Items.Remove(GridTransItem.SelectedItem);
            }

        }

        private void Txtquantity_TextPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            int vpay = Convert.ToInt32(Txtpay.Text);
            int totalpay = Convert.ToInt32(Txttotalpay.Text);
            if (Txttotalpay.Text == "")
            {
                MessageBox.Show("Payment required !");
                Txtquantity.Focus();
            }
            else if (totalpay <= vpay)
            {
                transid = Convert.ToInt32(TxtID.Text.ToString());
                var trans = myContext.Transactions.Where(t => t.Id == transid).FirstOrDefault();
                var item = myContext.Items.Where(i => i.Id == itemId).FirstOrDefault();
                int totalprice = Convert.ToInt32(Txttotalpay.Text);
                trans.Total = totalprice;
                Showdata();
                foreach (var transcart in Transcart)
                {
                    myContext.TransactionItems.Add(transcart);
                    myContext.SaveChanges();
                    struk += transcart.Item.Id.ToString() + "\t" + transcart.Item.Name + "\t" + transcart.Item.Price + "\t" + transcart.Quantity + "\t";
                }
                totalprice = 0;
                MessageBox.Show("Your Change is Rp." + (vpay - totalpay).ToString("n0") + "Thank You", "Notification", MessageBoxButton.OK);
                using (PdfDocument document = new PdfDocument())
                {
                    //Add a page to the document
                    PdfPage page = document.Pages.Add();

                    //Create PDF graphics for the page
                    PdfGraphics graphics = page.Graphics;

                    //Set the standard font
                    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                    //Draw the text
                    graphics.DrawString(struk, font, PdfBrushes.Black, new PointF(0, 0));

                    //Save the document
                    document.Save("Output.pdf");

                    #region View the Workbook
                    //Message box confirmation to view the created document.
                    if (MessageBox.Show("Do you want to view the PDF?", "PDF has been created",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                            System.Diagnostics.Process.Start("Output.pdf");

                            //Exit
                            Close();
                        }
                        catch (Win32Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                        Close();
                    #endregion
                }
            }
            else
            {
                MessageBox.Show("Your Payment is Invalid !");
            }
        }

        private void btnclear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void CbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roleid = Convert.ToInt32(CbRole.SelectedValue.ToString());
           
        }

        private void btnsubrole_Click(object sender, RoutedEventArgs e)
        {
            if (Txtnamerole.Text == "")
            {
                MessageBox.Show("Fill Name !");
                TxtName.Focus();
            }
            else
            {
                var pushRole = new Role(Txtnamerole.Text);
                myContext.Roles.Add(pushRole);
                var result = myContext.SaveChanges();
                if (result > 0)
                {
                    MessageBox.Show("Role has been inserted");
                    Txtidrole.Text = "";
                    Txtnamerole.Text = "";
                    GridRole.ItemsSource = myContext.Roles.ToList();
                }
            }
        
        }

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You're Logout !","Message", MessageBoxButton.OK);
            this.Close();
        }

        private void btnchange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnsubregis_Click(object sender, RoutedEventArgs e)
        {
            if (Txtnameregis.Text == "")
            {
                MessageBox.Show("Fill Name !");
                TxtName.Focus();
            }
            else if (Txtemailregis.Text == "")
            {
                MessageBox.Show("Fill Email !");
                Txtemailregis.Focus();
            }
            else
            {
                var emailcheck = myContext.Users.Where(em => em.Email == Txtemailregis.Text).FirstOrDefault();
                pass = Guid.NewGuid().ToString();
                var role = myContext.Roles.Where(r => r.Id == roleid).FirstOrDefault();

                if (emailcheck == null)
                {
                    var pushregis = new User(Txtnameregis.Text, Txtemailregis.Text, pass, role);
                    myContext.Users.Add(pushregis);
                    var resultt = myContext.SaveChanges();
                    if (resultt > 0)
                    {
                        MessageBox.Show("you have been registered !");
                        try
                        {
                            Outlook._Application _app = new Outlook.Application();
                            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            //sesuaikan dengan content yang di xaml
                            mail.To = Txtemailregis.Text;
                            mail.Subject = "Register Notification.";
                            mail.Body = "This Is Your Password :" + pass;
                            mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)mail).Send();
                            MessageBox.Show("message has been sent.", "message", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                        }
                    }
                    GridRegis.ItemsSource = myContext.Users.ToList();
                    Showdata();
                }
            }
        }

        private void btncancel_Click(object sender, RoutedEventArgs e)
        {
            var transitem = Transcart.Find(t => t.Id.ToString() == TxtID.Text);
            if (transitem != null)
            {
                Transcart.Clear();
                Txttotalpay.Text = "0";
                lasttot = 0;
                transitem.Item.Stock += transitem.Quantity;
                GridTransItem.Items.ToString();

            }
        }

        private void Txtpay_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int vpay = Convert.ToInt32(Txtpay.Text);
                int totalpay = Convert.ToInt32(Txttotalpay.Text);
                Txtchange.Text = "Rp. " + (vpay - totalpay).ToString("n0");

            }
            catch (Exception)
            {

            }
        }
        private void btnnew_Click(object sender, RoutedEventArgs e)
        {
            var push = new Transaction();
            myContext.Transactions.Add(push);
            myContext.SaveChanges();
            TxtID.Text = Convert.ToString(push.Id);
            btnadd.IsEnabled = true;
            btnnew.IsEnabled = false;

        }
        private void Clear()
        {
            GridTransItem.Items.Clear();
            TxtID.Text = "";
            Txtquantity.Text = "";
            Txtpricee.Text = "";
            Txttotal.Text = "";
            Txttotalpay.Text = "";
            Txtchange.Text = "";
        }
    }
}
