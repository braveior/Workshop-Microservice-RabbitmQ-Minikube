using Braveior.BuddyRewards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.Service.Interfaces
{
    public interface IDataService
    {
        TableDataDTO<MemberDTO> GetMembers(TableStateDTO tableState);

        List<MemberDTO> SearchMember(string key);

        Task AddRating(RatingDTO rating);

        TableDataDTO<RatingDTO> GetRatings(TableStateDTO tableState);

        List<AverageRatingDTO> GetAverageRatings(string ratedFor);


    }
}
