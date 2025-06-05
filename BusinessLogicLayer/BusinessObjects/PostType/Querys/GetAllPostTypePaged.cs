using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostTypePaged : QueryStrategyBase<PostTypeReadDTO>
    {

        private readonly IQueryable<PostType> queryCount;
        private readonly IQueryable<PostType> queryList;

        public GetAllPostTypePaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager) : base(unitOfWork, mapper)
        {
            var query = unitOfWork.PostTypes
                .Query()
                .Include(t => t.Posts)
                .AsNoTracking();

            queryList =
                query.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);

            queryCount = unitOfWork.PostTypes.Query();

        }
        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
