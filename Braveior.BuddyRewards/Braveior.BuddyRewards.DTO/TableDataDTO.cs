using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.DTO
{
    public class TableDataDTO<T>
    {
        public List<T> PagedMemberData { get; set; }

        public long TotalCount { get; set; }


    }
}
