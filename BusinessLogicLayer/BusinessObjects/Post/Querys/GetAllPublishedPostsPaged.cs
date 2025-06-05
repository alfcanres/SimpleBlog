using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPublishedPostsPaged : QueryStrategyBase<PostListDTO>
    {
        private readonly IQueryable<Post> queryCount;
        private readonly IQueryable<Post> queryList;
        public GetAllPublishedPostsPaged(IUnitOfWork unitOfWork, IMapper mapper, PagerParams pager) : base(unitOfWork, mapper)
        {
            var baseQuery = unitOfWork.Posts
                .Query()
                .Include(t => t.MoodType)
                .Include(t => t.PostType)
                .Include(t => t.ApplicationUserInfo)
                .Include(t => t.Comments)
                .Include(t => t.Votes)
                .Where(t =>
                t.IsPublished == true
                &&
                t.MoodType.IsAvailable == true
                &&
                t.PostType.IsAvailable == true
                );

            if (!String.IsNullOrWhiteSpace(pager.SearchKeyWord))
            {
                baseQuery = baseQuery
                    .Where(t =>
                    t.Title.Contains(pager.SearchKeyWord)
                    ||
                    t.MoodType.Mood.Contains(pager.SearchKeyWord)
                    ||
                    t.PostType.Description.Contains(pager.SearchKeyWord)
                    ||
                    t.ApplicationUserInfo.FirstName.Contains(pager.SearchKeyWord)
                    );
            }

            queryList = baseQuery.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage)
                .AsNoTracking()
                .OrderBy(t => t.CreationDate);

            queryCount = baseQuery.OrderBy(t => t.CreationDate);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await queryCount.CountAsync();
        }

        internal override async Task<IEnumerable<PostListDTO>> GetResultsAsync()
        {
            var result = await queryList.ToListAsync();
            return Map(result);
        }
    }
}
