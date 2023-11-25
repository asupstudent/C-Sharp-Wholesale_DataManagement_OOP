using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wholesale
{
    public partial class CategoryEdit : Form
    {
        //Идентификатор для обновления
        int id = 0;
        public CategoryEdit()
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
        //Получить название категории
        public string getName()
        {
            return this.textBox1.Text;
        }
        //Установить ID
        public void setID(int id)
        {
            this.id = id;
        }
        //Установить имя категории
        public void setName(string name)
        {
            textBox1.Text = name;
        }
        //Проверка на пустую строку
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
    }
}
