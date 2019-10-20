namespace Discord_Bot
{
    using System;
    using System.Data.OleDb;

    public static class Database
    {
        private static readonly string FilePath = $"C:/Users/{Environment.UserName}/Documents/Bot.accdb";
        private static readonly string Con = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {FilePath}";

        public static string Read(string table, string whereColumn, string keyInColumn, string @return)
        {
            string @out = null;

            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM [{table}] WHERE [{whereColumn}] = '" + keyInColumn + "'",
            };
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    @out = reader[@return].ToString();
                }
                catch (Exception)
                {
                    connection.Close();
                    return null;
                }
            }

            connection.Close();
            return @out;
        }

        public static int ReadInt(string table, string whereColumn, string keyInColumn, string @return)
        {
            int @out = 0;

            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"SELECT * FROM [{table}] WHERE [{whereColumn}] = '{keyInColumn}'",
            };
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    string n = reader[@return].ToString();
                    int.TryParse(n, out @out);
                }
                catch (Exception)
                {
                    connection.Close();
                    return 0;
                }
            }

            connection.Close();
            return @out;
        }

        public static string Read(string command1, string @return)
        {
            string @out = null;

            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = command1,
            };
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    @out = reader[@return].ToString();
                }
                catch (Exception)
                {
                    connection.Close();
                    return null;
                }
            }

            connection.Close();
            return @out;
        }

        public static void Write(string table, string column, string input)
        {
            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"INSERT INTO [{table}] ([{column}]) VALUES ('{input}')",
            };
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Write(string command1)
        {
            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = command1,
            };
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Update(string table, string editColumn, string whereColumn, string keyInColumn, string @new)
        {
            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"UPDATE [{table}] SET [{editColumn}] = '{@new}' WHERE [{whereColumn}] = '{keyInColumn}'",
            };
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Remove(string table, string whereColumn, string keyInColumn)
        {
            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"DELETE FROM [{table}] WHERE [{whereColumn}] = '{keyInColumn}'",
            };
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Update(string table, string editColumn, string whereColumn, string keyInColumn, bool isChecked)
        {
            OleDbConnection connection = new OleDbConnection();
            string @checked = isChecked == true ? "1" : "0";

            connection.ConnectionString = Con;
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = $"UPDATE [{table}] SET [{editColumn}] = {@checked} WHERE [{whereColumn}] = '{keyInColumn}'",
            };
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void Update(string command1)
        {
            OleDbConnection connection = new OleDbConnection
            {
                ConnectionString = Con,
            };
            connection.OpenAsync();
            OleDbCommand command = new OleDbCommand
            {
                Connection = connection,
                CommandText = command1,
            };
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
