using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_FR
{
    internal class connect
    {

        internal class Connect
        {

            private readonly string _host = "localhost";
            private readonly string _db = "application";
            private readonly string _user = "root";
            private readonly string _password = "";
            private readonly string _connectString;
            private readonly MySqlConnection _connection;


            public Connect()
            {

                _connectString = $"SERVER={_host};DATABASE={_db};UID={_user};PASSWORD={_password};";
                _connection = new MySqlConnection(_connectString);

            }


            public MySqlConnection GetConnection() => _connection;


            public void OpenConnection()
            {

                if (_connection.State == System.Data.ConnectionState.Closed)
                    _connection.Open();

            }

            public void CloseConnection()
            {

                if (_connection.State == System.Data.ConnectionState.Open)
                    _connection.Close();

            }
        }

    }
}
