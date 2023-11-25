using System;
using System.Configuration;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

//Последняя версия проекта

namespace Wholesale
{
    public partial class Main : Form
    {
        FbConnectionStringBuilder fb_cons;
        FbConnection fbCon;
        public Main()
        {
            InitializeComponent();
        }
        //Подключение к БД
        private void Main_Load(object sender, EventArgs e)
        {
            fb_cons = new FbConnectionStringBuilder();
            fb_cons.Charset = "UTF8";
            fb_cons.ClientLibrary = ".\\fbclient.dll";
            fb_cons.UserID = "SYSDBA";
            fb_cons.Password = "masterkey";
            fb_cons.Dialect = 3;
            //ПРИ ПЕРЕДАЧЕ ИЗМЕНИТЬ НА ПРАВИЛЬНЫЙ
            fb_cons.Database = "..\\..\\db\\WHOLESALE.FDB";
            //fb_cons.Database = ".\\db\\WHOLESALE.FDB";
            fb_cons.ServerType = FbServerType.Embedded;
            //ЕСЛИ РАЗВОРАЧИВАТЬ НА ПОЛНОЦЕННОМ СЕРВЕРЕ
            //fb_cons.ServerType = 0;
            //fb_cons.DataSource = "localhost";
            //fb_cons.Port = 3050;
            Program.AddUpdateAppSettings("ConnectionString", fb_cons.ToString());
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                fbCon.Close();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                fbCon.Close();
            }
        }
        //Вызов формы Сотрудники
        private void button2_Click(object sender, EventArgs e)
        {
            Employee emp_loaded = (Employee) Application.OpenForms["Employee"];
            if(emp_loaded == null)
            {
                Employee emp = new Employee();
                emp.ShowDialog();
            }
            else
            {
                emp_loaded.Activate();
            }
        }
        //Вызов формы Контрагенты
        private void button3_Click(object sender, EventArgs e)
        {
            Contractor cmp_loaded = (Contractor)Application.OpenForms["Contractor"];
            if (cmp_loaded == null)
            {
                Contractor contractor = new Contractor();
                contractor.ShowDialog();
            }
            else
            {
                cmp_loaded.Activate();
            }
        }
        //Вызов формы Склад
        private void button4_Click(object sender, EventArgs e)
        {
            Warehouse wh_loaded = (Warehouse)Application.OpenForms["Warehouse"];
            if (wh_loaded == null)
            {
                Warehouse warehouse = new Warehouse();
                warehouse.ShowDialog();
            }
            else
            {
                wh_loaded.Activate();
            }
        }
        //Вызов формы Товары
        private void button5_Click(object sender, EventArgs e)
        {
            Product product_loaded = (Product)Application.OpenForms["Product"];
            if (product_loaded == null)
            {
                Product product = new Product();
                product.ShowDialog();
            }
            else
            {
                product_loaded.Activate();
            }
        }
        //Вызов формы Накладные
        private void button6_Click(object sender, EventArgs e)
        {
            Invoices invoices_loaded = (Invoices)Application.OpenForms["Invoices"];
            if (invoices_loaded == null)
            {
                Invoices invoices = new Invoices();
                invoices.ShowDialog();
            }
            else
            {
                invoices_loaded.Activate();
            }
        }
        //Закрыть приложение
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Вызов формы категории
        private void button1_Click(object sender, EventArgs e)
        {
            Category category_loaded = (Category)Application.OpenForms["Category"];
            if (category_loaded == null)
            {
                Category category = new Category();
                category.ShowDialog();
            }
            else
            {
                category_loaded.Activate();
            }
        }
    }
}
