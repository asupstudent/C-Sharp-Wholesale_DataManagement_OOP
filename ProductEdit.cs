using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wholesale
{
    public partial class ProductEdit : Form
    {
        //Идентификатор товара
        int id = 0;
        //Идентификатор категории
        int id_category = 0;
        public ProductEdit()
        {
            InitializeComponent();
            comboBox1.DataSource = Data.units;
            comboBox1.DisplayMember = "longValue";
        }

        //Получить идентификатор

        public int getID()
        {
            return id;
        }

        //Установить идентификатор

        public void setID(int id)
        {
            this.id = id;
        }

        //Установить количество товара
        public void setAmount(string amount)
        {
            this.textBox5.Text = amount;
        }
        //Получить количество товара
        public string getAmount()
        {
            return this.textBox5.Text;
        }

        //Получить название товара

        public string getName()
        {
            return textBox1.Text;
        }

        //Установить название товара
        public void setName(string name)
        {
            textBox1.Text = name;
        }

        //Получить марку товара

        public string getTrademark()
        {
            return textBox2.Text;
        }

        //Установить марку товара
        public void setTrademark(string trademark)
        {
            textBox2.Text = trademark;
        }

        //Получить категорию товара

        public string getIdCategory()
        {
            return textBox3.Text;
        }
        //Получить идентификатор категории
        public int getIdCategoryValue()
        {
            return id_category;
        }
        //Установить название категории
        public void setIdCategory(string id_category)
        {
            textBox3.Text = id_category;
        }

        //Получить цену товара

        public string getPrice()
        {
            return textBox4.Text;
        }

        //Установить цену товара
        public void setPrice(string price)
        {
            textBox4.Text = price;
        }

        //Получить единицы измерения товара

        public string getUnit()
        {
            Unit unit = (Unit)comboBox1.SelectedItem;
            return unit.shortValue;
        }
        //Установить единицы измерения товара
        public void setUnit(string shortValue)
        {
            List<Unit> list = Data.units;
            var itemUnit = list.Find(item => item.shortValue == shortValue);
            comboBox1.SelectedIndex = itemUnit.index;
        }

        //Получить минимальное количество товара

        public string getMinStock()
        {
            return textBox6.Text;
        }
        //Установить минимальное количество товара
        public void setMinStock(string minStock)
        {
            textBox6.Text = minStock;
        }

        //Получить желаемое количество товара

        public string getWantStock()
        {
            return textBox7.Text;
        }

        //Установить желаемое количество товара

        public void setWantStock(string wantStock)
        {
            textBox7.Text = wantStock;
        }

        //Получить описание товара

        public string getDescription()
        {
            return textBox8.Text;
        }

        //Установить описание товара
        public void setDescription(string description)
        {
            textBox8.Text = description;
        }

        //Ввод только цирф, клавиша BackSpace и запятая

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8 && number != 44) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }
        //Закрыть форму
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Выбрать категорию

        private void button3_Click(object sender, EventArgs e)
        {
            using (Category cf = new Category())
            {
                cf.Text = "Выбор категории";
                cf.setChooseButton();
                cf.FormClosing += delegate (object fSender, FormClosingEventArgs fe)
                {
                    if (cf.DialogResult == DialogResult.OK)
                    {
                        textBox3.Text = cf.getCurrentRecord().Cells[1].Value.ToString();
                        id_category = Convert.ToInt32(cf.getCurrentRecord().Cells[0].Value);
                    }
                };
                cf.ShowDialog(this);
            }
        }

        //Ввод только букв, клавиша BackSpace и пробел
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if ((l < 'А' || l > 'я') && l != '\b' && l != 32)
            {
                e.Handled = true;
            }
        }
        //Ввод только букв, клавиша BackSpace и пробел
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if ((l < 'А' || l > 'я') && l != '\b' && l != 32)
            {
                e.Handled = true;
            }
        }
        //Ввод только цифр и Backspace
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }

        //Ввод только цифр и Backspace
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;

            if (!Char.IsDigit(l) && l != '\b')
            {
                e.Handled = true;
            }
        }
        //Проверка цвет название товара
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
        //Проверка цвет марка
        public void setTrademarkColor()
        {
            if (this.getTrademark().Length == 0)
            {
                textBox2.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox2.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка цвет категория
        public void setIdCategoryColor()
        {
            if (this.getIdCategory().Length == 0)
            {
                textBox3.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox3.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка цвет цена
        public void setPriceColor()
        {
            if (this.getPrice().Length == 0)
            {
                textBox4.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox4.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка цвет минимальный запас
        public void setMinStockColor()
        {
            if (this.getMinStock().Length == 0)
            {
                textBox6.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox6.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка цвет желаемый запас
        public void setWantStockColor()
        {
            if (this.getWantStock().Length == 0)
            {
                textBox7.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox7.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
        //Проверка цвет описание
        public void setDescriptionColor()
        {
            if (this.getDescription().Length == 0)
            {
                textBox8.BackColor = System.Drawing.Color.Salmon;
            }
            else
            {
                textBox8.BackColor = System.Drawing.Color.PaleGreen;
            }
        }
    }
    //comboBox - элемент
    public class Unit
    {
        public string shortValue { get; set; }
        public string longValue { get; set; }
        public int index { get; set; }
    }
    //данные comboBox
    public static class Data
    {
        public static List<Unit> units = new List<Unit>()
        {
            new Unit()
            {
                shortValue = "кг.",
                longValue = "килограмм",
                index = 0
            },
            new Unit()
            {
                shortValue = "шт.",
                longValue = "штуки",
                index = 1
            }
        };
    }
}
