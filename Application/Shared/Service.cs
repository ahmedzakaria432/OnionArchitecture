using Application.Shared.Dtos;
using Application.Shared.Interfaces;
using AutoMapper;
using Core.Shared;
using Core.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared
{
    public class Service<TEntity,TDto,TCreateDto,TUpdateDto> : IService<TEntity,TDto, TCreateDto, TUpdateDto> where TEntity : AuditableEntity
                                                                where TDto : AuditableDto
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public Service(IRepository<TEntity> repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity= await GetByIdAsync(id);
            if(entity is null) throw new NotFoundException(nameof(entity),id);
            await _repository.DeleteAsync(_mapper.Map<TEntity>(entity));
        }

        public async Task<IEnumerable<TDto>> GetAllAsync(int pageNumber = 1, int pageSize = int.MaxValue)
        {
            Expression<Func<TEntity, bool>>? expression = null;
            var entities = (await _repository.GetRangeAsync(expression, pageNumber, pageSize)).ToList();
            return _mapper.Map<List<TEntity>, List<TDto>>(entities) ;
        }

        public async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity=await _repository.GetByIdAsync(id);
            if(entity is null) throw new NotFoundException(nameof(entity),id);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> CreateAsync(TCreateDto entity)
        {
            var crearedEntity = await _repository.InsertAsync(_mapper.Map<TEntity>(entity));
            return _mapper.Map<TDto>(crearedEntity);
        }

        public async Task<TDto> UpdateAsync(Guid id, TUpdateDto entity)
        {
            var EntityToUpdate = _repository.GetByIdAsync(id);
            if (EntityToUpdate is null) throw new NotFoundException();
            var updatedEntity = await _repository.UpdateAsync(_mapper.Map<TEntity>(entity));
            return _mapper.Map<TDto>(updatedEntity);
            
        }
    }
}
