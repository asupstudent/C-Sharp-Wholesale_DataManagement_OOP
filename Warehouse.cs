using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Warehouse : Form
    {
        FbConnection fbCon;
        FbCommand werehouseCommand;
        FbTransaction warehouseTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        bool isAddInvoice = false;
        bool isExpenditureInvoice = false;
        public Warehouse()
        {
            InitializeComponent();
        }
        //Флаг добавления накладной
        public void setAddInvoiceFlag()
        {
            isAddInvoice = true;
        }
        //Флаг расходной накладной
        public void setExpenditureInvoice()
        {
            isExpenditureInvoice = true;
        }
        //Удалить пустые строки в DataTable
        public static void RemoveNullColumnFromDataTable(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] != DBNull.Value)
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();
        }
        //Обновить таблицу
        private void refreshTable()
        {
            if(isExpenditureInvoice)
            {
                try
                {
                    fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    fbCon.Open();
                    command = "SELECT WAREHOUSE.ID AS \"Номер\", " +
                                "(SELECT PRODUCT.ID FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) AS \"ID\", " +
                                "(SELECT PRODUCT.PRICE FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) AS \"Цена\", " +
                                "(SELECT PRODUCT.NAME FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) AS \"Товар\", " +
                                "WAREHOUSE.LINE AS \"Ряд\", " +
                                "WAREHOUSE.SHELF AS \"Полка\", " +
                                "WAREHOUSE.PLACE AS \"Место\" " +
                                "FROM INVOICE_TABLE_PART, WAREHOUSE WHERE WAREHOUSE.ID = INVOICE_TABLE_PART.ID_WAREHOUSE;";
                    werehouseCommand = new FbCommand(command, fbCon);
                    werehouseCommand.CommandType = CommandType.Text;
                    dr = werehouseCommand.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[3];
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
            else
            {
                try
                {
                    fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    fbCon.Open();
                    command = "SELECT WAREHOUSE.ID AS \"Номер\", " +
                                "(SELECT PRODUCT.NAME FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) AS \"Товар\", " +
                                "WAREHOUSE.LINE AS \"Ряд\", " +
                                "WAREHOUSE.SHELF AS \"Полка\", " +
                                "WAREHOUSE.PLACE AS \"Место\" " +
                                "FROM INVOICE_TABLE_PART RIGHT JOIN WAREHOUSE ON WAREHOUSE.ID = INVOICE_TABLE_PART.ID_WAREHOUSE;";
                    werehouseCommand = new FbCommand(command, fbCon);
                    werehouseCommand.CommandType = CommandType.Text;
                    dr = werehouseCommand.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    if (isAddInvoice)
                    {
                        RemoveNullColumnFromDataTable(dt);
                    }
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
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
        //Отобразить кнопу для выбора склада
        public void setChooseButton()
        {
            button6.Visible = true;
        }
        //Проверить затнято ли место на складе
        private bool checkPlace(int row, int shelf, int place)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT WAREHOUSE.ID FROM WAREHOUSE WHERE WAREHOUSE.LINE = @row AND WAREHOUSE.SHELF = @shelf AND WAREHOUSE.PLACE = @place";
                werehouseCommand = new FbCommand(command, fbCon);
                werehouseCommand.Parameters.AddWithValue("@row", row);
                werehouseCommand.Parameters.AddWithValue("@shelf", shelf);
                werehouseCommand.Parameters.AddWithValue("@place", place);
                werehouseCommand.CommandType = CommandType.Text;
                dr = werehouseCommand.ExecuteReader();
                if(dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
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
            return true;
        }
        //Получить выделенную запись из DataGridView
        public DataGridViewRow getCurrentRecord()
        {
            return dataGridView1.CurrentRow;
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
        //Удалить запись
        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы действиетльно хотите удалить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                    fbCon.Open();
                    warehouseTransaction = fbCon.BeginTransaction();
                    command = "DELETE FROM WAREHOUSE WHERE WAREHOUSE.ID = @current_record;";
                    werehouseCommand = new FbCommand(command, fbCon, warehouseTransaction);
                    werehouseCommand.Parameters.AddWithValue("@current_record", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    werehouseCommand.CommandType = CommandType.Text;
                    werehouseCommand.ExecuteNonQuery();
                    warehouseTransaction.Commit();
                }
                catch (Exception)
                {
                    MessageBox.Show("Запись удалить нельзя, она используется");
                }
                finally
                {
                    fbCon.Close();
                }
            }
            this.refreshTable();
        }
        //Изменить место на слкладе
        private void button3_Click(object sender, EventArgs e)
        {
            using (WarehouseEdit wef = new WarehouseEdit())
            {
                wef.Text = "Изменение места на складе";
                wef.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                wef.setLine(Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value));
                wef.setShelf(Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value));
                wef.setPlace(Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value));
                wef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (wef.DialogResult == DialogResult.OK)
                    {
                        if (checkPlace(wef.getLine(), wef.getShelf(), wef.getPlace()) ||
                                       wef.getLine() == 0 ||
                                       wef.getShelf() == 0 ||
                                       wef.getPlace() == 0)
                        {
                            wef.setLineColor();
                            wef.setShelfColor();
                            wef.setPlaceColor();
                            fe.Cancel = true;
                            MessageBox.Show("Место на складе уже существует или указаны некорректные данные");
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                warehouseTransaction = fbCon.BeginTransaction();
                                command = "UPDATE WAREHOUSE SET WAREHOUSE.LINE = @line, WAREHOUSE.SHELF = @shelf, WAREHOUSE.PLACE = @place WHERE WAREHOUSE.ID = @current_id;";
                                werehouseCommand = new FbCommand(command, fbCon, warehouseTransaction);
                                werehouseCommand.Parameters.AddWithValue("@line", wef.getLine());
                                werehouseCommand.Parameters.AddWithValue("@shelf", wef.getShelf());
                                werehouseCommand.Parameters.AddWithValue("@place", wef.getPlace());
                                werehouseCommand.Parameters.AddWithValue("@current_id", wef.getID());
                                werehouseCommand.CommandType = CommandType.Text;
                                werehouseCommand.ExecuteNonQuery();
                                warehouseTransaction.Commit();
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
                wef.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Добавить место на склад
        private void button4_Click(object sender, EventArgs e)
        {
            using (WarehouseEdit wef = new WarehouseEdit())
            {
                wef.Text = "Добавление места на складе";
                wef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (wef.DialogResult == DialogResult.OK)
                    {
                        if (checkPlace(wef.getLine(), wef.getShelf(), wef.getPlace()) ||
                                       wef.getLine() == 0 ||
                                       wef.getShelf() == 0 ||
                                       wef.getPlace() == 0)
                        {
                            wef.setLineColor();
                            wef.setShelfColor();
                            wef.setPlaceColor();
                            fe.Cancel = true;
                            MessageBox.Show("Место на складе уже существует или указаны некорректные данные");
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                warehouseTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO WAREHOUSE (LINE, SHELF, PLACE) VALUES (@line, @shelf, @place);";
                                werehouseCommand = new FbCommand(command, fbCon, warehouseTransaction);
                                werehouseCommand.Parameters.AddWithValue("@line", wef.getLine());
                                werehouseCommand.Parameters.AddWithValue("@shelf", wef.getShelf());
                                werehouseCommand.Parameters.AddWithValue("@place", wef.getPlace());
                                werehouseCommand.CommandType = CommandType.Text;
                                werehouseCommand.ExecuteNonQuery();
                                warehouseTransaction.Commit();
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
                wef.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Поиск по названию товара
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT WAREHOUSE.ID AS \"Номер\", " +
                            "(SELECT PRODUCT.NAME FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) AS \"Товар\", " +
                            "WAREHOUSE.ID AS \"Ряд\", " +
                            "WAREHOUSE.SHELF AS \"Полка\", " +
                            "WAREHOUSE.PLACE AS \"Место\" " +
                            "FROM INVOICE_TABLE_PART RIGHT JOIN WAREHOUSE ON WAREHOUSE.ID = INVOICE_TABLE_PART.ID_WAREHOUSE " +
                            "WHERE (SELECT PRODUCT.NAME FROM PRODUCT WHERE PRODUCT.ID = INVOICE_TABLE_PART.ID_PRODUCT) LIKE @name";
                werehouseCommand = new FbCommand(command, fbCon);
                werehouseCommand.Parameters.AddWithValue("@name", "%" + textBox1.Text + "%");
                werehouseCommand.CommandType = CommandType.Text;
                dr = werehouseCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
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
        //Обновить форму
        private void button5_Click(object sender, EventArgs e)
        {
            this.refreshTable();
            this.textBox1.Text = "";
        }
    }
}
