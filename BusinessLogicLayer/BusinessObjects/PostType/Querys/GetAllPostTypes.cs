using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostTypes : QueryStrategyBase<PostTypeReadDTO>
    {
        public GetAllPostTypes(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await unitOfWork.PostTypes.Query().CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await unitOfWork.PostTypes
                .Query()
                .AsNoTracking()
                .ToListAsync();
          
            return Map(result);
        }
    }
}
