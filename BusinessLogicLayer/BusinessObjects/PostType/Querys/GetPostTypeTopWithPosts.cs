using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetPostTypeTopWithPosts : QueryStrategyBase<PostTypeReadDTO>
    {
        private readonly IQueryable<PostTypeReadDTO> query;

        public GetPostTypeTopWithPosts(IUnitOfWork unitOfWork, IMapper mapper, int top) : base(unitOfWork, mapper)
        {
            query = unitOfWork.PostTypes
                .Query()
                .Include(m => m.Posts)
                .AsNoTracking()
                .Where(t => t.Posts.Count > 0 && t.IsAvailable == true)
                .Take(top)
                .Select(t => new PostTypeReadDTO
                {
                    Id = t.Id,
                    IsAvailable = t.IsAvailable,
                    Description = t.Description,
                    PostCount = t.Posts.Count(p => p.IsPublished == true),
                })
                .OrderByDescending(t => t.PostCount);
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();

            return result;
        }
    }
}
