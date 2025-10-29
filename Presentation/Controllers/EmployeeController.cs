using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Data;

namespace SampleApi.Presentation.Controllers
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class EmployeesController : ControllerBase
  {
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
      return await _context.Employees.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
      var employee = await _context.Employees.FindAsync(id);

      if (employee == null)
        return NotFound();

      return employee;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
    {
      _context.Employees.Add(employee);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, Employee employee)
    {
      if (employee.Id.Equals(id))
        return BadRequest();

      _context.Entry(employee).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return NoContent();
    }

    // DELETE: api/employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
      var employee = await _context.Employees.FindAsync(id);
      if (employee == null)
        return NotFound();

      _context.Employees.Remove(employee);
      await _context.SaveChangesAsync();

      return NoContent();
    }
  }
}
