using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class Contractor : Form
    {
        FbConnection fbCon;
        FbCommand contractorCommand;
        FbTransaction contractorTransaction;
        FbDataReader dr;
        string command;
        DataTable dt;
        public Contractor()
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
                command = "SELECT ID AS \"Номер\", NAME AS \"Название\", ACTUAL_ADDRESS AS \"Факт. адрес\", LEGAL_ADDRESS AS \"Юр. адрес\", PHONE AS \"Телефон\" FROM CONTRACTOR;";
                contractorCommand = new FbCommand(command, fbCon);
                contractorCommand.CommandType = CommandType.Text;
                dr = contractorCommand.ExecuteReader();
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
        //Разрешить кнопку ВЫБРАТЬ (выбрать контрагента)
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
                    contractorTransaction = fbCon.BeginTransaction();
                    command = "DELETE FROM CONTRACTOR WHERE CONTRACTOR.ID = @current_record;";
                    contractorCommand = new FbCommand(command, fbCon, contractorTransaction);
                    contractorCommand.Parameters.AddWithValue("@current_record", Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                    contractorCommand.CommandType = CommandType.Text;
                    contractorCommand.ExecuteNonQuery();
                    contractorTransaction.Commit();
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
            using (ContractorEdit cef = new ContractorEdit())
            {
                cef.Text = "Изменение контрагента";
                cef.setID(Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value));
                cef.setName(Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value));
                cef.setActualAddress(Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value));
                cef.setLegalAddress(Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value));
                cef.setPhone(Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value));
                cef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cef.DialogResult == DialogResult.OK)
                    {
                        if (cef.getName().Length == 0 || cef.getActualAddress().Length == 0 || cef.getLegalAddress().Length == 0 || cef.getPhone().Length == 0)
                        {
                            cef.setNameColor();
                            cef.setActualAddressColor();
                            cef.setLegalAddressColor();
                            cef.setPhoneColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                contractorTransaction = fbCon.BeginTransaction();
                                command = "UPDATE CONTRACTOR SET CONTRACTOR.NAME = @name," +
                                                                "CONTRACTOR.ACTUAL_ADDRESS = @actual_address," +
                                                                "CONTRACTOR.LEGAL_ADDRESS = @legal_address, " +
                                                                "CONTRACTOR.PHONE = @phone " +
                                                                "WHERE CONTRACTOR.ID = @current_id;";
                                contractorCommand = new FbCommand(command, fbCon, contractorTransaction);
                                contractorCommand.Parameters.AddWithValue("@name", cef.getName());
                                contractorCommand.Parameters.AddWithValue("@actual_address", cef.getActualAddress());
                                contractorCommand.Parameters.AddWithValue("@legal_address", cef.getLegalAddress());
                                contractorCommand.Parameters.AddWithValue("@phone", cef.getPhone());
                                contractorCommand.Parameters.AddWithValue("@current_id", cef.getID());
                                contractorCommand.CommandType = CommandType.Text;
                                contractorCommand.ExecuteNonQuery();
                                contractorTransaction.Commit();
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
            using (ContractorEdit cef = new ContractorEdit())
            {
                cef.Text = "Добавление контрагента";
                cef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cef.DialogResult == DialogResult.OK)
                    {
                        if (cef.getName().Length == 0 || cef.getActualAddress().Length == 0 || cef.getLegalAddress().Length == 0 || cef.getPhone().Length == 0)
                        {
                            cef.setNameColor();
                            cef.setActualAddressColor();
                            cef.setLegalAddressColor();
                            cef.setPhoneColor();
                            fe.Cancel = true;
                        }
                        else
                        {
                            try
                            {
                                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                                fbCon.Open();
                                contractorTransaction = fbCon.BeginTransaction();
                                command = "INSERT INTO CONTRACTOR (NAME, ACTUAL_ADDRESS, LEGAL_ADDRESS, PHONE) VALUES (@name, @actual_address, @legal_address, @phone);";
                                contractorCommand = new FbCommand(command, fbCon, contractorTransaction);
                                contractorCommand.Parameters.AddWithValue("@name", cef.getName());
                                contractorCommand.Parameters.AddWithValue("@actual_address", cef.getActualAddress());
                                contractorCommand.Parameters.AddWithValue("@legal_address", cef.getLegalAddress());
                                contractorCommand.Parameters.AddWithValue("@phone", cef.getPhone());
                                contractorCommand.CommandType = CommandType.Text;
                                contractorCommand.ExecuteNonQuery();
                                contractorTransaction.Commit();
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
        //Поиск ко названию названию контрагента
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT ID AS \"Номер\", NAME AS \"Название\", ACTUAL_ADDRESS AS \"Факт. адрес\", LEGAL_ADDRESS AS \"Юр. адрес\", PHONE AS \"Телефон\" FROM CONTRACTOR WHERE CONTRACTOR.NAME LIKE @name;";
                contractorCommand = new FbCommand(command, fbCon);
                contractorCommand.Parameters.AddWithValue("@name", "%" + textBox1.Text + "%");
                contractorCommand.CommandType = CommandType.Text;
                dr = contractorCommand.ExecuteReader();
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
