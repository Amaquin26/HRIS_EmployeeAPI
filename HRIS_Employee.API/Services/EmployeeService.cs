using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.Repositories.Employee;
using HRIS_Employee.API.Services.UnitOfWork;
using HRIS_Employee.Infrastructure.Constants;
using HRIS_Employee.Infrastructure.Exceptions;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : IEmployeeService
    {
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await employeeRepository.GetAllAsync(null, query => query.Include(e => e.EmployeeStatus));
            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EntraObjectId = employee.EntraObjectId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    ContactNumber = employee.ContactNumber,
                    EmployeeStatusId = employee.EmployeeStatusId,
                    EmployeeStatusName = employee.EmployeeStatus.Name
                });
            }

            return employeeDtos;
        }

        public async Task<PaginatedItemsDto<EmployeeDto>> GetAllEmployeesPaginated(PaginationQueryDto paginationQuery)
        {
            var result = await employeeRepository.GetAllPaginatedAsync(paginationQuery.PageSize, paginationQuery.PageNumber, paginationQuery.SearchTerm);

            var employees = result.Item1;
            var totalRecords = result.Item2;

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EntraObjectId = employee.EntraObjectId,
                    EmployeeNumber = employee.EmployeeNumber,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    ContactNumber = employee.ContactNumber,
                    EmployeeStatusId = employee.EmployeeStatusId,
                    EmployeeStatusName = employee.EmployeeStatus.Name,
                    HiredDate = employee.HiredDate
                });
            }

            var paginatedItems = new PaginatedItemsDto<EmployeeDto>
            {
                Items = employeeDtos,
                TotalRecords = totalRecords
            };

            return paginatedItems;
        }

        public async Task<EmployeeDto?> GetEmployeeByEntraObjectIdAsync(string entraObjectId)
        {
            var employee = await employeeRepository.GetSingleByEntraObjectIdAsync(entraObjectId);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name,
                HiredDate = employee.HiredDate
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await employeeRepository.GetSingleAsync(x => x.Id == id, query => query.Include(x => x.EmployeeStatus));

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                EmployeeNumber = employee.EmployeeNumber,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name,
                HiredDate = employee.HiredDate
            };
        }

        public async Task<Employee> AddEmployeeRecord(WriteEmployeeDto employeeDto)
        {
            try
            {
                var existingEmployee = await employeeRepository.GetSingleByEntraObjectIdAsync(employeeDto.EntraObjectId);

                if (existingEmployee != null)
                    throw new ConflictException($"Employee with Entra Object ID: {employeeDto.EntraObjectId} already exists.");

                var employeeNumber = await GenerateEmployeeNumber();

                var employee = new Employee
                {
                    EntraObjectId = employeeDto.EntraObjectId,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    ContactNumber = employeeDto.ContactNumber,
                    EmployeeStatusId = employeeDto.EmployeeStatusId,
                    HiredDate = employeeDto.HiredDate,
                    EmployeeNumber = employeeNumber,
                    Schedule = new Infrastructure.Persistence.Models.Schedule(),
                    TimeZone = TimeZoneInfo.Utc.Id // TODO: Get actual timezone from request user profile
                };

                await employeeRepository.AddAsync(employee);
                await unitOfWork.SaveChangesAsync();

                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> GenerateEmployeeNumber()
        {
            var lastEmployeeNumber = await employeeRepository.GetLastEmployeeNumber();

            int next = 1;
            if (!string.IsNullOrEmpty(lastEmployeeNumber))
            {
                var numericPart = lastEmployeeNumber.Split('-')[1];
                next = int.Parse(numericPart) + 1;
            }

            return $"{EmployeeConstants.EmployeeNumberPrefix}-{next:D4}";
        }
    }
}
