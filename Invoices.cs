using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Invoices : Form
    {
        FbConnection fbCon;
        FbCommand invoiceCommand;
        FbTransaction invoiceTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        public Invoices()
        {
            InitializeComponent();
        }
        //Обновить таблицу
        private void refreshTable()
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT INVOICE_HEADER.ID AS \"Номер\", " +
                    "CONTRACTOR.NAME AS \"Контрагент\", " +
                    "EMPLOYEE.FIO AS \"Сотрудник\", " +
                    "INVOICE_TYPE.NAME AS \"Тип накладной\", " +
                    "INVOICE_HEADER.INVOICE_DATE AS \"Дата\" " +
                    "FROM INVOICE_HEADER, CONTRACTOR, EMPLOYEE, INVOICE_TYPE " +
                    "WHERE INVOICE_HEADER.ID_CONTRACTOR = CONTRACTOR.ID AND " +
                    "INVOICE_HEADER.ID_EMPLOYEE = EMPLOYEE.ID AND " +
                    "INVOICE_HEADER.ID_INVOICE_TYPE = INVOICE_TYPE.ID;";
                invoiceCommand = new FbCommand(command, fbCon);
                invoiceCommand.CommandType = CommandType.Text;
                dr = invoiceCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.Columns[4].Width = 150;
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
        //При загрузке формы
        private void EMPLOYEE_Load(object sender, EventArgs e)
        {
            this.refreshTable();
        }
        //Закрыть форму
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Просмотр накладной
        private void button3_Click(object sender, EventArgs e)
        {
            using (InvoicesEdit ief = new InvoicesEdit())
            {
                ief.Text = "Товарная накладная";
                ief.loadComboBoxValues();
                ief.setView();
                ief.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                ief.setContractor(Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value));
                ief.setEmployee(Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value));
                ief.setTypeInvoice(Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value));
                ief.setDate(Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value));

                try
                {
                    fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    fbCon.Open();
                    command = "SELECT PRODUCT.NAME AS \"Товар\", " +
                                    "INVOICE_TABLE_PART.AMOUNT AS \"Количество\", " +
                                    "PRODUCT.PRICE AS \"Цена\", " +
                                    "INVOICE_TABLE_PART.ID_WAREHOUSE AS \"Место на складе\", " +
                                    "INVOICE_TABLE_PART.TOTAL_PRICE AS \"Полная стоимость\" " +
                                    "FROM PRODUCT, INVOICE_TABLE_PART " +
                                    "WHERE INVOICE_TABLE_PART.ID_PRODUCT = PRODUCT.ID AND INVOICE_TABLE_PART.ID_INVOICE_HEADER = @id";
                    invoiceCommand = new FbCommand(command, fbCon);
                    invoiceCommand.Parameters.AddWithValue("@id", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    invoiceCommand.CommandType = CommandType.Text;
                    dr = invoiceCommand.ExecuteReader();
                    ief.setInvoicesTableData(dr);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    fbCon.Close();
                }
                ief.ShowDialog(this);
            }
        }
        //Добавление накладной
        private void button4_Click(object sender, EventArgs e)
        {
            using (InvoicesEdit ief = new InvoicesEdit(true))
            {
                ief.Text = "Добавление товарной накладной";
                ief.loadComboBoxValues();
                ief.unsubscribeMouseDataGridView();
                ief.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (ief.DialogResult == DialogResult.OK)
                    {
                        if(ief.getContractor() == 0 ||
                            ief.getEmployee() == 0 ||
                            ief.checkRowCount())
                        {
                            ief.setContractorColor();
                            ief.setEmployeeColor();
                            ief.setProductListColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                invoiceTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO INVOICE_HEADER (ID_CONTRACTOR, ID_EMPLOYEE, ID_INVOICE_TYPE, INVOICE_DATE) VALUES (@contractor, @employee, @invoice_type, @invoice_date);";
                                invoiceCommand = new FbCommand(command, fbCon, invoiceTransaction);
                                invoiceCommand.Parameters.AddWithValue("@contractor", ief.getContractor());
                                invoiceCommand.Parameters.AddWithValue("@employee", ief.getEmployee());
                                invoiceCommand.Parameters.AddWithValue("@invoice_type", ief.getTypeInvoice());
                                invoiceCommand.Parameters.AddWithValue("@invoice_date", DateTime.Now);
                                invoiceCommand.CommandType = CommandType.Text;
                                invoiceCommand.ExecuteNonQuery();
                                invoiceTransaction.Commit();

                                command = "select gen_id(GEN_INVOICE_HEADER_ID, 0) from rdb$database;";
                                invoiceCommand = new FbCommand(command, fbCon);
                                invoiceCommand.CommandType = CommandType.Text;
                                dr = invoiceCommand.ExecuteReader();
                                dr.Read();

                                int last_id = Convert.ToInt32(dr.GetValue(0));

                                DataTable dt = ief.getDataTableProducts();
                                foreach (DataRow row in dt.Rows)
                                {
                                    string id_product = row["ID товара"].ToString();
                                    string amount = row["Количество"].ToString();
                                    string price = row["Цена"].ToString();
                                    string place = row["Место на складе"].ToString();

                                    invoiceTransaction = fbCon.BeginTransaction();
                                    command = "INSERT INTO INVOICE_TABLE_PART (ID_INVOICE_HEADER, ID_PRODUCT, AMOUNT, TOTAL_PRICE, ID_WAREHOUSE) VALUES (@id, @product, @amount, @price, @place);";
                                    invoiceCommand = new FbCommand(command, fbCon, invoiceTransaction);
                                    invoiceCommand.Parameters.Add("@id", FbDbType.Integer).Value = last_id;
                                    invoiceCommand.Parameters.Add("@product", FbDbType.Integer).Value = Convert.ToInt32(id_product);
                                    invoiceCommand.Parameters.Add("@amount", FbDbType.Integer).Value = Convert.ToInt32(amount);
                                    invoiceCommand.Parameters.Add("@price", FbDbType.Decimal).Value = Convert.ToDecimal(price);
                                    invoiceCommand.Parameters.Add("@place", FbDbType.Integer).Value = Convert.ToInt32(place);
                                    invoiceCommand.CommandType = CommandType.Text;
                                    invoiceCommand.ExecuteNonQuery();
                                    invoiceTransaction.Commit();

                                    command = "SELECT PRODUCT.AMOUNT FROM PRODUCT WHERE PRODUCT.ID = @current_id;";
                                    invoiceCommand = new FbCommand(command, fbCon);
                                    invoiceCommand.Parameters.AddWithValue("@current_id", Convert.ToInt32(id_product));
                                    invoiceCommand.CommandType = CommandType.Text;
                                    dr = invoiceCommand.ExecuteReader();
                                    dr.Read();

                                    int amount_product = Convert.ToInt32(dr.GetValue(0));

                                    if (ief.getTypeInvoice() == "1")
                                    {
                                        amount_product += Convert.ToInt32(amount);
                                    }
                                    else
                                    {
                                        amount_product -= Convert.ToInt32(amount);
                                    }
                                    if(amount_product > 0)
                                    {
                                        invoiceTransaction = fbCon.BeginTransaction();
                                        command = "UPDATE PRODUCT SET PRODUCT.AMOUNT = @amount WHERE PRODUCT.ID = @current_id;";
                                        invoiceCommand = new FbCommand(command, fbCon, invoiceTransaction);
                                        invoiceCommand.Parameters.AddWithValue("@amount", amount_product);
                                        invoiceCommand.Parameters.AddWithValue("@current_id", Convert.ToInt32(id_product));
                                        invoiceCommand.CommandType = CommandType.Text;
                                        invoiceCommand.ExecuteNonQuery();
                                        invoiceTransaction.Commit();
                                    }
                                    else
                                    {
                                        MessageBox.Show("На складе недостаточно товара");
                                    }

                                }
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
                    }
                };
                ief.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Поиск по накладной
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();

                command = "SELECT INVOICE_HEADER.ID AS \"Номер\", " +
                                "CONTRACTOR.NAME AS \"Контрагент\", " +
                                "EMPLOYEE.FIO AS \"Сотрудник\", " +
                                "INVOICE_TYPE.NAME AS \"Тип накладной\", " +
                                "INVOICE_HEADER.INVOICE_DATE AS \"Дата\" " +
                                "FROM INVOICE_HEADER, CONTRACTOR, EMPLOYEE, INVOICE_TYPE " +
                                "WHERE INVOICE_HEADER.ID_CONTRACTOR = CONTRACTOR.ID AND " +
                                "INVOICE_HEADER.ID_EMPLOYEE = EMPLOYEE.ID AND " +
                                "INVOICE_HEADER.ID_INVOICE_TYPE = INVOICE_TYPE.ID AND " +
                                "INVOICE_HEADER.ID LIKE @id;";
                invoiceCommand = new FbCommand(command, fbCon);
                invoiceCommand.Parameters.AddWithValue("@id", "%" + textBox1.Text + "%");
                invoiceCommand.CommandType = CommandType.Text;
                dr = invoiceCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.Columns[4].Width = 150;
                dataGridView1.DataSource = dt;
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
        //Обновить таблицу
        private void button5_Click(object sender, EventArgs e)
        {
            this.refreshTable();
            this.textBox1.Text = "";
        }
        //Ввод только цифр
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
    }
}
