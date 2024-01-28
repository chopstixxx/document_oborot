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
    public partial class Manager_form : Form
    {
        string conn_string;
        string login;
        public Manager_form(string conn, string log)
        {
            InitializeComponent();
            conn_string = conn;
            login = log;
        }

        private void Manager_form_Load(object sender, EventArgs e)
        {

            label2.Text = login;
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM individual ORDER BY id ASC", conn_string);

            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Фамилия";
            dataGridView1.Columns[3].HeaderText = "Дата рождения";
            dataGridView1.Columns[4].HeaderText = "Серия паспорта";
            dataGridView1.Columns[5].HeaderText = "Номер паспорта";
            dataGridView1.Columns[6].HeaderText = "Место проживания";
            dataGridView1.Columns[7].HeaderText = "Электронная почта";
            dataGridView1.Columns[8].HeaderText = "Номер телефона";
            dataGridView1.Columns[9].HeaderText = "Должность";
            dataGridView1.Columns[10].HeaderText = "Место работы";
            dataGridView1.Rows[0].Selected = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выбери физическое лицо!");
            }
            else
            {
                int ind_id = (int)dataGridView1.CurrentRow.Cells[0].Value;

                Manger_form2 form2 = new Manger_form2(ind_id, conn_string, login);
                form2.Show();
                this.Hide();


            }
        }

        private void Manager_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Auth_form form = new Auth_form();
            form.Show();
            this.Hide();
        }
    }
}
