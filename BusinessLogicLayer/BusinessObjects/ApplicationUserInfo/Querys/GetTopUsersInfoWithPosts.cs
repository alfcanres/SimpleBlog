using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
namespace BusinessLogicLayer.BusinessObjects
{
    public class GetTopUsersInfoWithPosts : QueryStrategyBase<ApplicationUserInfoListDTO>
    {
        private readonly IQueryable<ApplicationUserInfo> query;
        public GetTopUsersInfoWithPosts(IUnitOfWork unitOfWork, IMapper mapper, int top) : base(unitOfWork, mapper)
        {
            query = unitOfWork.UsersInfo
                .Query()
                .Take(top)
                .OrderByDescending(t => t.UserName)
                .AsNoTracking();
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<ApplicationUserInfoListDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}

