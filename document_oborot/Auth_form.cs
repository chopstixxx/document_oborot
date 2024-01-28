using Microsoft.VisualBasic.Logging;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace document_oborot
{
    public partial class Auth_form : Form
    {
        readonly string conn_string = "Server=localhost;Port=5432;Database=docs_db;User Id=postgres;Password=123;";
        int auth_count;
        public Auth_form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string login = textBox1.Text;
            string password = textBox2.Text;
            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле логина пустое!");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Поле пароля пустое!");
                return;
            }






            using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter($"SELECT * from users WHERE login = @login", conn_string))
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue("@login", login);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (check_password(password) == true)
                    {


                        Check_role(login, password);
                        auth_count = 0;
                    }
                    else
                    {
                        if (auth_count >= 2)
                        {
                            DialogResult result = MessageBox.Show("Пароль был введён неверно 3 раза. Вы хотите его сбросить?", " Сброс пароля", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result == DialogResult.Yes)
                            {
                                Pass_reset_form form = new Pass_reset_form(login, conn_string);
                                form.Show();
                                this.Hide();
                                return;
                            }
                            if (result == DialogResult.No)
                            {
                                return;
                            }
                        }
                        MessageBox.Show("Неправильный пароль!");
                        auth_count++;
                    }

                }
                else
                {
                    MessageBox.Show("Пользователя не существует!");
                }




            }

        }
        private bool check_password(string password)
        {
            using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter($"SELECT * from users WHERE password = @pass", conn_string))
            {
                dataAdapter.SelectCommand.Parameters.AddWithValue("@pass", password);
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
        private void Check_role(string login, string password)
        {


            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT role_id FROM users WHERE login = @login and password = @pass", conn))
                {
                    cmd.Parameters.Add("@login", NpgsqlDbType.Text).Value = login;
                    cmd.Parameters.Add("@pass", NpgsqlDbType.Text).Value = password;
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int role_id = reader.GetInt32(reader.GetOrdinal("role_id"));
                            switch (role_id)
                            {
                                case 1:
                                    Admin_form form = new Admin_form(conn_string, login);
                                    form.Show();
                                    this.Hide();
                                    break;

                                case 2:
                                    Manager_form form2 = new Manager_form(conn_string, login);
                                    form2.Show();
                                    this.Hide();
                                    break;
                            }
                        }
                    }

                }

            }


        }

        private void label5_Click(object sender, EventArgs e)
        {
            Reg_form form = new Reg_form(conn_string);
            form.Show();
            this.Hide();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void Auth_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}