using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObject
{
    public class GetAllUsersInfoActivePaged : QueryStrategyBase<ApplicationUserInfoListDTO>
    {

        private readonly IQueryable<ApplicationUserInfo> queryCount;
        private readonly IQueryable<ApplicationUserInfo> queryList;

        public GetAllUsersInfoActivePaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager)
            : base(unitOfWork, mapper)
        {
            var queryBase = unitOfWork.UsersInfo
                 .Query()
                 .Include(t => t.Posts)
                 .Include(t => t.Comments)
                 .AsNoTracking()
                 .Where(t => t.IsActive);



            queryList = queryBase
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);

            queryCount = queryBase;
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
