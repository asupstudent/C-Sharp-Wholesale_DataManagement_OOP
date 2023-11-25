using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wholesale
{
    public partial class EmployeeEdit : Form
    {
        //Идентификатор для обновления
        int id = 0;
        public EmployeeEdit()
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
        //Получить ФИО
        public string getFIO()
        {
            return this.textBox1.Text;
        }
        //Получить должность
        public string getPost()
        {
            return this.textBox2.Text;
        }
        //Установить ID
        public void setID(int id)
        {
            this.id = id;
        }
        //Установить ФИО
        public void setFIO(string fio)
        {
            textBox1.Text = fio;
        }
        //Установить должность
        public void setPost(string post)
        {
            textBox2.Text = post;
        }
        //Проверка на пустую строку (ФИО)
        public void setFIOColor()
        {
            if (this.getFIO().Length == 0)
            {
                textBox1.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox1.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка на пустую строку (должность)
        public void setPostColor()
        {
            if (this.getPost().Length == 0)
            {
                textBox2.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
    }
}
