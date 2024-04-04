namespace PerfectTemplate.Application.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
