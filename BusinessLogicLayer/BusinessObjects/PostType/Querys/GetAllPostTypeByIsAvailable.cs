using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entity;
using DataTransferObjects.DTO;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.BusinessObjects
{
    public class GetAllPostTypeByIsAvailable : QueryStrategyBase<PostTypeReadDTO>
    {
        private readonly IQueryable<PostType> query;

        public GetAllPostTypeByIsAvailable(IUnitOfWork unitOfWork, IMapper mapper, bool isAvailable) : base(unitOfWork, mapper)
        {
            query = unitOfWork.PostTypes.Query()
                .Where(t => t.IsAvailable == isAvailable)
                .AsNoTracking();
        }

        internal override async Task<int> CountResultsAsync()
        {
            return await query.CountAsync();
        }

        internal override async Task<IEnumerable<PostTypeReadDTO>> GetResultsAsync()
        {
            var result = await query.ToListAsync();
            return Map(result);
        }
    }
}
