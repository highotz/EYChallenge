using AutoMapper;
using Community.OData.Linq.AspNetCore;
using EYChallenge.Domain.Product.ViewModels.Common;
using EYChallenge.Domain.Product.ViewModels.Response;
using EYChallenge.Presentation.API.Attributes;
using EYChallenge.Utilities.SystemObjects.Entities;
using EYChallenge.Utilities.SystemObjects.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData;
using Swashbuckle.AspNetCore.Annotations;

namespace EYChallenge.Presentation.API.Controllers
{
    public abstract class BaseController<TEntity, TKey, TInputViewModel, TReturnViewModel> : Controller
        where TEntity : IEntity<TKey>
        where TInputViewModel : class
        where TReturnViewModel : class
    {

        #region Variables

        private readonly IEntityService<TEntity, TKey> _entityService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        #endregion Variables

        #region Constructor

        public BaseController(IEntityService<TEntity, TKey> entityService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _entityService = entityService;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected BaseController()
        {
        }

        #endregion Constructor

        #region .: Search :.

        /// <summary>
        /// Search list {entity} OData Operators are accepted
        /// </summary>
        /// <returns>A paged list of {entity}</returns>
        [HttpGet, Route(""), ODataMethod]
        [SwaggerOperation(OperationId = "{entity}GetOData")]
        public virtual IActionResult GetOData([SwaggerExclude] ODataQueryOptions options)
        {
            try
            {
                IEnumerable<TEntity> query = _entityService.GetAllFromOData(options, out int totalRecords, out int pageSize).ToList();

                return Ok(new PagedResponseViewModel<TReturnViewModel>
                {
                    Data = _mapper.Map<IEnumerable<TReturnViewModel>>(query),
                    Total = totalRecords,
                    PageSize = pageSize
                });
            }
            catch (ODataException ex)
            {
                return BadRequest(new ErrorResponseViewModel
                {
                    Code = "INVALID_REQUEST",
                    Message = ex.Message
                });
            }
        }

        #endregion

        #region Persistance

        /// <summary>
        /// Create a new {entity}
        /// </summary>
        /// <param name="model">the model with properties of {entity} to be created </param>
        /// <returns>Returns the newly created {entity}</returns>
        [HttpPost, Route("")]
        [SwaggerOperation(OperationId = "{entity}Create")]
        public virtual async Task<IActionResult> Create([FromBody] TInputViewModel model)
        {
            TReturnViewModel result = _mapper.Map<TReturnViewModel>(await _entityService.AddAndReturnAsync(_mapper.Map<TEntity>(model)).ConfigureAwait(false));

            return Ok(new SuccessResponseViewModel<TReturnViewModel>
            {
                Data = result
            });
        }

        /// <summary>
        /// Create a list of entities
        /// </summary>
        /// <param name="model">List of {entity} to be created</param>
        /// <returns>Returns the list of newly created {entity}</returns>
        [HttpPost, Route("batch")]
        [SwaggerOperation(OperationId = "{entity}CreateBatch")]
        public virtual async Task<IActionResult> CreateBatch([FromBody] BatchViewModel<TInputViewModel> model)
        {
            IEnumerable<TReturnViewModel> result = _mapper.Map<IEnumerable<TReturnViewModel>>(await _entityService.AddBatchAsync(_mapper.Map<IEnumerable<TEntity>>(model?.Data)).ConfigureAwait(false));

            return Ok(new SuccessResponseViewModel<IEnumerable<TReturnViewModel>>
            {
                Data = result
            });

        }

        /// <summary>
        /// Update a list of entities
        /// </summary>
        /// <param name="model">List of {entity} to be updated</param>
        /// <returns>Returns the list of updated {entity}</returns>
        [HttpPut, Route("batch")]
        [SwaggerOperation(OperationId = "{entity}UpdateBatch")]
        public virtual async Task<IActionResult> UpdateBatch([FromBody] BatchViewModel<TInputViewModel> model)
        {
            IEnumerable<TReturnViewModel> result = _mapper.Map<IEnumerable<TReturnViewModel>>(await _entityService.UpdateBatchAsync(_mapper.Map<IEnumerable<TEntity>>(model?.Data)).ConfigureAwait(false));

            return Ok(new SuccessResponseViewModel<IEnumerable<TReturnViewModel>>
            {
                Data = result
            });

        }

        /// <summary>
        /// Update a single {entity}
        /// </summary>
        /// <param name="model">{entity} to be updated</param>
        /// <returns>Returns the updated {entity}</returns>
        [HttpPut, Route("")]
        [SwaggerOperation(OperationId = "{entity}Update")]
        public virtual async Task<IActionResult> Update([FromBody] TInputViewModel model)
        {
            TReturnViewModel result = _mapper.Map<TReturnViewModel>(await _entityService.UpdateAndReturnAsync(_mapper.Map<TEntity>(model)).ConfigureAwait(false));

            return Ok(new SuccessResponseViewModel<TReturnViewModel>
            {
                Data = result
            });

        }

        /// <summary>
        /// Delete a single {entity}
        /// </summary>
        /// <param name="id">Id of {entity} </param>
        [HttpDelete, Route("{id}")]
        [SwaggerOperation(OperationId = "{entity}Delete")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            TEntity entity = _entityService.FindById(id);

            if (entity != null)
            {
                await _entityService.DeleteAsync(entity).ConfigureAwait(false);

                return Ok(new SuccessResponseViewModel<string>
                {
                    Data = ""
                });
            }
            else
            {
                _logger.LogWarning($"Id {id} não encontrado");
                return NotFound();
            }

        }

        /// <summary>
        /// Find a single {entity} by primary key
        /// </summary>
        /// <param name="id">Id of {entity}</param>
        /// <returns>Returns the {entity} with specified id </returns>
        [HttpGet, Route("{id}")]
        [SwaggerOperation(OperationId = "{entity}GetById")]
        public virtual IActionResult GetById(TKey id)
        {
            TEntity entity = _entityService.FindById(id);

            if (entity != null)
            {
                return Ok(new SuccessResponseViewModel<TReturnViewModel>
                {
                    Data = _mapper.Map<TReturnViewModel>(entity)
                });
            }
            else
            {
                return NotFound();
            }

        }

        #endregion Persistance

    }
}
