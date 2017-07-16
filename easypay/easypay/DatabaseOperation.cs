using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace easypay
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
            command = new MySqlCommand("Select * from tb_accounts",conn);
            reader = command.ExecuteReader();
           return reader;
            
        }
        public static MySqlDataReader ViewAccountsWhere(string name)
        {
            command = new MySqlCommand("Select * from tb_accounts where acc_fullname='"+name+"'", conn);
            reader = command.ExecuteReader();
            return reader;

        }

        public static MySqlDataReader getcustomID(String fname) {
            command = new MySqlCommand("select * from tb_accounts where acc_fullname='"+fname+"'",conn);
            reader = command.ExecuteReader();
            return reader;
        }

        public static MySqlDataReader getPic(String customerID)
        {
            command = new MySqlCommand("select tm_pic from tb_thumbmark where tm_customer='" + customerID + "'", conn);
            reader = command.ExecuteReader();
            return reader;
        }
        public static MySqlCommand getPic2(String customerID)
        {
            command = new MySqlCommand("select tm_pic from tb_thumbmark where tm_customer='" + customerID + "'", conn);
          
            return command;
        }
    
    }

}
