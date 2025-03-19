namespace Application.System.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public long? UseUserNorId { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Avatar { get; set; }
    }
}
