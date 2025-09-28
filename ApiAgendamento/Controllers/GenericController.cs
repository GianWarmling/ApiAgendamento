using ApiAgendamento.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<T, TDto> : ControllerBase where T : class
    {
        private readonly IRepository<T> _repository;
        protected readonly IMapper _mapper;

        public GenericController(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get() =>
            Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = GetEntityId(entity) }, dto);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(int id, [FromBody] T entity)
        {
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();

            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [NonAction]
        protected virtual object GetEntityId(T entity)
        {
            // Assumindo convenção de "Id"
            var prop = typeof(T).GetProperty("Id");
            return prop?.GetValue(entity);
        }

    }

}
