using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;

using System.Collections.Generic;


namespace Cw5.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Student GetStudent(string index);

        public int AddStudent(Student studentToAdd);

        public int RemoveStudent(string index);

        public int UpdateStudent(Student studentToUpdate);

        public StudentsEnrollmentResponse GetStudentsEnrollment(string index);

     




    }
}
