using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Product : Form
    {
        FbConnection fbCon;
        FbCommand productCommand;
        FbTransaction productTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        public Product()
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
                command = "SELECT PRODUCT.ID AS \"Номер\", " +
                    "PRODUCT.NAME AS \"Название\", " +
                    "PRODUCT.TRADEMARK AS \"Торговая марка\", " +
                    "CATEGORY.NAME AS \"Категория\", " +
                    "PRODUCT.PRICE AS \"Цена\", " +
                    "PRODUCT.UNIT AS \"Ед. изм.\", " +
                    "PRODUCT.AMOUNT AS \"Кол.\", " +
                    "PRODUCT.MIN_STOCK AS \"Мин.\", " +
                    "PRODUCT.WANT_STOCK AS \"Жел.\", " +
                    "PRODUCT.DESCRIPTION AS \"Описание.\" " +
                    "FROM PRODUCT, CATEGORY " +
                    "WHERE PRODUCT.ID_CATEGORY = CATEGORY.ID;";
                productCommand = new FbCommand(command, fbCon);
                productCommand.CommandType = CommandType.Text;
                dr = productCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[1].Width = 200;
                dataGridView1.Columns[2].Width = 200;
                dataGridView1.Focus();
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[1];
            }
            catch (Exception)
            {
                MessageBox.Show("В таблице нет ни одной записи");
            }
            finally
            {
                fbCon.Close();
            }
        }
        //Отобразить кнопку выбора
        public void setChooseButton()
        {
            button6.Visible = true;
        }
        //Получить выделенную запись из dataGridView
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
                    productTransaction = fbCon.BeginTransaction();
                    command = "DELETE FROM PRODUCT WHERE PRODUCT.ID = @current_record;";
                    productCommand = new FbCommand(command, fbCon, productTransaction);
                    productCommand.Parameters.AddWithValue("@current_record", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    productCommand.CommandType = CommandType.Text;
                    productCommand.ExecuteNonQuery();
                    productTransaction.Commit();
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
        //Получить идентификатор категории
        private int getIdCategoryValue(string category)
        {
            object id = 0;
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT ID FROM CATEGORY WHERE NAME = '" + category + "';" ;
                productCommand = new FbCommand(command, fbCon);
                productCommand.CommandType = CommandType.Text;
                dr = productCommand.ExecuteReader();
                while (dr.Read())
                {
                    id = dr[0];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось получить идентификатор категории");
            }
            finally
            {
                fbCon.Close();
            }
            return Convert.ToInt32(id);
        }
        //Изменить товар
        private void button3_Click(object sender, EventArgs e)
        {
            string category = Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
            int id_category = this.getIdCategoryValue(category);
            using (ProductEdit cef = new ProductEdit())
            {
                cef.Text = "Изменение товара";
                cef.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                cef.setName(Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value));
                cef.setTrademark(Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value));
                cef.setIdCategory(category);
                cef.setPrice(Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value));
                cef.setUnit(Convert.ToString(dataGridView1[5, dataGridView1.CurrentRow.Index].Value));
                cef.setAmount(Convert.ToString(dataGridView1[6, dataGridView1.CurrentRow.Index].Value));
                cef.setMinStock(Convert.ToString(dataGridView1[7, dataGridView1.CurrentRow.Index].Value));
                cef.setWantStock(Convert.ToString(dataGridView1[8, dataGridView1.CurrentRow.Index].Value));
                cef.setDescription(Convert.ToString(dataGridView1[9, dataGridView1.CurrentRow.Index].Value));
                cef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cef.DialogResult == DialogResult.OK)
                    {
                        if (cef.getName().Length == 0 ||
                            cef.getTrademark().Length == 0 ||
                            cef.getIdCategory().Length == 0 ||
                            cef.getPrice().Length == 0 ||
                            cef.getUnit().Length == 0 ||
                            cef.getMinStock().Length == 0 ||
                            cef.getWantStock().Length == 0 ||
                            cef.getDescription().Length == 0)
                        {
                            cef.setNameColor();
                            cef.setTrademarkColor();
                            cef.setIdCategoryColor();
                            cef.setPriceColor();
                            cef.setMinStockColor();
                            cef.setWantStockColor();
                            cef.setDescriptionColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                productTransaction = fbCon.BeginTransaction();
                                command = "UPDATE PRODUCT SET PRODUCT.NAME = @name, " +
                                                              "PRODUCT.TRADEMARK = @trademark, " +
                                                              "PRODUCT.ID_CATEGORY = @id_category, " +
                                                              "PRODUCT.PRICE = @price, " +
                                                              "PRODUCT.UNIT = @unit, " +
                                                              "PRODUCT.MIN_STOCK = @min_stock, " +
                                                              "PRODUCT.WANT_STOCK = @want_stock, " +
                                                              "PRODUCT.DESCRIPTION = @description " +
                                                              "WHERE PRODUCT.ID = @current_id;";
                                productCommand = new FbCommand(command, fbCon, productTransaction);
                                productCommand.Parameters.AddWithValue("@current_id", cef.getID());
                                productCommand.Parameters.AddWithValue("@name", cef.getName());
                                productCommand.Parameters.AddWithValue("@trademark", cef.getTrademark());
                                productCommand.Parameters.Add("@id_category", FbDbType.Integer).Value = id_category;
                                productCommand.Parameters.Add("@price", FbDbType.Decimal).Value = Convert.ToDecimal(cef.getPrice());
                                productCommand.Parameters.AddWithValue("@unit", cef.getUnit());
                                productCommand.Parameters.Add("@min_stock", FbDbType.Integer).Value = Convert.ToInt32(cef.getMinStock());
                                productCommand.Parameters.Add("@want_stock", FbDbType.Integer).Value = Convert.ToInt32(cef.getWantStock());
                                productCommand.Parameters.AddWithValue("@description", cef.getDescription());
                                productCommand.CommandType = CommandType.Text;
                                productCommand.ExecuteNonQuery();
                                productTransaction.Commit();
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
                cef.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Добавить товар
        private void button4_Click(object sender, EventArgs e)
        {
            using (ProductEdit pef = new ProductEdit())
            {
                pef.Text = "Добавление товара";
                pef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (pef.DialogResult == DialogResult.OK)
                    {
                        if (pef.getName().Length == 0 ||
                            pef.getTrademark().Length == 0 ||
                            pef.getIdCategory().Length == 0 ||
                            pef.getPrice().Length == 0 ||
                            pef.getUnit().Length == 0 ||
                            pef.getMinStock().Length == 0 ||
                            pef.getWantStock().Length == 0 ||
                            pef.getDescription().Length == 0)
                        {
                            pef.setNameColor();
                            pef.setTrademarkColor();
                            pef.setIdCategoryColor();
                            pef.setPriceColor();
                            pef.setMinStockColor();
                            pef.setWantStockColor();
                            pef.setDescriptionColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                productTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO PRODUCT (NAME, TRADEMARK, ID_CATEGORY, PRICE, UNIT, AMOUNT, MIN_STOCK, WANT_STOCK, DESCRIPTION) " +
                                "VALUES (@name, @trademark, @id_category, @price, @unit, @amount, @min_stock, @want_stock, @description);";
                                productCommand = new FbCommand(command, fbCon, productTransaction);
                                productCommand.Parameters.AddWithValue("@name", pef.getName());
                                productCommand.Parameters.AddWithValue("@trademark", pef.getTrademark());
                                productCommand.Parameters.Add("@id_category", FbDbType.Integer).Value = pef.getIdCategoryValue();
                                productCommand.Parameters.Add("@price", FbDbType.Decimal).Value = Convert.ToDecimal(pef.getPrice());
                                productCommand.Parameters.AddWithValue("@unit", pef.getUnit());
                                productCommand.Parameters.Add("@amount", FbDbType.Integer).Value = 0;
                                productCommand.Parameters.Add("@min_stock", FbDbType.Integer).Value = Convert.ToInt32(pef.getMinStock());
                                productCommand.Parameters.Add("@want_stock", FbDbType.Integer).Value = Convert.ToInt32(pef.getWantStock());
                                productCommand.Parameters.AddWithValue("@description", pef.getDescription());
                                productCommand.CommandType = CommandType.Text;
                                productCommand.ExecuteNonQuery();
                                productTransaction.Commit();
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
                pef.ShowDialog(this);
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
                command = "SELECT PRODUCT.ID AS \"Номер\", " +
                    "PRODUCT.NAME AS \"Название\", " +
                    "PRODUCT.TRADEMARK AS \"Торговая марка\", " +
                    "CATEGORY.NAME AS \"Категория\", " +
                    "PRODUCT.PRICE AS \"Цена\", " +
                    "PRODUCT.UNIT AS \"Ед. изм.\", " +
                    "PRODUCT.AMOUNT AS \"Кол.\", " +
                    "PRODUCT.MIN_STOCK AS \"Мин.\", " +
                    "PRODUCT.WANT_STOCK AS \"Жел.\", " +
                    "PRODUCT.DESCRIPTION AS \"Описание.\" " +
                    "FROM PRODUCT, CATEGORY " +
                    "WHERE PRODUCT.ID_CATEGORY = CATEGORY.ID AND PRODUCT.NAME LIKE @name;";
                productCommand = new FbCommand(command, fbCon);
                productCommand.Parameters.AddWithValue("@name", "%" + textBox1.Text + "%");
                productCommand.CommandType = CommandType.Text;
                dr = productCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[1].Width = 200;
                dataGridView1.Columns[2].Width = 200;
            }
            catch (Exception)
            {
                MessageBox.Show("Поиск завершился с ошибкой");
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
