using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.DTO
{
    public class TableStateDTO
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string SearchKey { get; set; }

        public string SortDirection { get; set; }

        public string FilterID { get; set; }
    }
}
