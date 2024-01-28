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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace document_oborot
{
    public partial class Pass_reset_form : Form
    {
        string login;
        string conn_string;
        public Pass_reset_form(string log, string conn)
        {
            InitializeComponent();
            login = log;
            conn_string = conn;
        }

        private void Pass_reset_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE users SET password = @pass WHERE login = @login", conn))
                {
                    cmd.Parameters.Add("@login", NpgsqlDbType.Text).Value = login;
                    cmd.Parameters.Add("@pass", NpgsqlDbType.Text).Value = textBox2.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Пароль успешно сброшен!");
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
