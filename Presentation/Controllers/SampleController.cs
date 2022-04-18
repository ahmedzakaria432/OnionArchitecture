using Application.Samples.Dtos;
using Application.Samples.Interfaces;
using Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ISampleService _sampleService;
        private readonly IUnitOfWork _unitOfWork;

        public SampleController(ISampleService sampleService,IUnitOfWork unitOfWork)
        {
            this._sampleService = sampleService;
            this._unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<SampleDto> CreateAsync(CreateSampleDto entity)
        {
            var CreatedEntity= await _sampleService.CreateAsync(entity);
            await _unitOfWork.CommitAsync();
            return CreatedEntity;
        }
        [HttpDelete]
        public async Task DeleteAsync(Guid id)
        {
            await _sampleService.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }
        [HttpGet]
        public async Task<IEnumerable<SampleDto>> GetAllAsync(int pageNumber = 1, int pageSize = int.MaxValue)
        {
            return await _sampleService.GetAllAsync(pageNumber, pageSize);
        }
        [HttpGet("{id}")]
        public async Task<SampleDto?> GetByIdAsync(Guid id)
        {
            return await _sampleService.GetByIdAsync(id);
        }
        [HttpPut("{id}")]
        public async Task<SampleDto> UpdateAsync(Guid id, UpdateSampleDto entity)
        {
            var UpdatedEntity = await _sampleService.UpdateAsync(id, entity);
            await _unitOfWork.CommitAsync();
            return UpdatedEntity;
        }
    }
}
