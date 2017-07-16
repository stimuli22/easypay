using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace easypaycashier
{
    class DatabaseOperation
    {
        static MySqlConnection conn = new MySqlConnection("Server=192.168.1.102;Database=bank;User=stimuli;Password=stimuli");
        static MySqlCommand command;
        static MySqlDataReader reader;

        public static void Open()
        {
            conn.Open();

        }
        public static void Close()
        {
            conn.Close();
        }

        public static MySqlDataReader ViewAccounts()
        {
            command = new MySqlCommand("Select * from tb_accounts", conn);
            reader = command.ExecuteReader();
            return reader;

        }
        public static MySqlDataReader ViewAccountsWhere(string fname)
        {
            command = new MySqlCommand("Select * from tb_accounts where acc_fullname='"+fname+"'", conn);
            reader = command.ExecuteReader();
            return reader;

        }
        public static MySqlDataReader getcustomID(String fname)
        {
            command = new MySqlCommand("select * from tb_accounts where acc_fullname='" + fname + "'", conn);
            reader = command.ExecuteReader();
            return reader;
        }
        public static MySqlCommand for_pic(String fname)
        {
            command = new MySqlCommand("select tm_pic from tb_thumbmark where tm_customer='" + fname + "'", conn);
          
            return command;
        }
        public static MySqlDataReader for_pic2(String fname)
        {
            command = new MySqlCommand("select tm_pic from tb_thumbmark where tm_customer='" + fname + "'", conn);
            reader = command.ExecuteReader();
            return reader;
        }
     
        //get Balannce
        public static MySqlDataReader getBalance(String customID)
        {
            command = new MySqlCommand("select * from tb_accounbal where customId='" + customID + "'", conn);
            reader = command.ExecuteReader();
            return reader;
        }
//Update Balance
        public static MySqlCommand UpdateBalance(string newBalance,string accId) {
            command = new MySqlCommand("Update tb_accounbal set bal='"+newBalance+"' where customId='"+accId+"'",conn);
            return command;
        }
    }
}
