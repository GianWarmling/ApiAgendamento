using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GenericController<T, TDto> : ControllerBase where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;
        public GenericController(IRepository<T> repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dados)
        {
            var entity = _mapper.Map<T>(dados);
            await _repository.AddAsync(entity);
            await _repository.SaveChanges();
            return Created(nameof(T),entity);
        }
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] T dados) 
        {
            await _repository.UpdateAsync(dados);
            await _repository.SaveChanges();
            return Ok(dados);
        }
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int id)
        {
            var dado = await _repository.GetByIdAsync(id);
            if(dado is null)
            {
                return BadRequest("O REGISTRO NÃO EXISTE!");
            }
            await _repository.DeleteAsync(dado);
            await _repository.SaveChanges();
            return Ok("Removido Com Sucesso!");
        }
    }
}
