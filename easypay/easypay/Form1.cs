using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
namespace easypay
{
    public partial class Form1 : Form
    {
        Bitmap bitmap1;
        MySqlDataReader reader;
        MySqlCommand cd;
        MySqlDataAdapter da;
      //  List<string>name = 
        public Form1()
        {
            InitializeComponent();
            getsource();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openfile.FileName;
                bitmap1 = new Bitmap(openfile.FileName);

            }
        }
        public void getsource()
        {
            DatabaseOperation.Open();
            reader = DatabaseOperation.ViewAccounts();
            while (reader.Read())
            {
                textBox1.AutoCompleteCustomSource.Add(reader["acc_fullname"] + "");

            }
            DatabaseOperation.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!(pictureBox1.Image==null)){
                String name = "";
                DatabaseOperation.Open();
                reader = DatabaseOperation.getcustomID(textBox2.Text);
                while(reader.Read()){
                name = reader["acc_id"]+"";
                }
                DatabaseOperation.Close();
                MessageBox.Show(name);
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                     
                        MySqlParameter[] pm = new MySqlParameter[2];
                        pm[0] = new MySqlParameter("tm_pic", MySqlDbType.LongBlob, 50);
                        pm[0].Value = img;
                        pm[1] = new MySqlParameter("tm_customer", MySqlDbType.LongBlob, 50);
                        pm[1].Value = name;

                        MySqlCommand cd2;
                        cd2 = new MySqlCommand();
                        cd2.CommandType = CommandType.StoredProcedure;
                        cd2.CommandText = "Add_Data";
                        String connect = "server=192.168.1.102;Database=bank;Username=stimuli;password=stimuli";
                        MySqlConnection con = new MySqlConnection(connect);
                        cd2.Connection = con;
                        cd2.Parameters.AddRange(pm);
                        con.Open();
                        cd2.ExecuteNonQuery();
                        con.Close();
                       
                }
            }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter){
                button2.PerformClick();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DatabaseOperation.Open();
            reader = DatabaseOperation.ViewAccountsWhere(textBox1.Text);
            while(reader.Read()){
            textBox2.Text=reader["acc_fullname"]+"";
                textBox3.Text = reader["acc_gender"]+"";
                textBox4.Text = reader["acc_address"]+"";
                textBox5.Text = reader["AccountNumber"]+"";
            }
            DatabaseOperation.Close();
            //check FingerPrint
            DatabaseOperation.Open();
            reader = DatabaseOperation.getcustomID(textBox1.Text);
            while(reader.Read()){
            tempName = reader["acc_id"]+"";
            }
            DatabaseOperation.Close();

            DatabaseOperation.Open();
            reader = DatabaseOperation.getPic(tempName);
           if(reader.HasRows){
               check = true;
           }
            DatabaseOperation.Close();
            if (check == true)
            {
                pic();
                button1.Enabled = false;
              
                btnSubmit.Enabled = false;
                MessageBox.Show("Customer has Registered thumbmark already","",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else {
               
                button1.Enabled = true;
                btnSubmit.Enabled = true;
                pictureBox1.Image = null;
            }
            check = false;
        }
        public void pic()
        {
            try
            {
                DatabaseOperation.Open();
                cd = DatabaseOperation.getPic2(tempName);
                da = new MySqlDataAdapter(cd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                byte[] img = (byte[])dt.Rows[0][0];
                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);
                da.Dispose();
                DatabaseOperation.Close();

            }
            catch (MySqlException e)
            {
                MessageBox.Show("Please Register your Thumbmark to your Respective Bank.");
            }


        }
        public static string tempName { get; set; }
        public static bool check { get; set; }
        }
        
   
    }

