using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostsNotPublished : QueryStrategyBase<PostPendingToPublishDTO>
    {
        private readonly IQueryable<Post> queryCount;
        private readonly IQueryable<Post> queryList;

        public GetAllPostsNotPublished(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager) : base(unitOfWork, mapper)
        {
            var basQuery = unitOfWork.Posts
                 .Query()
                 .Include(t => t.ApplicationUserInfoId)
                 .Include(t => t.MoodType)
                 .Include(t => t.PostType)
                 .AsNoTracking()
                 .Where(t => !t.IsPublished);

            queryCount = basQuery.OrderByDescending(t => t.Id);

            queryList = basQuery.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .OrderBy(t => t.CreationDate);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<PostPendingToPublishDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
