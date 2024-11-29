using System.Data.SqlClient;

namespace App6.Infra
{
    public class ApplicationDbContext
    {
        public ApplicationDbContext(string strCon)
        {
            SqlConnection = new SqlConnection(strCon);
            SqlConnection.Open();
        }

        public SqlConnection SqlConnection { get; set; }
    }
}
