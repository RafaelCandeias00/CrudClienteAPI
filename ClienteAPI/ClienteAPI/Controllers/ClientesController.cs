using AutoMapper;
using ClienteAPI.Context;
using ClienteAPI.DTOs;
using ClienteAPI.Interfaces;
using ClienteAPI.Models;
using ClienteAPI.Pagination;
using ClienteAPI.Respositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClienteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ClientesController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        // GET api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
        {
            try
            {
                var clientes = await _uof.ClienteRepository.GetAll().ToListAsync();
                var clientesDto = _mapper.Map<List<ClienteDTO>>(clientes);

                if (clientesDto == null)
                {
                    return NotFound("Clientes não encontrado!");
                }
                return Ok(clientesDto);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        // GET api/Clientes/clientesPag?pageNumber=1&pageSize=2
        [HttpGet("clientesPag")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll([FromQuery] ClientesParameters clientesParameters)
        {
            try
            {
                var clientes = await _uof.ClienteRepository.GetClientesPag(clientesParameters);

                var metadata = new
                {
                    clientes.TotalCount,
                    clientes.PageSize,
                    clientes.CurrentPage,
                    clientes.TotalPages,
                    clientes.HasNext,
                    clientes.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var clientesDto = _mapper.Map<List<ClienteDTO>>(clientes);
                return clientesDto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        // GET api/Clientes/0
        [HttpGet("id/{id:int:min(1)}")]
        public async Task<ActionResult<ClienteDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var cliente = await _uof.ClienteRepository.GetById(c => c.Id == id);
                
                if (cliente == null)
                {
                    return NotFound($"Id: {id} | Cliente não encontrado!");
                }

                var clientesDto = _mapper.Map<ClienteDTO>(cliente);
                return Ok(clientesDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        // GET api/Clientes/nome
        [HttpGet("nome/{nome:alpha:minlength(2)}")]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetByNome([FromRoute] string nome)
        {
            try
            {
                var clienteNome = await _uof.ClienteRepository.GetByNome(nome);

                if(clienteNome == null)
                {
                    return NotFound($"Nome: {nome} | Nome do cliente não encontrado!");
                }

                var clientesNomeDto = _mapper.Map<List<ClienteDTO>>(clienteNome);
                return Ok(clientesNomeDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        // GET api/Clientes/cpf/Num
        [HttpGet("cpf/{cpf:minlength(11):maxlength(14)}")]
        public async Task<ActionResult<ClienteDTO>> GetByCPF([FromRoute] string cpf)
        {
            try
            {
                var clienteCPF = await _uof.ClienteRepository.GetByCPF(cpf);

                if(clienteCPF == null)
                {
                    return NotFound($"Nome: {cpf} | CPF do cliente não encontrado!");
                }

                var clienteCpfDto = _mapper.Map<ClienteDTO>(clienteCPF);
                return Ok(clienteCpfDto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClienteDTO clienteDto)
        {
            try
            {
                var CpfExiste = await _uof.ClienteRepository.GetByCPF(clienteDto.Cpf);

                if(CpfExiste == null)
                {
                    var cliente = _mapper.Map<Cliente>(clienteDto);

                    _uof.ClienteRepository.Add(cliente);
                    await _uof.Commit();

                    var produtoDTO = _mapper.Map<ClienteDTO>(cliente);

                    return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, produtoDTO);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "CPF já existe!");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> Put([FromBody] ClienteDTO clienteDto, [FromRoute] int id)
        {
            try
            {
                if(id != clienteDto.Id)
                {
                    return BadRequest("Dados incorretos!");
                }

                var cliente = _mapper.Map<Cliente>(clienteDto);

                _uof.ClienteRepository.Update(cliente);
                await _uof.Commit();
                return Ok($"Id: {id} | Cliente atualizado!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ClienteDTO>> Delete([FromRoute] int id)
        {
            try
            {
                var cliente = await _uof.ClienteRepository.GetById(c => c.Id == id);

                if(cliente == null)
                {
                    return NotFound($"Id: {id} | Cliente não encontrado!");
                }
                _uof.ClienteRepository.Delete(cliente);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ClienteDTO>(cliente);

                return Ok($"Id: {id} | Cliente excluído com sucesso! | {produtoDto}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }
    }
}

