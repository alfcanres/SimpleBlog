using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllMoodTypes : QueryStrategyBase<MoodTypeReadDTO>
    {
        public GetAllMoodTypes(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await unitOfWork.MoodTypes.Query().CountAsync();
        }

        internal override async Task<IEnumerable<MoodTypeReadDTO>> GetResultsAsync()
        {
            var result = await unitOfWork.MoodTypes
                .Query()
                .AsNoTracking()
                .ToListAsync();

            return Map(result);
        }
    }
}
