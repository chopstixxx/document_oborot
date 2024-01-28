using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Drawing;


using Npgsql;
using NpgsqlTypes;
using System.Security.Cryptography;

namespace document_oborot
{
    public partial class Manager_form3 : Form
    {
        int individual_id;
        int program_id;
        string conn_string;
        string login;
        int contract_id;
        public Manager_form3(int pr_id, int ind_id, string log, string conn, int cont_id)
        {
            program_id = pr_id;
            individual_id = ind_id;
            conn_string = conn;
            login = log;
            contract_id = cont_id;
            InitializeComponent();
        }

        private void Manager_form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }

        private void create_contract()
        {
            string name = "";
            string surname = "";
            string date_of_birth = "";
            int pass_series = 0;
            int pass_number = 0;
            string place_of_residence = "";
            string email = "";
            string phone_number = "";
            string position = "";
            string place_of_work = "";

            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM individual WHERE id = @id", conn))
                {
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = individual_id;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(reader.GetOrdinal("name"));
                            surname = reader.GetString(reader.GetOrdinal("surname"));
                            DateTime dates = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));
                            date_of_birth = dates.ToString("dd.MM.yyyy");
                            pass_series = reader.GetInt32(reader.GetOrdinal("pass_series"));
                            pass_number = reader.GetInt32(reader.GetOrdinal("pass_number"));
                            place_of_residence = reader.GetString(reader.GetOrdinal("place_of_residence"));
                            email = reader.GetString(reader.GetOrdinal("email"));
                            phone_number = reader.GetString(reader.GetOrdinal("phone_number"));
                            position = reader.GetString(reader.GetOrdinal("position"));
                            place_of_work = reader.GetString(reader.GetOrdinal("place_of_work"));
                        }
                    }
                }
            }


            string course_name = "";


            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM educational_program WHERE id = @id", conn))
                {
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = program_id;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            course_name = reader.GetString(reader.GetOrdinal("name"));

                        }
                    }
                }
            }









            
            Document document = new Document();
            
            Page page = document.Pages.Add();

           
            var contractTitle = new TextFragment("Договор оказания образовательных услуг");
            contractTitle.TextState.Font = FontRepository.FindFont("Arial");
            contractTitle.TextState.FontSize = 16;
            contractTitle.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;
            contractTitle.TextState.FontStyle = FontStyles.Bold;
            page.Paragraphs.Add(contractTitle);

           
            string cur_date = DateTime.Now.ToString("dd.MM.yyyy");
            var date = new TextFragment($"Г. Воронеж {cur_date}");
            date.TextState.Font = FontRepository.FindFont("Times New Roman");
            date.TextState.FontSize = 12;

            date.Position = new Position(100, 700);
            page.Paragraphs.Add(date);

            
            var partiesText = "ООО «Гугу Гага», в лице генерального директора Баркалова Никиты Игоревича, действующего на основании Устава общества, именуемого в дальнейшем Исполнитель, с одной стороны\n" +
                              "И\n" +
                               $"{name} {surname} , {date_of_birth} года рождения, проживающий по адресу: {place_of_residence}, паспорт: серия {pass_series}, номер {pass_number}, почта: {email}, номер телефона: {phone_number}, именуемый в дальнейшем Заказчик, с другой стороны\n" +
                              "заключили настоящий договор о нижеследующем\n";
            var parties = new TextFragment(partiesText);
            parties.TextState.Font = FontRepository.FindFont("Times New Roman");
            parties.TextState.FontSize = 12;
            parties.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Left;
            page.Paragraphs.Add(parties);

           
            var subjectTitle = new TextFragment("\nПредмет\n");
            subjectTitle.TextState.Font = FontRepository.FindFont("Arial");
            subjectTitle.TextState.FontSize = 14;
            subjectTitle.IsInLineParagraph = true;
            subjectTitle.TextState.FontStyle = FontStyles.Bold;
            page.Paragraphs.Add(subjectTitle);

           
            var subjectContent = new TextFragment($"\nВ соответствии с настоящим соглашением Исполнитель в лице ООО «Гугу Гага» обязуется оказать Заказчику , которого зовут {name} {surname}, за оговоренную договором плату, следующие образовательные услуги:\n" +
                                                   $"● обучающий курс '{course_name}';\n" +
                                                   "● курс лекций в режиме online, связанных с заявленной тематикой.\n");
            subjectContent.TextState.Font = FontRepository.FindFont("Times New Roman");
            subjectContent.TextState.FontSize = 12;
            subjectContent.IsInLineParagraph = true;
            page.Paragraphs.Add(subjectContent);

         
            var finalProvisionsTitle = new TextFragment("\nЗаключительные положения\n");
            finalProvisionsTitle.TextState.Font = FontRepository.FindFont("Arial");
            finalProvisionsTitle.TextState.FontSize = 14;
            finalProvisionsTitle.IsInLineParagraph = true;
            finalProvisionsTitle.TextState.FontStyle = FontStyles.Bold;
            page.Paragraphs.Add(finalProvisionsTitle);

            
            var finalProvisionsContent = new TextFragment("\n● Настоящий договор составлен в двух экземплярах. Один экземпляр передается Заказчику, другой передается Исполнителю.\n" +
                                                          "● По всем моментам, которые не оговорены в настоящем соглашении, стороны руководствуются действующим законодательством Российской Федерации.\n");
            finalProvisionsContent.TextState.Font = FontRepository.FindFont("Times New Roman");
            finalProvisionsContent.TextState.FontSize = 12;
            finalProvisionsContent.IsInLineParagraph = true;
            page.Paragraphs.Add(finalProvisionsContent);

           
            var signatureTitles = new TextFragment("\nПодпись директора\n"
                                                  + "\nПодпись плательщика");
            signatureTitles.TextState.Font = FontRepository.FindFont("Arial");
            signatureTitles.TextState.FontSize = 14;
            signatureTitles.IsInLineParagraph = true;
            signatureTitles.TextState.FontStyle = FontStyles.Bold;
            page.Paragraphs.Add(signatureTitles);

       
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF Files|*.pdf";
            saveFileDialog1.Title = "Save PDF File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                document.Save(saveFileDialog1.FileName);
            }
        }


        private void create_receipt()
        {

            string name = "";
            string surname = "";
            string place_of_residence = "";


            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM individual WHERE id = @id", conn))
                {
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = individual_id;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader.GetString(reader.GetOrdinal("name"));
                            surname = reader.GetString(reader.GetOrdinal("surname"));
                            place_of_residence = reader.GetString(reader.GetOrdinal("place_of_residence"));

                        }
                    }
                }
            }


            int sum = 0;


            using (NpgsqlConnection conn = new NpgsqlConnection(conn_string))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM educational_program WHERE id = @id", conn))
                {
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = program_id;

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sum = reader.GetInt32(reader.GetOrdinal("price"));

                        }
                    }
                }
            }


            Document document = new Document();
            
            Page page = document.Pages.Add();

            
            page.PageInfo.Margin.Left = 40;
            page.PageInfo.Margin.Top = 40;
            page.PageInfo.Margin.Right = 40;
            page.PageInfo.Margin.Bottom = 40;

            
            Table table = new Table();
            table.ColumnWidths = "30% 70%";

            
            Row row1 = table.Rows.Add();
            Cell cellQRCode = row1.Cells.Add();
            Cell cellRight = row1.Cells.Add();

            TextFragment textFragment = new TextFragment("\nКвитанция\n\n");
            textFragment.TextState.FontSize = 14;
            cellQRCode.Paragraphs.Add(textFragment);
            cellQRCode.Alignment = Aspose.Pdf.HorizontalAlignment.Center; 
            cellQRCode.VerticalAlignment = VerticalAlignment.Center; 

           
            string qrCodePath = "images\\qr-code.png"; 
            Aspose.Pdf.Image qrCodeImage = new Aspose.Pdf.Image();
            qrCodeImage.File = qrCodePath;
            qrCodeImage.FixHeight = 100;
            qrCodeImage.FixWidth = 100;
            qrCodeImage.HorizontalAlignment = Aspose.Pdf.HorizontalAlignment.Center;
            qrCodeImage.VerticalAlignment = VerticalAlignment.Center;
            cellQRCode.Paragraphs.Add(qrCodeImage);

            cellQRCode.Border = new BorderInfo(BorderSide.All, 0.5f);
           
            Table rightCellTable = new Table();
            rightCellTable.ColumnWidths = "100%"; 

           
            foreach (string lineText in new string[]
            {
    "Воронежский Государственный Университет",
    "ИНН 777777777777 КПП 4444444",
    "БИК 9292929 (ОТДЕЛЕНИЕ СБЕРБАНКА РОССИИ ПО ВОРОНЕЖСКОЙ ОБЛАСТИ)",
    $"Договор: №{contract_id}",
    $"ФИО обучающегося: {name} {surname}",
    "Назначение: Оплата за курсы",
    $"ФИО плательщика: {name} {surname}",
    $"Адрес плательщика: {place_of_residence}",
    "КБК 0000000000000002332",
    "ОКТМО: 267070",
    $"Сумма: {sum}₽",
    "Подпись плательщика _________"
            })
            {
                Row row = rightCellTable.Rows.Add();
                Cell cell = row.Cells.Add();
                TextFragment textFragmentRight = new TextFragment(lineText + "\n");
                textFragmentRight.TextState.FontSize = 10;
                cell.Paragraphs.Add(textFragmentRight);
                cell.Border = new BorderInfo(BorderSide.All, 0.5f); 
            }

          
            cellRight.Paragraphs.Add(rightCellTable);
            cellRight.Border = new BorderInfo(BorderSide.All, 0.5f); 

            
            page.Paragraphs.Add(table);

            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF Files|*.pdf";
            saveFileDialog1.Title = "Save PDF File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                document.Save(saveFileDialog1.FileName);
            }



        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            create_contract();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            create_receipt();
        }

        private void Manager_form3_Load(object sender, EventArgs e)
        {
            label2.Text = login;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Manager_form form = new Manager_form(conn_string, login);
            form.Show();
            this.Hide();
        }
    }
}
