using Domain.Backstage.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.EntityFramework
{
    public class BaseEntityWithCreateUpdate : BaseEntity
    {
        public Guid? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        [ForeignKey("CreateBy")]
        public User? CreateByUser { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        [ForeignKey("UpdateBy")]
        public User? UpdateByUser { get; set; }
    }
}
