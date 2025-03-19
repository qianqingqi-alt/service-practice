using Domain.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Domain.System.Entities
{
    [Table("user")]
    [PrimaryKey(nameof(UserId))]
    public class User : BaseEntityWithCreateUpdate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; private set; }
        public long UserNo { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public string Avatar { get; private set; }

        public User() { }

        public User(long userNo, string displayName, string email, string mobile, string avatar)
        {
            UserNo = userNo;
            DisplayName = displayName;
            Email = email;
            Mobile = mobile;
            Avatar = avatar;
        }
    }
}
