using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using System.Collections.ObjectModel;

namespace Cw5.Services
{
   public interface IStudentsDbService
    {
        public StudentsEnrollmentResponse StartEnrollStudent(EnrollStudentRequest request);

        public PromoteStudents(PromoteStudentsRequest request);

    }
}
