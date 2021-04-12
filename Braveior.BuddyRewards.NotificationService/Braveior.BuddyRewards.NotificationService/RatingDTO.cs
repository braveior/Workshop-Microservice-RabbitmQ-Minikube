using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.NotificationService
{
    public class RatingDTO
    {
        public string Comment { get; set; }

        public int Star { get; set; }

        public string RatedByRef { get; set; }

        public string RatedBy { get; set; }

        public string RatedForRef { get; set; }

        public string RatedFor { get; set; }

        public DateTime RatingDate { get; set; }

    }
}
