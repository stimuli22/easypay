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
using System.Drawing.Imaging;

namespace easypaycashier
{
    public partial class Form1 : Form
    {
        Bitmap bitmap1=null;
        Bitmap bitmap2=null;
        MySqlCommand cd;
        MySqlDataAdapter da;
        MySqlDataReader dr;
        MemoryStream ms;
       
        public Form1()
        {
            InitializeComponent();
            getsource();
        }

        public void pic()
        {
            try {
                DatabaseOperation.Open();
                cd = DatabaseOperation.for_pic(tname);
                da = new MySqlDataAdapter(cd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                byte[] img = (byte[])dt.Rows[0][0];
                MemoryStream ms = new MemoryStream(img);
                pictureBox1.Image = Image.FromStream(ms);
                da.Dispose();
                DatabaseOperation.Close();
            
            }catch(MySqlException e){
                MessageBox.Show("Please Register your Thumbmark to your Respective Bank.");
            }
           

        }

        public void getsource()
        {
            DatabaseOperation.Open();
            dr = DatabaseOperation.ViewAccounts();
            while (dr.Read())
            {
                textBox1.AutoCompleteCustomSource.Add(dr["acc_fullname"] + "");

            }
            DatabaseOperation.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    
        private void btnvalidate_Click(object sender, EventArgs e)
        {
          if(pictureBox1.Image==null){
              MessageBox.Show("null");
          }
            String path = Directory.GetCurrentDirectory();
            bitmap1 = (Bitmap)pictureBox1.Image;
         
          //  pictureBox1.Image = Image.FromFile(path);
            bool compare = ImageCompareString(bitmap1, bitmap2);
            if (compare == true)
            {
                MessageBox.Show("Match","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                button3.Visible = true;
            }
            else
            {
                MessageBox.Show("Not match","", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ImageCompareString(Bitmap bitmap11, Bitmap bitmap12)
        {
            //	throw new NotImplementedException();
            MemoryStream ms = new MemoryStream();
            bitmap11.Save(ms, ImageFormat.Png);
            string firstbitmap = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;
            bitmap12.Save(ms, ImageFormat.Png);
            string secondbitmap = Convert.ToBase64String(ms.ToArray());
            if (firstbitmap.Equals(secondbitmap))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool check { get;set; }
        private void button2_Click(object sender, EventArgs e)
        {
            DatabaseOperation.Open();
            dr = DatabaseOperation.getcustomID(textBox1.Text);
            while(dr.Read()){
                tname = dr["acc_id"]+"";
            }
            DatabaseOperation.Close();
         //   checking 
            DatabaseOperation.Open();
            dr = DatabaseOperation.for_pic2(tname);
            if(dr.HasRows){
                check = true;
            }
            DatabaseOperation.Close();

            if (check == true)
            {
                pic();
                DatabaseOperation.Open();
                dr = DatabaseOperation.ViewAccountsWhere(textBox1.Text);
                while (dr.Read())
                {
                    txtfullname.Text = dr["acc_fullname"] + "";
                    txtaccount.Text = dr["AccountNumber"] + "";
                }
                DatabaseOperation.Close();

            }
            else {
                MessageBox.Show("Please Register your thumb to UnionBank","",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            check = false;
        }
        public static string tname { get; set; }

        private void btnattach_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = openfile.FileName;
                bitmap2 = new Bitmap(openfile.FileName);

            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter){
                button2.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DatabaseOperation.Open();
            dr = DatabaseOperation.getBalance(tname);
            while(dr.Read()){
            balance = dr["bal"]+"";
            }
            DatabaseOperation.Close();
            lBalance.Text = balance;
            double bal = 0;
            double bal2 = 0;
            bool b = Double.TryParse(txtpayment.Text,out bal);
            bool a = Double.TryParse(lBalance.Text,out bal2);
            double change = 0;
            if (!(bal2 < bal))
            {
                 change = (bal2 - bal);
                 tfCange.Text = change + "";
                 DatabaseOperation.Open();
                 cd = DatabaseOperation.UpdateBalance(tfCange.Text, tname);
                 cd.ExecuteNonQuery();
                 DatabaseOperation.Close();
                 MessageBox.Show("Transaction Completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show("Insufficient Balance","",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           
           
        }
        //balance
        public static string balance { get; set; }
    }
}
