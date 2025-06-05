using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllMoodTypeByIsAvailable : QueryStrategyBase<MoodTypeReadDTO>
    {
        private readonly IQueryable<MoodType> query;

        public GetAllMoodTypeByIsAvailable(IUnitOfWork unitOfWork, IMapper mapper, bool isAvailable) : base(unitOfWork, mapper)
        {
            query = unitOfWork.MoodTypes
                .Query()
                .AsNoTracking()
                .Where(t => t.IsAvailable == isAvailable);
                
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
