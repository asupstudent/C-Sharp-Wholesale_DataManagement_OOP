using System;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wholesale
{
    public partial class WarehouseEdit : Form
    {
        //Идентификтор места
        int id = 0;
        public WarehouseEdit()
        {
            InitializeComponent();
        }
        //Закрыть форму
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Получить идентификатор
        public int getID()
        {
            return id;
        }
        //Получить ряд на складе
        public int getLine()
        {
            int result;
            if(Int32.TryParse(textBox1.Text, out result))
            {
                return result;
            }
            return 0;
        }
        //Получить полку на складе
        public int getShelf()
        {
            int result;
            if (Int32.TryParse(textBox2.Text, out result))
            {
                return result;
            }
            return 0;
        }
        //Получить место на складе
        public int getPlace()
        {
            int result;
            if (Int32.TryParse(textBox3.Text, out result))
            {
                return result;
            }
            return 0;
        }
        //Установить идентификатор
        public void setID(int id)
        {
            this.id = id;
        }
        //Установить ряд
        public void setLine(string line)
        {
            textBox1.Text = line;
        }
        //Установить полку
        public void setShelf(string shelf)
        {
            textBox2.Text = shelf;
        }
        //Установить место
        public void setPlace(string place)
        {
            textBox3.Text = place;
        }
        //Только цифры и backspace
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
        //Только цифры и backspace
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
        //Только цифры и backspace
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
        //Проверка ряда - цвет
        public void setLineColor()
        {
            if (this.getLine() == 0)
            {
                textBox1.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка полки - цвет
        public void setShelfColor()
        {
            if (this.getShelf() == 0)
            {
                textBox2.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка места - цвет
        public void setPlaceColor()
        {
            if (this.getPlace() == 0)
            {
                textBox3.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox3.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
    }
}
