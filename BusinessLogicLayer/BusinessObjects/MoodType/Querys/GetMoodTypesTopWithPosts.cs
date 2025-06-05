using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.BusinessObjects
{
    public class GetMoodTypesTopWithPosts : QueryStrategyBase<MoodTypeReadDTO>
    {
        private readonly IQueryable<MoodTypeReadDTO> query;
        public GetMoodTypesTopWithPosts(IUnitOfWork unitOfWork, IMapper mapper, int top) : base(unitOfWork, mapper)
        {

            query = unitOfWork
                .MoodTypes
                .Query()
                .AsNoTracking()
                .Include(m => m.Posts)
                .AsNoTracking()
                .Where(t => t.Posts.Count > 0 && t.IsAvailable == true)
                .Take(top)
                .Select(t => new MoodTypeReadDTO
                {
                    Id = t.Id,
                    IsAvailable = t.IsAvailable,
                    Mood = t.Mood,
                    PostCount = t.Posts.Count(p => p.IsPublished == true),
                })
                .OrderByDescending(t => t.PostCount);

        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return result;
        }
    }
}
