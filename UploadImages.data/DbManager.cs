using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace UploadImages.data
{
    public class DbManager
    {
        private string _connString;
        public DbManager(string connString)
        {
            _connString = connString;
        }

        public void AddUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            SqlConnection conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, PasswordHash) " +
                                  "VALUES (@name, @email, @hash) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", user.PasswordHash);
            conn.Open();
            user.Id = (int)(decimal)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            
        }

        public User Login(string email, string password)
        {
            User user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }
            if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public User GetByEmail(string email)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User
            {
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"],
                Id = (int)reader["Id"],
                Name = (string)reader["Name"]
            };
        }

        public int AddImage(Image image)
   
     {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@filename, @password, 0, @userId) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@filename", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@userId", image.UserId);
            conn.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return id;
        }

        public Image GetImage(int id)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE id = @id";
            conn.Open();
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Image image = new Image
            {
                FileName = (string)reader["fileName"],
                Id = (int)reader["Id"],
                Password = (string)reader["Password"],
                TimesViewed = (int)reader["TimesViewed"],
                UserId = (int)reader["UserId"]
            };
            conn.Close();
            conn.Dispose();
            return image;
        }

        public void UpdateImage(int id)
        {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Images SET TimesViewed = TimesViewed+1 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public IEnumerable<Image> GetImgaesForUser(int id)
        {
            SqlConnection conn = new SqlConnection(_connString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE userId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            List<Image> images = new List<Image>();
            while (reader.Read())
            {
                images.Add(new Image
                {
                    FileName = (string)reader["fileName"],
                    Id = (int)reader["Id"],
                    Password = (string)reader["Password"],
                    TimesViewed = (int)reader["TimesViewed"],
                    UserId = (int)reader["UserId"]
                });
            }
            conn.Close();
            conn.Dispose();
            return images;

        }
    }
}
