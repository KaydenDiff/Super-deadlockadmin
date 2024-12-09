using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadLockApp.Models
{
    public class BuildItem
    {
        public long Id { get; set; }
        public long BuildId { get; set; }
        public long ItemId { get; set; }
        public long PartId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
