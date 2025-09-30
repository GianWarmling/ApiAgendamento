using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;
        public GenericController(IRepository<T> repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T dados)
        {
           return Created(nameof(T),await _repository.AddAsync(dados));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] T dados) 
        {
            return Ok(await _repository.UpdateAsync(dados));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var dado = await _repository.GetByIdAsync(id);
            if(dado is null)
            {
                return BadRequest("O REGISTRO NÃO EXISTE!");
            }
            await _repository.DeleteAsync(dado);
            return Ok("Removido Com Sucesso!");
        }
    }
}
