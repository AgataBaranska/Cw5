using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;


namespace Cw5.Services
{
   public interface IStudentsDbService
    {
        public StudentsEnrollmentResponse StartEnrollStudent(EnrollStudentRequest request);

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request);

    }
}
