using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;
namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllUsersInfoPaged : QueryStrategyBase<ApplicationUserInfoListDTO>
    {

        private readonly IQueryable<ApplicationUserInfo> queryCount;
        private readonly IQueryable<ApplicationUserInfo> queryList;

        public GetAllUsersInfoPaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager)
            : base(unitOfWork, mapper)
        {
            var query = unitOfWork.UsersInfo
                .Query()
                .Include(t=>t.Posts)
                .Include(t=>t.Comments)
                .AsNoTracking();

            queryList = query.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderByDescending(t => t.Id);

            queryCount = query.OrderBy(t => t.UserID);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<ApplicationUserInfoListDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
