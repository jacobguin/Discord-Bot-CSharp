using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public static class Database
    {
        private static string FilePath = "C:/Users/techn/Documents/Bot.accdb";
        private static string Con = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {FilePath}";

        public static string Read(string Table, string WhereColumn, string KeyInColumn, string Return)
        {
            string Out = null;

            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"SELECT * FROM [{Table}] WHERE [{WhereColumn}] = '" + KeyInColumn + "'"
            };
            OleDbDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                try
                {
                    Out = Reader[Return].ToString();
                }
                catch (Exception)
                {
                    Connection.Close();
                    return null;
                }
            }
            Connection.Close();
            return Out;
        }

        public static int ReadInt(string Table, string WhereColumn, string KeyInColumn, string Return)
        {
            int Out = 0;

            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"SELECT * FROM [{Table}] WHERE [{WhereColumn}] = '" + KeyInColumn + "'"
            };
            OleDbDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                try
                {
                    string n = Reader[Return].ToString();
                    int.TryParse(n, out Out);
                }
                catch (Exception)
                {
                    Connection.Close();
                    return 0;
                }
            }
            Connection.Close();
            return Out;
        }

        public static string Read(string Command, string Return)
        {
            string Out = null;

            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = Command
            };
            OleDbDataReader Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                try
                {
                    Out = Reader[Return].ToString();
                }
                catch (Exception)
                {
                    Connection.Close();
                    return null;
                }
            }
            Connection.Close();
            return Out;
        }

        public static void Write(string Table, string Column, string Input)
        {
            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"INSERT INTO [{Table}] ([{Column}]) VALUES ('{Input}')"
            };
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void Write(string Command)
        {
            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = Command
            };
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void Update(string Table, string EditColumn, string WhereColumn, string KeyInColumn, string New)
        {
            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"UPDATE [{Table}] SET [{EditColumn}] = '{New}' WHERE [{WhereColumn}] = '{KeyInColumn}'"
            };
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void Remove(string Table, string WhereColumn, string KeyInColumn)
        {
            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"DELETE FROM [{Table}] WHERE [{WhereColumn}] = '{KeyInColumn}'"
            };
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void Update(string Table, string EditColumn, string WhereColumn, string KeyInColumn, bool IsChecked)
        {
            OleDbConnection Connection = new OleDbConnection();
            string Checked = null;
            if (IsChecked == true)
            {
                Checked = "1";
            }
            else
            {
                Checked = "0";
            }

            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand Command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = $"UPDATE [{Table}] SET [{EditColumn}] = {Checked} WHERE [{WhereColumn}] = '{KeyInColumn}'"
            };
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void Update(string Command)
        {
            OleDbConnection Connection = new OleDbConnection();
            Connection.ConnectionString = Con;
            Connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = Connection,
                CommandText = Command
            };
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
