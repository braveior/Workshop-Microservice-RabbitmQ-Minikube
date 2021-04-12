using Braveior.BuddyRewards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.Service.Interfaces
{
    public interface ILoginService
    {
        MemberDTO Login(LoginDTO member);
        Task<MemberDTO> GetUserFromAccessToken(string accessToken);
    }

}
