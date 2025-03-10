using Domain.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.System.Entities
{
    [Table("Config")]
    [PrimaryKey(nameof(ConfigKey))]
    public class Config : BaseEntityWithCreateUpdate
    {
        public string ConfigKey { get; private set; }
        public string? ConfigValue { get; private set; }
        public string? Desc { get; private set; }
        public bool IsPublic { get; private set; }

        private Config() { }

        public void UpdateConfigValue(string? configValue)
        {
            ConfigValue = configValue?.Trim();
        }
    }
}
