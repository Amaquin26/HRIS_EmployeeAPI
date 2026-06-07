using HRIS_Employee.API.Repositories.Base;
using HRIS_Employee.Infrastructure.Persistence.DBContext;
using HRIS_Employee.Infrastructure.Persistence.Models;

namespace HRIS_Employee.API.Repositories
{
    public class ShiftRecordRepository(EmployeeDbContext context) : BaseRepository<ShiftRecord>(context), IShiftRecordRepository
    {
    }
}
