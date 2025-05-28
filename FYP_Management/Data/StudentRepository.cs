//// Data/StudentRepository.cs  (Dapper example)
//using Dapper;
//using FYP_MS.HelperClasses;
//using FYP_MS.Models;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FYP_MS.Data;
//public sealed class StudentRepository : IStudentRepository
//{
//    public Task<IEnumerable<Student>> GetAllAsync()
//        => Config.GetConnection().QueryAsync<Student>("SELECT * FROM Student");

//    public async Task<Student> GetAsync(int id)
//        => (await Config.GetConnection().QueryAsync<Student>(
//               "SELECT * FROM Student WHERE Id=@id", new { id })).FirstOrDefault();

//    // 1) insert the Person row and capture the generated PersonId
//    // 2) call usp_AddStudent with that Id + RegNo
//    public async Task AddAsync(Student p, string regNo)
//    {
//        using var con = Config.GetConnection();
//        await con.OpenAsync();

//        // 1) insert into Person and capture the new Id
//        var id = await con.ExecuteScalarAsync<int>(
//            """
//        INSERT INTO Person (FirstName, LastName, ContactNo, Email, GenderId, Dob)
//        OUTPUT INSERTED.Id
//        VALUES (@FirstName, @LastName, @ContactNo, @Email, @GenderId, @Dob);
//        """, p);

//        // 2) run stored procedure
//        await con.ExecuteAsync(
//            "usp_AddStudent",
//            new { Id = id, Regno = regNo },
//            commandType: CommandType.StoredProcedure);
//    }



//    public Task UpdateAsync(Student s)
//        => Config.GetConnection().ExecuteAsync("""
//            UPDATE Student SET FirstName=@FirstName,LastName=@LastName,
//                ContactNo=@ContactNo,Email=@Email,GenderId=@GenderId,Dob=@Dob
//            WHERE Id=@Id
//           """, s);

//    public Task DeleteAsync(int id)
//        => Config.GetConnection().ExecuteAsync("DELETE FROM Student WHERE Id=@id", new { id });
//}
