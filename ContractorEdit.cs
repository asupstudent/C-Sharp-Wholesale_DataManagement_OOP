using System;
using System.Windows.Forms;

namespace Wholesale
{
    public partial class ContractorEdit : Form
    {
        //Идентификатор для обновления
        int id = 0;
        public ContractorEdit()
        {
            InitializeComponent();
        }
        //Закрыть форму
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Получить ID
        public int getID()
        {
            return id;
        }
        //Получить название контрагента
        public string getName()
        {
            return this.textBox1.Text;
        }
        //Получить фактический адрес контрагента
        public string getActualAddress()
        {
            return this.textBox2.Text;
        }
        //Получить юридический адрес контрагента
        public string getLegalAddress()
        {
            return this.textBox3.Text;
        }
        //Получить телефон контрагента
        public string getPhone()
        {
            return this.textBox4.Text;
        }
        //Установить ID
        public void setID(int id)
        {
            this.id = id;
        }
        //Установить имя контрагента
        public void setName(string name)
        {
            textBox1.Text = name;
        }
        //Установить фактический адрес
        public void setActualAddress(string actual_address)
        {
            textBox2.Text = actual_address;
        }
        //Установить юридический адрес
        public void setLegalAddress(string legal_address)
        {
            textBox3.Text = legal_address;
        }
        //Установить телефон
        public void setPhone(string phone)
        {
            textBox4.Text = phone;
        }
        //Проверка на пустую строку (название контрагента)
        public void setNameColor()
        {
            if (this.getName().Length == 0)
            {
                textBox1.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка на пустую строку (фактический адрес)
        public void setActualAddressColor()
        {
            if (this.getActualAddress().Length == 0)
            {
                textBox2.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка на пустую строку (юридический адрес)
        public void setLegalAddressColor()
        {
            if (this.getLegalAddress().Length == 0)
            {
                textBox3.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox3.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка на пустую строку (название телефон)
        public void setPhoneColor()
        {
            if (this.getPhone().Length == 0)
            {
                textBox4.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox4.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
    }
}
