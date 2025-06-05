using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetPostsByUserPaged : QueryStrategyBase<PostListDTO>
    {
        private readonly IQueryable<Post> queryList;
        private readonly IQueryable<Post> queryCount;
        public GetPostsByUserPaged(
            int UserID, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            PagerParams pager,
            bool getOnlyPublished = true
            ) : base(unitOfWork, mapper)
        {
            var baseQuery = unitOfWork.Posts
                 .Query()
                 .AsNoTracking();

            if (!String.IsNullOrEmpty(pager.SearchKeyWord))
            {
                baseQuery = baseQuery
                    .Where(t =>
                    t.Title.Contains(pager.SearchKeyWord)
                    ||
                    t.MoodType.Mood.Contains(pager.SearchKeyWord)
                    ||
                    t.PostType.Description.Contains(pager.SearchKeyWord)
                    );
            }

            if (getOnlyPublished)
            {
                baseQuery = baseQuery
                    .Where(t => t.IsPublished && t.ApplicationUserInfoId == UserID)
                    .OrderBy(t => t.CreationDate);
            }
            else
            {
                baseQuery = baseQuery.Where(t=>t.ApplicationUserInfoId == UserID)
                    .OrderBy(t => t.CreationDate);
            }
            
            queryCount = baseQuery;

            queryList = baseQuery.Skip((pager.CurrentPage - 1) * pager.RecordsPerPage)
                .Take(pager.RecordsPerPage);

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
