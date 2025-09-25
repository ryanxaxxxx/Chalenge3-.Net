using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;

namespace MotoApi.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de motos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as motos cadastradas com paginação e links HATEOAS.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão 10).</param>
        /// <returns>Lista paginada de motos com links HATEOAS.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var motos = await _context.Motos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = motos.Select(m => new
            {
                m.Id,
                m.Placa,
                m.Modelo,
                links = CreateLinks("Moto", m.Id)
            });

            return Ok(result);
        }

        /// <summary>
        /// Retorna uma moto pelo seu ID.
        /// </summary>
        /// <param name="id">ID da moto.</param>
        /// <returns>Objeto da moto correspondente ao ID fornecido.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Moto>> GetById(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Retorna uma moto pelo número da placa.
        /// </summary>
        /// <param name="placa">Número da placa da moto.</param>
        /// <returns>Objeto da moto correspondente à placa fornecida.</returns>
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Moto>> GetByPlaca(string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="moto">Objeto da moto a ser criada.</param>
        /// <returns>A moto criada com o ID gerado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Moto>> CreateMoto(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
        }

        /// <summary>
        /// Atualiza uma moto existente pelo ID.
        /// </summary>
        /// <param name="id">ID da moto a ser atualizada.</param>
        /// <param name="moto">Objeto da moto com as alterações.</param>
        /// <returns>Sem conteúdo se atualizado com sucesso.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMoto(int id, Moto moto)
        {
            if (id != moto.Id) return BadRequest();

            _context.Entry(moto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Motos.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma moto pelo ID.
        /// </summary>
        /// <param name="id">ID da moto a ser removida.</param>
        /// <returns>Sem conteúdo se removido com sucesso.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Helper para gerar links HATEOAS para a entidade.
        /// </summary>
        private object CreateLinks(string entityName, int id)
        {
            return new[]
            {
                new { rel = "self", href = Url.Action("GetById", entityName, new { id }) },
                new { rel = "update", href = Url.Action("Update" + entityName, entityName, new { id }) },
                new { rel = "delete", href = Url.Action("Delete" + entityName, entityName, new { id }) }
            };
        }
    }
}
