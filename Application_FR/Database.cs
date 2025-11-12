using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Application_FR.connect;

namespace Application_FR
{
    internal class Database
    {

        private readonly Connect conn = new Connect();

        public bool RegisterUser(string username, string passvord, string version = "1.0")
        {
            try
            {
                conn.OpenConnection();

                var check = new MySqlCommand("SELECT COUNT(*) FROM datas WHERE UserName=@u", conn.GetConnection());
                check.Parameters.AddWithValue("@u", username);
                if ((long)check.ExecuteScalar() > 0)
                    return false;

                string salt = GenerateSalt(16);
                string hash = HashPassword(passvord, salt);

                var insert = new MySqlCommand(@"INSERT INTO datas (version, UserName, Password, Salt, RegTime, ModTime)
                                                VALUES (@v, @u, @p, @s, NOW(), NOW())", conn.GetConnection());
                insert.Parameters.AddWithValue("@v", version);
                insert.Parameters.AddWithValue("@u", username);
                insert.Parameters.AddWithValue("@p", hash);
                insert.Parameters.AddWithValue("@s", salt);

                return insert.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        private string GenerateSalt(int length)
        {
            var random = new SecureRandom();
            var salt = new byte[length];
            random.NextBytes(salt);
            return Convert.ToBase64String(salt);
        }

        private string HashPassword(string password, string salt)
        {
            var digest = new Sha256Digest();
            var input = Encoding.UTF8.GetBytes(password + salt);
            digest.BlockUpdate(input, 0, input.Length);
            var result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;database=application;uid=root;pwd=;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM datas", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            return dt;
        }
    }
}
