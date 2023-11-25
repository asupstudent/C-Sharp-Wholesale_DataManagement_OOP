using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Category : Form
    {
        FbConnection fbCon;
        FbCommand categoryCommand;
        FbTransaction categoryTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        public Category()
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
                command = "SELECT ID AS \"Номер\", NAME AS \"Название категории\" FROM CATEGORY;";
                categoryCommand = new FbCommand(command, fbCon);
                categoryCommand.CommandType = CommandType.Text;
                dr = categoryCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
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

        //Разрешить кнопку ВЫБРАТЬ (выбрать категорию)
        public void setChooseButton()
        {
            button6.Visible = true;
        }

        //Получить текущую запись из dataGridView
        public DataGridViewRow getCurrentRecord()
        {
            return dataGridView1.CurrentRow;
        }

        //Вызывается при загрузке формы
        private void EMPLOYEE_Load(object sender, EventArgs e)
        {
            refreshTable();
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
                    categoryTransaction = fbCon.BeginTransaction();
                    command = "DELETE FROM CATEGORY WHERE CATEGORY.ID = @current_record;";
                    categoryCommand = new FbCommand(command, fbCon, categoryTransaction);
                    categoryCommand.Parameters.AddWithValue("@current_record", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    categoryCommand.CommandType = CommandType.Text;
                    categoryCommand.ExecuteNonQuery();
                    categoryTransaction.Commit();
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

        //Изменение записи
        private void button3_Click(object sender, EventArgs e)
        {
            using (CategoryEdit cef = new CategoryEdit())
            {
                cef.Text = "Изменение категории";
                cef.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                cef.setName(Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value));
                cef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cef.DialogResult == DialogResult.OK)
                    {
                        if (cef.getName().Length == 0)
                        {
                            cef.setNameColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                categoryTransaction = fbCon.BeginTransaction();
                                command = "UPDATE CATEGORY SET CATEGORY.NAME = @name WHERE CATEGORY.ID = @current_id;";
                                categoryCommand = new FbCommand(command, fbCon, categoryTransaction);
                                categoryCommand.Parameters.AddWithValue("@name", cef.getName());
                                categoryCommand.Parameters.AddWithValue("@current_id", cef.getID());
                                categoryCommand.CommandType = CommandType.Text;
                                categoryCommand.ExecuteNonQuery();
                                categoryTransaction.Commit();
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
        //Добавление записи
        private void button4_Click(object sender, EventArgs e)
        {
            using (CategoryEdit cef = new CategoryEdit())
            {
                cef.Text = "Добавление категории";
                cef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cef.DialogResult == DialogResult.OK)
                    {
                        if (cef.getName().Length == 0)
                        {
                            cef.setNameColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                categoryTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO CATEGORY (NAME) VALUES (@name);";
                                categoryCommand = new FbCommand(command, fbCon, categoryTransaction);
                                categoryCommand.Parameters.AddWithValue("@name", cef.getName());
                                categoryCommand.CommandType = CommandType.Text;
                                categoryCommand.ExecuteNonQuery();
                                categoryTransaction.Commit();
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

        //Поиск ко названию категории
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT ID AS \"Номер\", NAME AS \"Название категории\" FROM CATEGORY WHERE CATEGORY.NAME LIKE @name";
                categoryCommand = new FbCommand(command, fbCon);
                categoryCommand.Parameters.AddWithValue("@name", "%" + textBox1.Text + "%");
                categoryCommand.CommandType = CommandType.Text;
                dr = categoryCommand.ExecuteReader();
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
