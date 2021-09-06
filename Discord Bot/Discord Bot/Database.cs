namespace Discord_Bot
{
    using System;
    using Npgsql;

    public class Database
    {
        public static string ConectionString = $"SSL Mode=Disable;Persist Security Info=True;Password={Hidden_Info.Database.PW};Username={Hidden_Info.Database.ROLE};Database={Hidden_Info.Database.DB};Host={Hidden_Info.Database.IP}";

        private static NpgsqlConnection RowReader = new NpgsqlConnection(ConectionString);
        private static bool open = false;

        private static void Open()
        {
            if (!open)
            {
                open = true;
                RowReader.Open();
            }
        }

        public static NpgsqlParameter CreateParameter(string name, object value)
        {
            return new NpgsqlParameter(name, value);
        }

        public static void Insert(string table, params NpgsqlParameter[] Parameters)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ConectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    string vals = "";
                    foreach (NpgsqlParameter param in Parameters)
                    {
                        vals += "@" + param.ParameterName + ", ";
                        cmd.Parameters.Add(param);
                    }

                    vals = vals.Remove(vals.Length - 2, 2);
                    cmd.CommandText = $"INSERT INTO {table} ({vals.Replace("@", "")}) VALUES({vals})";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ConectionString);
        }

        [Obsolete("removing")]
        public static NpgsqlCommand ReadRow(string command)
        {
            Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = RowReader;
            cmd.CommandText = command;
            return cmd;
        }

        public static void Update(string table, string condiction_column, string condiction_value, params NpgsqlParameter[] Parameters)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ConectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    string vals = "";
                    foreach (NpgsqlParameter param in Parameters)
                    {
                        vals += param.ParameterName + " = @" + param.ParameterName + ", ";
                        cmd.Parameters.Add(param);
                    }
                    vals = vals.Remove(vals.Length - 2, 2);
                    cmd.CommandText = $"UPDATE {table} SET {vals} WHERE {condiction_column} = '{condiction_value}';";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static T Read<T>(string table, string condiction_column, string condiction_value, string returned_column)
        {
            return Read<T>($"SELECT {returned_column} FROM {table} WHERE {condiction_column} = '{condiction_value}';");
        }

        public static T Read<T>(string command)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(ConectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    try
                    {
                        if (cmd.ExecuteScalar() is DBNull)
                        {
                            con.Close();
                            return (T)("" as object);
                        }
                        else
                        {
#pragma warning disable CS8603 // Dereference of a possibly null reference.
                            T bob = (T)cmd.ExecuteScalar();
                            con.Close();
                            return bob;
#pragma warning restore CS8603 // Dereference of a possibly null reference.
                        }
                    }
                    catch
                    {
                        return (T)("" as object);
                    }
                }
            }
        }

        public static void ExecuteNonQuery(string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }

    /*
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
    }*/
}
