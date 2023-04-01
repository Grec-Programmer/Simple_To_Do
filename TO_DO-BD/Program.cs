using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TO_DO_BD
{
    class Program
    {
        
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;
        string cs = ConfigurationManager.ConnectionStrings["ToDo"].ConnectionString;
        public Program()
        {
            con = new SqlConnection(cs);
        }
        public void WatchTask()
        {
            try
            {
                int step = 0;
                con.Open();
                string com = "Select * FROM ToDo";
                cmd = new SqlCommand(com, con);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    if (line == 0)
                    {
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            Console.Write($"\t{reader.GetName(i)}\t|\t");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    Console.Write($"\t{reader[1]} \t {reader[2]}\n");
                    Console.SetCursorPosition(60, step += 2);
                    Console.Write(reader[3]);
                }
            }
            catch
            {
                Console.WriteLine("Что-то пошло не так");
            }
            finally { con.Close(); }
        }
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.WatchTask();
            Console.WriteLine();
        }
    }
}
