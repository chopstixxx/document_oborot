using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace document_oborot
{
    public partial class Admin_form : Form
    {
        string conn_string;
        string login;
        public Admin_form(string con, string log)
        {

            conn_string = con;
            login = log;
            InitializeComponent();
        }

        private void Admin_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int price;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Поле с названием пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Поле с длительностью пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Поле с квалификацией пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Поле с ценой пустое!");
                return;
            }

            try
            {
                price = Convert.ToInt32(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("Неправильный формат цены!");
                return;
            }

            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO educational_program (name, duration, qualification, price) VALUES ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}')", conn))
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Программа успешно добавлена!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Поле с именем пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Поле с фамилией пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("Поле с датой рождения пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox8.Text))
            {
                MessageBox.Show("Поле с серией паспорта пустое!");
                return;
            }
            try
            {
                int series = Convert.ToInt32(textBox8.Text);
            }
            catch
            {
                MessageBox.Show("Неправильный формат серии паспорта!");
                return;
            }
            if (string.IsNullOrEmpty(textBox9.Text))
            {
                MessageBox.Show("Поле с номером паспорта пустое!");
                return;
            }
            try
            {
                int numb = Convert.ToInt32(textBox9.Text);
            }
            catch
            {
                MessageBox.Show("Неправильный формат номера паспорта!");
                return;
            }
            if (string.IsNullOrEmpty(textBox10.Text))
            {
                MessageBox.Show("Поле с местом жительства пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox11.Text))
            {
                MessageBox.Show("Поле с электронной почтой пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox12.Text))
            {
                MessageBox.Show("Поле с номером телефона пустое!");
                return;
            }

            if (string.IsNullOrEmpty(textBox13.Text))
            {
                MessageBox.Show("Поле с должностью пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox14.Text))
            {
                MessageBox.Show("Поле с местом работы пустое!");
                return;

            }

            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO individual (name, surname, date_of_birth, pass_series, pass_number, place_of_residence, email, phone_number, position, place_of_work) VALUES ('{textBox5.Text}', '{textBox6.Text}', '{textBox7.Text}', '{textBox8.Text}','{textBox9.Text}', '{textBox10.Text}', '{textBox11.Text}', '{textBox12.Text}', '{textBox13.Text}', '{textBox14.Text}')", conn))
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Физическое лицо успешно добавлено!");
                }
            }

        }

        private void Admin_form_Load(object sender, EventArgs e)
        {
            label16.Text = login;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Auth_form form = new Auth_form();
            form.Show();
            this.Hide();
        }
    }
}
