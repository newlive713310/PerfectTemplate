using Microsoft.AspNetCore.Mvc;
using PerfectTemplate.Application.Interfaces;
using PerfectTemplate.Application.Models.Entities;

namespace PerfectTemplate.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDynamicComparer<User> _comparer;
        public UsersController(
            IDynamicComparer<User> comparer
            )
        {
            _comparer = comparer;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string sortBy, bool ascending)
        {
            var users = new List<User>()
            {
                new User() { Id = Guid.NewGuid(), FirstName = "Steven", LastName = "Gerrard", Created = new DateTime(2012, 12, 12) },
                new User() { Id = Guid.NewGuid(), FirstName = "Wayne", LastName = "Rooney", Created = new DateTime(2011, 11, 11) },
                new User() { Id = Guid.NewGuid(), FirstName = "Frank", LastName = "Lampard", Created = new DateTime(2010, 10, 10) }
            };

            // Поле, по которому нужно сортировать
            // true для сортировки по возрастанию, false для сортировки по убыванию
            users.Sort((x, y) => _comparer.Compare(x, y, sortBy, ascending));

            return Ok(users);
        }
    }
}
