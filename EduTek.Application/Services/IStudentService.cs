using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
   public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student> CreateAsync(Student student);

        Task<bool> UpdateAsync(int id, Student student);

        Task<bool> DeleteAsync(int id);
    }
}
