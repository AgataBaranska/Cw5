using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using System;
using System.Data.SqlClient;

namespace Cw5.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        private const string ConString = "Data Source=DESKTOP-ENIT2G5\\" +
             "SQLEXPRESS;Initial Catalog=S19487;Integrated Security=True";

        public StudentsEnrollmentResponse StartEnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var trans = con.BeginTransaction();
                com.Transaction = trans;

                StudentsEnrollmentResponse response = null;
                try
                {
                    //Czy studia istnieja?
                    com.CommandText = "SELECT IdStudies FROM Studies WHERE Name = @name";
                    com.Parameters.AddWithValue("name", request.Studies);
                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        trans.Rollback();
                        //("Podane studia nie istnieją");
                    }
                    int idStudy = (int)dr["IdStudies"];

                    //Czy index jest unikalny?
                    com.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNummber =@IndexNumber";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        trans.Rollback();
                        //Podany numer indexu jest już używany");
                        return null;
                    }


                    com.CommandText = $"SELECT IdEnrollment FROM Enrollment WHERE IdStudy ={idStudy} AND Semester = 1 ORDER BY StartDate DESC";
                    dr = com.ExecuteReader();
                    int idEnrollment;
                    DateTime startDate = DateTime.Now;
                    if (dr.Read())
                    {
                        idEnrollment = (int)dr["IdEnrollment"];

                    }
                    else
                    {
                        //dodajemy wpis do Enorllment
                        //sprawdzamy maxymalny wpis w IdEnrollment
                        com.CommandText = "SELECT Max(IdEnrollment) as Max FROM Enrollment";
                        dr = com.ExecuteReader();
                        idEnrollment = (int)dr["Max"] + 1;


                        com.CommandText = $"INSERT INTO Enrollment(IdEnrollment,Semester,IdStudy,StartDate) " +
                            $"VAUES (@IdEnrollment,@Semester,@IdStudy,@StartDate)";
                        com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                        com.Parameters.AddWithValue("Semester", "1");
                        com.Parameters.AddWithValue("IdStudy", idStudy);
                        com.Parameters.AddWithValue("StartDate", startDate);

                        var di = com.BeginExecuteNonQuery();

                    }

                    //Dodajemy wpis do Students
                    com.CommandText = $"INSERT INTO Student(IndexNumber, FirstName,LastName,BirthDate, IdEnrollment)" +
                    $" VALUES ( @IndexNumber, @FirstName, @LastName, @Birthdate, @IdEnrollment)";

                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", request.FirstName);
                    com.Parameters.AddWithValue("LastName", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("IdEnrollment", idEnrollment);

                    com.ExecuteNonQuery();

                    response = new StudentsEnrollmentResponse
                    {
                        IndexNumber = request.IndexNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        BirthDate = request.BirthDate,
                        Name = request.Studies,
                        Semester = 1,
                        StartDate = startDate

                    };

                    trans.Commit();

                }
                catch (SqlException e)
                {
                    trans.Rollback();

                }

                return response;
            }

        }

       
        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                con.Open();
                com.CommandText = "Select * FROM Enrollment e JOIN Studies s ON e.IdStudy = s.IdStudy WHERE e.Semester = @Semester AND s.Name = @Studies";
               var dr =  com.ExecuteReader();
                if (!dr.HasRows) return null;



                //Procedura składowana - treść
/*
CREATE PROCEDURE PromoteStudents @Studies NVARCHAR(100), @Semester INT, @NewSemester INT OUTPUT
AS
BEGIN
DECLARE @IdStudy INT = (SELECT s.IdStudy FROM Studies s JOIN Enrollment e ON
e.IdStudy = s.IdStudy WHERE s.IdStudy = @Studies AND e.Semester = @Semester)
IF ( @IdStudy IS NULL)
BEGIN
Raiserror  ('Studia o podanej nazwie i semestrze nie istnieją',1,1);
RETURN;
END

DECLARE @NewIdEnrollment INT = (SELECT IdEnrollment FROM Enrollment e JOIN Studies s ON
                    s.IdStudy = e.IdStudy WHERE e.Semester = (@Semester +1) AND
                    s.IdStudy = @Studies)
IF (@NewIdEnrollment IS NULL)
BEGIN

INSERT INTO Enrollment(IdEnrollment,Semester,IdStudy,StartDate)
VALUES (@@Identity+1,@Semester+1,@IdStudy,CURRENT_TIMESTAMP)
SET @NewIdEnrollment =(SELECT IdEnrollment FROM Enrollment e JOIN Studies s ON
                    s.IdStudy = e.IdStudy WHERE e.Semester = (@Semester +1) AND
                    s.IdStudy = @Studies)
END

UPDATE Student SET IdEnrollment = @NewIdEnrollment WHERE IdEnrollment = (SELECT IdEnrollment FROM Enrollment e JOIN Studies s ON
                    s.IdStudy = e.IdStudy WHERE e.Semester = @Semester AND
                    s.IdStudy = @Studies);

SET @NewSemester = @Semester +1;
RETURN
END;
*/




com.CommandText = "PromoteStudents";
com.CommandType = System.Data.CommandType.StoredProcedure;
com.Parameters.AddWithValue("Studies", request.Studies);
com.Parameters.AddWithValue("Semester", request.Semester);
dr = com.ExecuteReader();


}

return 0;
}


}
}
