using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLockApp.Models
{
    public class Build
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CharacterId { get; set; }
        public long? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
