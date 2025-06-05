using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPublishedPostsByMoodPaged : QueryStrategyBase<PostReadDTO>
    {
        private readonly IQueryable<Post> queryCount;
        private readonly IQueryable<Post> queryList;
        public GetAllPublishedPostsByMoodPaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager, int moodTypeId) : base(unitOfWork, mapper)
        {
            var query = unitOfWork.Posts
                 .Query()
                 .Include(t => t.ApplicationUserInfoId)
                 .Include(t => t.MoodType)
                 .Include(t => t.PostType)
                 .AsNoTracking()
                 .Where(t => t.IsPublished && t.MoodTypeId == moodTypeId);


            queryList = query.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderBy(t => t.CreationDate);

            queryCount = query.OrderBy(t => t.MoodTypeId);


        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<PostReadDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
