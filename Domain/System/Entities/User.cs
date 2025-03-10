using System.Reflection;

namespace Domain.System.Entities
{
    public class User
    {
        public Guid UserId { get; private set; }
        public Guid UseUserNorId { get; private set; }
        public Guid DisplayName { get; private set; }
        public Guid Email { get; private set; }
        public Guid Mobile { get; private set; }
        public Guid Avatar { get; private set; }
    }
}
