using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.ComponentModel.Design;

namespace TO_DO_BD
{
    class Program
    {
        SqlDataAdapter dataAdapter;
        DataSet dataSet;
        SqlCommandBuilder builder;
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

            dataSet = new DataSet();
            string com = "Select * FROM ToDo";
            dataAdapter = new SqlDataAdapter(com, con);
            builder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataSet);
            int chooce;
            do
            {
                Console.WriteLine("1)Вывести задачи на сегодня\n2)Вывести задачи на неделю\n3)Выйти");
                chooce = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (chooce)
                {
                    case 1:
                        {
                            foreach (DataTable table in dataSet.Tables)
                            {
                                foreach (DataColumn column in table.Columns)
                                    Console.Write($"{column.ColumnName}\t");
                                Console.WriteLine();
                                foreach (DataRow row in table.Rows)
                                {
                                    var cels = row.ItemArray;
                                    if ((DateTime)cels[3] == DateTime.Today)
                                    {
                                        foreach (object cel in cels)
                                            Console.Write($"{cel}\t");
                                        Console.WriteLine();
                                    }
                                }
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 2:
                        {
                            foreach (DataTable table in dataSet.Tables)
                            {
                                DateTime date = DateTime.Today;
                                foreach (DataColumn column in table.Columns)
                                    Console.Write($"{column.ColumnName}\t");
                                Console.WriteLine();
                                foreach (DataRow row in table.Rows)
                                {
                                    var cels = row.ItemArray;
                                    if ((DateTime)cels[3] >= DateTime.Today && (DateTime)cels[3] <= date.AddDays(6))
                                        foreach (object cel in cels)
                                            Console.Write($"{cel}\t");
                                    Console.WriteLine();
                                }
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 3:
                        break;
                }
            } while (chooce != 3);
        }
        public void AddTask()
        {
            Console.Write("Введите название задачи: ");
            string name = Console.ReadLine();
            Console.Write("Введите описание задачи: ");
            string about = Console.ReadLine();
            Console.Write("Введите дедлайн задачи: ");
            string[] date = Console.ReadLine().Split('.');
            DateTime datePerson = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
            con.Open();
            cmd.CommandText = $"Insert INTO ToDo Values('{name}','{about}','{datePerson}')";
            cmd.Connection= con;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteTask()
        {
            Console.Write("Введите название задачи, которую хотите удалить: ");
            string name = Console.ReadLine();
            con.Open();
            cmd.CommandText = $"DELETE FROM ToDo WHERE TaskName = '{name}'";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void Menu()
        {
            int chooce;
            do
            {
                Console.WriteLine("1) Посмотреть задачи\n2) Добавить задачу\n3) Удалить задачу\n4) Выйти");
                chooce = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();
                switch(chooce)
                {
                    case 1: WatchTask(); break;
                    case 2: AddTask(); break;
                    case 3: DeleteTask(); break;
                    case 4: break;
                }
            } while (chooce != 4);
        }
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.Menu();
        }
    }
}
