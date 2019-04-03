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

        public int AddImage(Image image)
   
     {
            SqlConnection conn = new SqlConnection(_connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@filename, @password, 0) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@filename", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
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
                TimesViewed = (int)reader["TimesViewed"]
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
    }
}
