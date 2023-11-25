using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class InvoicesEdit : Form
    {

        int id = 0;
        int place = 0;
        int contractor = 0;
        int employee = 0;
        int product = 0;

        decimal price = 0;

        DataTable dt;
        bool showInvoice = false;
        FbConnection fbCon;
        FbCommand invoiceCommand;
        FbDataReader dr;
        string command;
        public InvoicesEdit(bool isAdding = false)
        {
            InitializeComponent();
            if (isAdding)
            {
                dataGridView1.ColumnCount = 5;
                dataGridView1.Columns[0].Name = "ID товара";
                dataGridView1.Columns[1].Name = "Товар";
                dataGridView1.Columns[2].Name = "Количество";
                dataGridView1.Columns[3].Name = "Цена";
                dataGridView1.Columns[4].Name = "Место на складе";
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].Visible = false;
            }
        }
        //Закрыть форму
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Получить место по товару
        private int[] getWarehousePosition(int position)
        {
            int[] result = new int[3];

            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT LINE, SHELF, PLACE " +
                            "FROM WAREHOUSE WHERE " +
                            "WAREHOUSE.ID = @current_id;";
                invoiceCommand = new FbCommand(command, fbCon);
                invoiceCommand.Parameters.AddWithValue("@current_id", position);
                invoiceCommand.CommandType = CommandType.Text;
                dr = invoiceCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                result[0] = Convert.ToInt32(dt.Rows[0][0]);
                result[1] = Convert.ToInt32(dt.Rows[0][1]);
                result[2] = Convert.ToInt32(dt.Rows[0][2]);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                fbCon.Close();
            }

            return result;
        }
        //Удалить событие при клике на dataGridView
        public void unsubscribeMouseDataGridView()
        {
            this.dataGridView1.MouseClick -= new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
        }
        //Получить идентификатор
        public int getID()
        {
            return id;
        }
        //Получить место
        public int getPlace()
        {
            return place;
        }
        //получить контагента
        public int getContractor()
        {
            return contractor;
        }
        //получить сотрудника
        public int getEmployee()
        {
            return employee;
        }
        //получить тип накладной
        public string getTypeInvoice()
        {
            return comboBox1.SelectedValue.ToString();
        }
        //получить продукт
        public int getProduct()
        {
            return product;
        }
        //получить количество
        public int getAmount()
        {
            return Convert.ToInt32(this.textBox6.Text);
        }
        //получить ряд
        public string getRow()
        {
            return label9.Text;
        }
        //получить полку
        public string getShelf()
        {
            return label14.Text;
        }
        //получить место
        public string getSeat()
        {
            return label15.Text;
        }
        //Получить цену по накладной
        public decimal getPrice()
        {
            return Convert.ToDecimal(textBox7.Text);
        }
        //Вычислить финальную сумму в накладной
        private decimal getFinalSum(int row)
        {
            decimal sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[row].Value);
            }
            return sum;
        }
        //Проверка на пустой dataGridView
        public bool checkRowCount()
        {
            if (this.dataGridView1.RowCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //установить идентификатор
        public void setID(int id)
        {
            this.id = id;
        }
        //установить контрагента
        public void setContractor(string contractor)
        {
            textBox1.Text = contractor;
        }
        //установить сотрудника
        public void setEmployee(string employee)
        {
            textBox2.Text = employee;
        }
        //установить тип накладной
        public void setTypeInvoice(string type_invoices)
        {
            comboBox1.SelectedIndex = comboBox1.FindStringExact(type_invoices);
            comboBox1.Enabled = false;
        }
        //установить дату
        public void setDate(string date)
        {
            textBox4.Text = date;
        }
        //установить товар
        public void setProduct(string product)
        {
            textBox5.Text = product;
        }
        //Загрузить табличную часть в накладную
        public void setInvoicesTableData(FbDataReader dr)
        {
            dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }
        //Настроить форму для просмотра накладной
        public void setView()
        {
            this.textBox1.ReadOnly = true;
            this.textBox2.ReadOnly = true;
            this.textBox5.ReadOnly = true;
            this.textBox6.ReadOnly = true;
            this.button1.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button7.Visible = false;
            this.button8.Visible = false;
            this.button9.Visible = false;
            this.label4.Visible = true;
            this.textBox4.Visible = true;
            this.label8.Visible = true;
            this.textBox8.Visible = true;
            this.button2.Text = "ЗАКРЫТЬ";
            this.button2.TabIndex = 0;
            showInvoice = true;
        }
        //Загрузить типы накладной в combobox
        public void loadComboBoxValues()
        {
            try
            {
                fbCon = new FbConnection(ConfigurationManager.AppSettings["ConnectionString"]);
                fbCon.Open();
                command = "SELECT ID, NAME FROM INVOICE_TYPE;";
                invoiceCommand = new FbCommand(command, fbCon);
                invoiceCommand.CommandType = CommandType.Text;
                dr = invoiceCommand.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "NAME";
                comboBox1.ValueMember = "ID";
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
        //При выборе товара в накладной, отобразить его в табличной части
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            int[] position = this.getWarehousePosition(Convert.ToInt32(dataGridView1[3, dataGridView1.CurrentRow.Index].Value));
            this.textBox5.Text = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
            this.textBox6.Text = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
            this.textBox7.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
            this.label9.Text = position[0].ToString();
            this.label14.Text = position[1].ToString();
            this.label15.Text = position[2].ToString();
            this.textBox8.Text = getFinalSum(4).ToString();
        }
        //При загрузке формы
        private void InvoicesEdit_Load(object sender, EventArgs e)
        {
            if (showInvoice)
            {
                int[] position = this.getWarehousePosition(Convert.ToInt32(dataGridView1[3, dataGridView1.CurrentRow.Index].Value));
                dataGridView1.Columns[0].Width = 400;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                this.textBox5.Text = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
                this.textBox6.Text = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
                this.textBox7.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
                this.label9.Text = position[0].ToString();
                this.label14.Text = position[1].ToString();
                this.label15.Text = position[2].ToString();
                this.textBox8.Text = getFinalSum(4).ToString();
            }
        }
        //Выбрать контрагента
        private void button3_Click(object sender, EventArgs e)
        {
            using (Contractor cf = new Contractor())
            {
                cf.Text = "Выбор контрагента";
                cf.setChooseButton();
                cf.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cf.DialogResult == DialogResult.OK)
                    {
                        textBox1.Text = cf.getCurrentRecord().Cells[1].Value.ToString();
                        contractor = Convert.ToInt32(cf.getCurrentRecord().Cells[0].Value);
                    }
                };
                cf.ShowDialog(this);
            }
        }
        //Выбрать сотрудника
        private void button5_Click(object sender, EventArgs e)
        {
            using (Employee ef = new Employee())
            {
                ef.Text = "Выбор сотрудника";
                ef.setChooseButton();
                ef.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (ef.DialogResult == DialogResult.OK)
                    {
                        textBox2.Text = ef.getCurrentRecord().Cells[1].Value.ToString();
                        employee = Convert.ToInt32(ef.getCurrentRecord().Cells[0].Value);
                    }
                };
                ef.ShowDialog(this);
            }
        }
        //Выбрать место на сладе для накладной
        private void button4_Click(object sender, EventArgs e)
        {
            using (Warehouse whf = new Warehouse())
            {
                whf.Text = "Выбор места на складе";
                whf.setChooseButton();
                whf.setAddInvoiceFlag();
                if(getTypeInvoice() == "2")
                {
                    whf.setExpenditureInvoice();
                }
                whf.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (whf.DialogResult == DialogResult.OK)
                    {
                        if (getTypeInvoice() == "2")
                        {
                            place = Convert.ToInt32(whf.getCurrentRecord().Cells[0].Value);
                            product = Convert.ToInt32(whf.getCurrentRecord().Cells[1].Value);
                            textBox5.Text = whf.getCurrentRecord().Cells[3].Value.ToString();
                            price = Convert.ToDecimal(whf.getCurrentRecord().Cells[2].Value);
                            label9.Text = whf.getCurrentRecord().Cells[4].Value.ToString();
                            label14.Text = whf.getCurrentRecord().Cells[5].Value.ToString();
                            label15.Text = whf.getCurrentRecord().Cells[6].Value.ToString();
                        }
                        else
                        {
                            place = Convert.ToInt32(whf.getCurrentRecord().Cells[0].Value);
                            label9.Text = whf.getCurrentRecord().Cells[2].Value.ToString();
                            label14.Text = whf.getCurrentRecord().Cells[3].Value.ToString();
                            label15.Text = whf.getCurrentRecord().Cells[4].Value.ToString();
                        }
                    }
                };
                whf.ShowDialog(this);
            }
        }
        //Выбрать товар для накладной
        private void button8_Click(object sender, EventArgs e)
        {
            using (Product pf = new Product())
            {
                pf.Text = "Выбор товара";
                pf.setChooseButton();
                pf.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (pf.DialogResult == DialogResult.OK)
                    {
                        product = Convert.ToInt32(pf.getCurrentRecord().Cells[0].Value);
                        textBox5.Text = pf.getCurrentRecord().Cells[1].Value.ToString();
                        price = Convert.ToDecimal(pf.getCurrentRecord().Cells[4].Value);
                    }
                };
                pf.ShowDialog(this);
            }
            this.textBox6.Text = "0";
            this.textBox7.Text = "0";
            this.label9.Text = "";
            this.label14.Text = "";
            this.label15.Text = "";
        }
        //Проверка - выделить цветом (контрагент)
        public void setContractorColor()
        {
            if (this.getContractor() == 0)
            {
                textBox1.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка - выделить цветом (сотрудник)
        public void setEmployeeColor()
        {
            if (this.getEmployee() == 0)
            {
                textBox2.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка - выделить цветом (список товаров)
        public void setProductListColor()
        {
            if (this.dataGridView1.RowCount == 0)
            {
                dataGridView1.BackgroundColor = System.Drawing.Color.Salmon;
            }
            else
            {
                dataGridView1.BackgroundColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка - выделить цветом (товар)
        public void setProductColor()
        {
            if (this.getProduct() == 0)
            {
                textBox5.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox5.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка - выделить цветом (количество товара)
        public void setAmountColor()
        {
            if (this.textBox6.Text.Length == 0 || this.textBox6.Text == "0")
            {
                textBox6.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox6.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка - выделить цветом (место на складе)
        public void setPlaceColor()
        {
            if (this.getRow().Length == 0 || this.getShelf().Length == 0 || this.getSeat().Length == 0)
            {
                label9.BackColor = System.Drawing.Color.Salmon;
                label14.BackColor = System.Drawing.Color.Salmon;
                label15.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                label9.BackColor = System.Drawing.Color.PaleGreen;
                label14.BackColor = System.Drawing.Color.PaleGreen;
                label15.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Удалить из накладной товар
        private void button9_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.Remove(dataGridView1.Rows[a]);
            if(dataGridView1.RowCount == 0)
            {
                button9.Enabled = false;
            }
            textBox8.Text = getFinalSum(3).ToString();
        }
        //Поиск по dataGridView по столбцу
        public static int SearchDGV(DataGridView dgv, string SearchValue, string ColName)
        {
            foreach (DataGridViewRow Row in dgv.Rows)
            {
                if (Row.Cells[ColName].Value.ToString().Equals(SearchValue))
                    return Row.Index;
            }
            return -1;
        }
        //Добавить в накладную позицию товара
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.getProduct() == 0 ||
                this.getAmount() == 0 ||
                this.getRow().Length == 0 ||
                this.getShelf().Length == 0 ||
                this.getSeat().Length == 0)
            {
                this.setProductColor();
                this.setPlaceColor();
                this.setAmountColor();
            }
            else
            {
                if(SearchDGV(dataGridView1, place.ToString(), "Место на складе") != -1)
                {
                    MessageBox.Show("Место на складе используется");
                }
                else
                {
                    dataGridView1.Rows.Add(product, this.textBox5.Text, this.getAmount(), this.getPrice().ToString(), place);
                    label9.BackColor = SystemColors.Control;
                    label14.BackColor = SystemColors.Control;
                    label15.BackColor = SystemColors.Control;
                    textBox6.BackColor = SystemColors.Control;
                    textBox5.BackColor = SystemColors.Control;
                    this.textBox5.Text = "";
                    this.textBox6.Text = "0";
                    this.textBox7.Text = "0";
                    this.label9.Text = "";
                    this.label14.Text = "";
                    this.label15.Text = "";
                    button9.Enabled = true;
                    textBox8.Text = getFinalSum(3).ToString();
                }
            }
        }
        //В поле количество товара вводить только цифры
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
        //Вычислить стоимость товаров по накладной
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            decimal price_product = 0;
            Decimal.TryParse(textBox6.Text, out price_product);
            decimal price_products = price_product * price;
            textBox7.Text = price_products.ToString();
        }
        //При активации фокуса очистить поле количество товара
        private void textBox6_Enter(object sender, EventArgs e)
        {
            this.textBox6.Text = "";
        }
        //При потере фокуса если поле пустое установить 0 (Количество товара)
        private void textBox6_Leave(object sender, EventArgs e)
        {
            if(this.textBox6.Text == "")
            {
                this.textBox6.Text = "0";
            }
        }
        //Преобразовать таблицу из dataGridView в DataTable
        public DataTable getDataTableProducts()
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            return dt;
        }
        //Скрыть кнопку выбора товара, выбираем из склада
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(getTypeInvoice() == "2")
            {
                this.button8.Visible = false;
            }
            else
            {
                this.button8.Visible = true;
            }
        }
    }
}
