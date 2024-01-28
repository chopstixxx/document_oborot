using Microsoft.VisualBasic.Logging;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace document_oborot
{
    public partial class Reg_form : Form
    {
        string conn_string;
        public Reg_form(string conn)
        {
            InitializeComponent();
            conn_string = conn;
        }

        private void Registration_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Поле логина пустое!");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Поле пароля пустое!");
                return;

            }
            string pattern = @"^[a-zA-Z]{5}\d{3}[@#%)(.<]+$";
            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                MessageBox.Show("Пароль не соответствует требованиям!");
                return;
            }
            if (check_login(textBox1.Text) == true)
            {
                MessageBox.Show("Логин занят!");
                return;
            }

            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO users (role_id, login, password) VALUES (@r_id, @login, @pass)", conn))
                {
                    cmd.Parameters.Add("@r_id", NpgsqlDbType.Integer).Value = 2;
                    cmd.Parameters.Add("@login", NpgsqlDbType.Text).Value = textBox1.Text;
                    cmd.Parameters.Add("@pass", NpgsqlDbType.Text).Value = textBox2.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Регистрация прошла успешно!");
                        Auth_form form = new Auth_form();
                        form.Show();
                        this.Hide();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }






                }

            }
        }
        public bool check_login(string login)
        {
            using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter($"SELECT * from users WHERE login = @login", conn_string))
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue("@login", login);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}
