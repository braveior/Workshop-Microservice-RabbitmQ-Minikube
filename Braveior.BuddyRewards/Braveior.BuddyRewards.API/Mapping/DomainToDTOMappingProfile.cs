using AutoMapper;
using Braveior.BuddyRewards.DTO;
using Braveior.BuddyRewards.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.API.Mapping
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            //Mapping the Member MongoDB entity to the MemberDTO. 
            CreateMap<Member, MemberDTO>();

            //Mapping the Rating MongoDB Entity to the RatingDTO. Mapping the Member ID for RatedByRef and RatedForRef in DTO from One to One Mapping in Entity
            CreateMap<Rating, RatingDTO>()
            .ForMember(dest => dest.RatedByRef, opt => opt.MapFrom(src => src.RatedByRef.ID))
            .ForMember(dest => dest.RatedForRef, opt => opt.MapFrom(src => src.RatedForRef.ID));
        }
    }
}
