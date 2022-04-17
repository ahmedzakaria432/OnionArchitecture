using Application.Shared.Dtos;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Interfaces
{
    public interface IService<TEntity,TDto, TCreateDto, TUpdateDto> where TEntity : AuditableEntity 
                                            where  TDto : AuditableDto
    {
        Task<TDto?> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TCreateDto entity);
        Task<TDto> UpdateAsync(Guid id , TUpdateDto entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync(int pageNumber = 1, int pageSize = int.MaxValue);

    }
}
