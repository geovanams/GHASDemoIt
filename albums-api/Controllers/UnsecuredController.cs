using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
namespace UnsecureApp.Controllers
{
    public class MyController
    {

        public string ReadFile(string userInput)
        {
            // Define a base directory for safe file access
            string safeBaseDir = Path.GetFullPath("SafeFiles");
            // Combine and resolve the user input path
            string combinedPath = Path.Combine(safeBaseDir, userInput);
            string fullPath = Path.GetFullPath(combinedPath);
            // Validate that the resolved path is within the safe base directory
            if (!fullPath.StartsWith(safeBaseDir + Path.DirectorySeparatorChar))
            {
                return null;
            }
            using (FileStream fs = File.Open(fullPath, FileMode.Open))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    return temp.GetString(b);
                }
            }

            return null;
        }

        public int GetProduct(string productName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand()
                {
                    CommandText = "SELECT ProductId FROM Products WHERE ProductName = @ProductName",
                    CommandType = CommandType.Text,
                    Connection = connection
                };
                sqlCommand.Parameters.Add(new SqlParameter("@ProductName", productName));
                connection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
                return -1; // Or throw exception, as appropriate
            }
        }

        public void GetObject()
        {
            try
            {
                object o = null;
                o.ToString();
            }
            catch (Exception e)
            {
                this.Response.Write(e.ToString());
            }
        
        }

        private string connectionString = "";
    }
}
