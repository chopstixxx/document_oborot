using Npgsql;
using NpgsqlTypes;
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
    public partial class Manger_form2 : Form
    {
        string login;
        string conn_string;
        int ind_id;
        public Manger_form2(int i_id, string con, string log)
        {
            InitializeComponent();
            ind_id = i_id;
            conn_string = con;
            login = log;

        }

        private void Manger_form2_Load(object sender, EventArgs e)
        {
            label2.Text = login;
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM educational_program ORDER BY id ASC", conn_string);

            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название";
            dataGridView1.Columns[2].HeaderText = "Длительность";
            dataGridView1.Columns[3].HeaderText = "Квалификация";
            dataGridView1.Columns[4].HeaderText = "Цена";

            dataGridView1.Rows[0].Selected = false;


        }

        private void Manger_form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выбери образовательную программу!");
                return;
            }
            int program_id = (int)dataGridView1.CurrentRow.Cells[0].Value;

            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO contracts (ed_programm_id, individual_id, contract_date) VALUES (@ed_id, @in_id, @cont_date)  RETURNING id", conn))
                {
                    cmd.Parameters.Add("@ed_id", NpgsqlDbType.Integer).Value = program_id;
                    cmd.Parameters.Add("@in_id", NpgsqlDbType.Integer).Value = ind_id;
                    cmd.Parameters.Add("@cont_date", NpgsqlDbType.Timestamp).Value = DateTime.Now;

                    try
                    {
                        int cont_id = (int)cmd.ExecuteScalar(); // Получаем ID после вставки
                        MessageBox.Show("Договор успешно заключен!");
                        Manager_form3 form = new Manager_form3(program_id, ind_id, login, conn_string, cont_id);
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
    }
}
