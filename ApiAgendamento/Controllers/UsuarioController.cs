using ApiAgendamento.Data;
using ApiAgendamento.Models;
using ApiAgendamento.Repositories.Inferfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiAgendamento.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : GenericController<Usuario>
    {
        public UsuarioController(IRepository<Usuario> repository, IMapper mapper) : base(repository, mapper) 
        {
            
        }
    }
}
