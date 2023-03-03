using ClienteAPI.Context;
using ClienteAPI.Interfaces;
using ClienteAPI.Models;
using ClienteAPI.Respositorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICliente _cliente;

        public ClientesController(ICliente cliente)
        {
            _cliente = cliente;
        }

        // GET api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        {
            try
            {
                var clientes = await _cliente.GetAll();

                if (clientes == null)
                {
                    return NotFound("Clientes não encontrado!");
                }
                return Ok(clientes);
            } 
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        // GET api/Clientes/0
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> GetById([FromRoute] int id)
        {
            try
            {
                var cliente = await _cliente.GetById(id);

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado!");
                }

                return Ok(cliente);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            try
            {
                await _cliente.Post(cliente);
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromBody] Cliente cliente, int id)
        {
            try
            {
                if(cliente.Id == id)
                {
                    await _cliente.Put(cliente);
                    return Ok($"Id: {id} | Cliente atualizado!");
                }
                else
                {
                    return BadRequest("Dados incorretos!");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _cliente.GetById(id);
                if(cliente != null)
                {
                    await _cliente.Delete(cliente);
                    return Ok($"Id: {id} | Cliente excluído com sucesso!");
                }
                else
                {
                    return BadRequest($"Id: {id} | Cliente não encontrado!");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter clientes!");
            }
        }
    }
}

