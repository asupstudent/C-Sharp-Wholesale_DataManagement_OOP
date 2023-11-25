using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Employee : Form
    {
        FbConnection fbCon;
        FbCommand employeeCommand;
        FbTransaction employeeTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        public Employee()
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
                command = "SELECT ID AS \"Номер\", FIO AS \"ФИО\", POST AS \"Должность\" FROM EMPLOYEE;";
                employeeCommand = new FbCommand(command, fbCon);
                employeeCommand.CommandType = CommandType.Text;
                dr = employeeCommand.ExecuteReader();
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
        //Разрешить кнопку ВЫБРАТЬ (выбрать сотрудника)
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
                    employeeTransaction = fbCon.BeginTransaction();
                    command = "DELETE FROM EMPLOYEE WHERE EMPLOYEE.ID = @current_record;";
                    employeeCommand = new FbCommand(command, fbCon, employeeTransaction);
                    employeeCommand.Parameters.AddWithValue("@current_record", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    employeeCommand.CommandType = CommandType.Text;
                    employeeCommand.ExecuteNonQuery();
                    employeeTransaction.Commit();
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
            using (EmployeeEdit eef = new EmployeeEdit())
            {
                eef.Text = "Изменение сотрудника";
                eef.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                eef.setFIO(Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value));
                eef.setPost(Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value));
                eef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (eef.DialogResult == DialogResult.OK)
                    {
                        if (eef.getFIO().Length == 0 || eef.getPost().Length == 0)
                        {
                            eef.setFIOColor();
                            eef.setPostColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                employeeTransaction = fbCon.BeginTransaction();
                                command = "UPDATE EMPLOYEE SET EMPLOYEE.FIO = @fio, EMPLOYEE.POST = @post WHERE EMPLOYEE.ID = @current_id;";
                                employeeCommand = new FbCommand(command, fbCon, employeeTransaction);
                                employeeCommand.Parameters.AddWithValue("@fio", eef.getFIO());
                                employeeCommand.Parameters.AddWithValue("@post", eef.getPost());
                                employeeCommand.Parameters.AddWithValue("@current_id", eef.getID());
                                employeeCommand.CommandType = CommandType.Text;
                                employeeCommand.ExecuteNonQuery();
                                employeeTransaction.Commit();
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
                eef.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Добавление записи
        private void button4_Click(object sender, EventArgs e)
        {
            using (EmployeeEdit eef = new EmployeeEdit())
            {
                eef.Text = "Добавление сотрудника";
                eef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (eef.DialogResult == DialogResult.OK)
                    {
                        if(eef.getFIO().Length == 0 || eef.getPost().Length == 0)
                        {
                            eef.setFIOColor();
                            eef.setPostColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                employeeTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO EMPLOYEE (FIO, POST) VALUES (@fio, @post);";
                                employeeCommand = new FbCommand(command, fbCon, employeeTransaction);
                                employeeCommand.Parameters.AddWithValue("@fio", eef.getFIO());
                                employeeCommand.Parameters.AddWithValue("@post", eef.getPost());
                                employeeCommand.CommandType = CommandType.Text;
                                employeeCommand.ExecuteNonQuery();
                                employeeTransaction.Commit();
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
                eef.ShowDialog(this);
            }
            this.refreshTable();
        }
        //Поиск ко названию ФИО
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT ID AS \"Номер\", FIO AS \"ФИО\", POST AS \"Должность\" FROM EMPLOYEE WHERE EMPLOYEE.FIO LIKE @surname";
                employeeCommand = new FbCommand(command, fbCon);
                employeeCommand.Parameters.AddWithValue("@surname", "%" + textBox1.Text + "%");
                employeeCommand.CommandType = CommandType.Text;
                dr = employeeCommand.ExecuteReader();
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
        //Обновить таблицу
        private void button5_Click(object sender, EventArgs e)
        {
            this.refreshTable();
            this.textBox1.Text = "";
        }
    }
}
