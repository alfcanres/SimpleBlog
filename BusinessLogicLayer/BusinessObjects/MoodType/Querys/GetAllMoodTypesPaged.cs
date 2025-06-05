using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllMoodTypesPaged : QueryStrategyBase<MoodTypeReadDTO>
    {
        private readonly IQueryable<MoodType> queryCount;
        private readonly IQueryable<MoodType> queryList;

        public GetAllMoodTypesPaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager)
            : base(unitOfWork, mapper)
        {
            var queryBase = unitOfWork.MoodTypes
                .Query()
                .Include(t => t.Posts)
                .AsNoTracking()
                .Where(t => t.IsAvailable);

            if(pager.SearchKeyWord != null)
            {
                queryBase = queryBase.Where(t => t.Mood.Contains(pager.SearchKeyWord));
            }

            queryList = queryBase
                .Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);

            queryCount = queryBase.OrderByDescending(t => t.Id);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
