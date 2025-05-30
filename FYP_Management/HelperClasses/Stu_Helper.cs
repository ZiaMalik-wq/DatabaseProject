using Microsoft.Data.SqlClient;
using System.Data;


namespace FYP_Management.HelperClasses
{
    public static class Stu_Helper
    {
        public static void AddStudent(int id, string reg)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("usp_AddStudent", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Regno", reg);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static void DeleteStudent(int id)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("usp_DeleteStudent", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public static DataTable GetStudentTable()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("Select * from student", con);
            var dt = new DataTable();
            con.Open();
            using var sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return dt;
        }

        public static DataTable GetStudentTableDetails()
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("usp_GetStudentDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static void UpdateStudent(string regNo, int PID)
        {
            string Text = "UPDATE Student SET RegistrationNo = @Regno_ Where id = @id_";
            using var con = Config.GetConnection();
            con.Open();
            using var cmd = new SqlCommand(Text, con);
            cmd.Parameters.AddWithValue("Regno_", regNo);
            cmd.Parameters.AddWithValue("id_", PID);
            using var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
        }

        public static DataTable Search(string str = "")
        {
            using var con = Config.GetConnection();
            con.Open();
            using var dt = new DataTable("Person");
            using var cmd = new SqlCommand("Select p.id,p.FirstName,p.LastName,s.RegistrationNo,p.Contact,p.Email,p.DateOfBirth,l.Value as Gender " +
                "from student as s " +
                "join person as p on s.id = p.id " +
                "join lookup as l on p.gender = l.id " +
                "where FirstName + LastName + RegistrationNo + Email + l.value + contact like @str ", con);
            cmd.Parameters.AddWithValue("str", string.Format("%{0}%", str));
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }
    }
}
