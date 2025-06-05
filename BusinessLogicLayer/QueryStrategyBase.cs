using AutoMapper;
using BusinessLogicLayer.Interfaces;

namespace BusinessLogicLayer
{
    public abstract class QueryStrategyBase<ReadDTO>
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        protected QueryStrategyBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        internal abstract Task<IEnumerable<ReadDTO>> GetResultsAsync();

        internal abstract Task<int> CountResultsAsync();

        protected IEnumerable<ReadDTO> Map(object list)
        {
            return mapper.Map<List<ReadDTO>>(list);
        }
       

    }
}
