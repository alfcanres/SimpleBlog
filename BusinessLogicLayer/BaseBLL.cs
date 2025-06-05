using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataTransferObjects;
using Microsoft.Extensions.Logging;


namespace BusinessLogicLayer
{
    public abstract class BaseBLL<CreateDTO, ReadDTO, UpdateDTO> : IBLL<CreateDTO, ReadDTO, UpdateDTO>
        where CreateDTO : class
        where ReadDTO : class
        where UpdateDTO : class
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        protected readonly ValidatorResponse _validate = new ValidatorResponse();
        protected readonly ILogger _logger;
        private readonly IDataAnnotationsValidator _dataAnnotationsValidator;
        protected IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        protected IMapper Mapper { get { return _mapper; } }

        #endregion

        protected BaseBLL(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger logger,
            IDataAnnotationsValidator dataAnnotationsValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _dataAnnotationsValidator = dataAnnotationsValidator;
        }



        #region Public Methods
        public async Task<ReadDTO> InsertAsync(CreateDTO createDTO)
        {
            ReadDTO readDTO = await ExecuteInsertAsync(createDTO);

            return readDTO;
        }
        public async Task<ReadDTO> UpdateAsync(int id, UpdateDTO updateDTO)
        {
            ReadDTO readDTO = await ExecuteUpdateAsync(updateDTO);

            return readDTO;
        }
        public async Task DeleteAsync(int id)
        {
            await ExecuteDeleteAsync(id);
        }
        public async Task<ReadDTO> GetByIdAsync(int id)
        {
            ReadDTO readDTO = await ExecuteGetByIdAsync(id);

            return readDTO;
        }

        #endregion

        #region Protected Methods
        protected void ResetValidations()
        {
            _validate.Clear();
        }
        protected abstract Task<ReadDTO> ExecuteGetByIdAsync(int id);
        protected abstract Task<ReadDTO> ExecuteInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecuteDeleteAsync(int id);
        protected abstract Task<ReadDTO> ExecuteUpdateAsync(UpdateDTO updateDTO);
        protected async Task<int> CountListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await queryStrategy.CountResultsAsync();
        }

        protected async Task<ResponsePagedList<ReadDTO>> ExecutePagedListAsync(QueryStrategyBase<ReadDTO> queryStrategy, PagerParams pager)
        {
            return await GetPagedResponseFromQueryAsync(queryStrategy, pager);
        }

        protected async Task<ResponseList<ReadDTO>> ExecuteListAsync(QueryStrategyBase<ReadDTO> queryStrategy)
        {
            return await GetResponseFromQueryAsync(queryStrategy);
        }

        protected async Task<ResponsePagedList<T>> ExecutePagedListAsync<T>(QueryStrategyBase<T> queryStrategy, PagerParams pager)
            where T : class
        {
          return await GetPagedResponseFromQueryAsync<T>(queryStrategy, pager);

        }

        protected async Task<ResponseList<T>> ExecuteListAsync<T>(QueryStrategyBase<T> queryStrategy)
            where T : class
        {
            return await GetResponseFromQueryAsync<T>(queryStrategy);
        }

        public async Task<ValidatorResponse> ValidateInsertAsync(CreateDTO createDTO)
        {
            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(createDTO, this._validate);

            if (this._validate.IsValid)
            {
                await ExecValidateInsertAsync(createDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE INSERT RETURNED FALSE: {validate}", _validate);
            }

            return this._validate;
        }

        public async Task<ValidatorResponse> ValidateDeleteAsync(int id)
        {
            ResetValidations();

            await ExecValidateDeleteAsync(id);

            if (!_validate.IsValid)
                _logger.LogWarning("VALIDATE DELETE RETURNED FALSE: {id} {validate}", id, _validate);


            return this._validate;
        }

        public async Task<ValidatorResponse> ValidateUpdateAsync(int id, UpdateDTO updateDTO)
        {

            ResetValidations();

            _dataAnnotationsValidator.ValidateModel(updateDTO, this._validate);

            if (id == 0)
            {
                _validate.AddError("Id is required");
            }

            if (this._validate.IsValid)
            {
                await ExecValidateUpdateAsync(id, updateDTO);

                if (!_validate.IsValid)
                    _logger.LogWarning("VALIDATE UPDATE RETURNED FALSE: {validate}", _validate);
            }

            return this._validate;
        }
        #endregion 

        #region Model Validations

        protected abstract Task ExecValidateInsertAsync(CreateDTO createDTO);
        protected abstract Task ExecValidateDeleteAsync(int id);
        protected abstract Task ExecValidateUpdateAsync(int id, UpdateDTO updateDTO);

        #endregion

        private async Task<ResponseList<T>> GetResponseFromQueryAsync<T>(QueryStrategyBase<T> queryStrategyBase)
            where T : class
        {
            var list = await queryStrategyBase.GetResultsAsync();
            var recordCount = await queryStrategyBase.CountResultsAsync();

            return new ResponseList<T>(list, recordCount);

        }

        private async Task<ResponsePagedList<T>> GetPagedResponseFromQueryAsync<T>(QueryStrategyBase<T> queryStrategy, PagerParams pager)
            where T : class
        {
            var list = await queryStrategy.GetResultsAsync();
            var recordCount = await queryStrategy.CountResultsAsync();
            var currentPage = pager.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pager.RecordsPerPage))));
            var pageCount = Convert.ToInt32(pageCountDoub);

            ResponsePagedList<T> newList = new ResponsePagedList<T>(list, recordCount, pager);

            return newList;
        }
    }
}
