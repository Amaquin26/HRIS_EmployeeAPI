using HRIS_Employee.API.DTOs;
using HRIS_Employee.API.Repositories;
using HRIS_Employee.Infrastructure.Constants;
using HRIS_Employee.Infrastructure.Exceptions;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS_Employee.API.Services
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await employeeRepository.GetAllAsync();
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

        public async Task<EmployeeDto?> GetEmployeeByEntraObjectIdAsync(string entraObjectId)
        {
            var employee = await employeeRepository.GetSingleByEntraObjectIdAsync(entraObjectId);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name
            };
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await employeeRepository.GetSingleByIdAsync(id);

            if (employee == null)
                return null;

            return new EmployeeDto
            {
                Id = employee.Id,
                EntraObjectId = employee.EntraObjectId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                ContactNumber = employee.ContactNumber,
                EmployeeStatusId = employee.EmployeeStatusId,
                EmployeeStatusName = employee.EmployeeStatus.Name
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
                    HireDate = employeeDto.HireDate,
                    EmployeeNumber = employeeNumber
                };

                var result = await employeeRepository.AddEmployee(employee);

                if (!result)
                {
                    throw new ConflictException("Failed to add the new employee record.");
                }

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
