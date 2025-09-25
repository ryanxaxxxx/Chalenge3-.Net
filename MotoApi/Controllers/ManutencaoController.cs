using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;

namespace MotoApi.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de manutenções.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ManutencaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ManutencaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as manutenções com paginação.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão 10).</param>
        /// <returns>Lista paginada de manutenções com links HATEOAS.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var manutencoes = await _context.Manutencoes
                .Include(m => m.Moto)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = manutencoes.Select(m => new
            {
                m.Id,
                m.MotoId,
                MotoModelo = m.Moto.Modelo,
                MotoPlaca = m.Moto.Placa,
                m.TipoServico,
                m.DataServico,
                m.Status,
                m.Descricao,
                Links = new
                {
                    Self = Url.Action(nameof(GetById), new { id = m.Id }),
                    Moto = Url.Action("GetById", "Moto", new { id = m.MotoId })
                }
            });

            return Ok(result);
        }

        /// <summary>
        /// Retorna uma manutenção pelo seu ID.
        /// </summary>
        /// <param name="id">ID da manutenção.</param>
        /// <returns>Objeto da manutenção correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var manutencao = await _context.Manutencoes
                .Include(m => m.Moto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null) return NotFound();

            var result = new
            {
                manutencao.Id,
                manutencao.MotoId,
                MotoModelo = manutencao.Moto.Modelo,
                MotoPlaca = manutencao.Moto.Placa,
                manutencao.TipoServico,
                manutencao.DataServico,
                manutencao.Status,
                manutencao.Descricao,
                Links = new
                {
                    Self = Url.Action(nameof(GetById), new { id = manutencao.Id }),
                    Moto = Url.Action("GetById", "Moto", new { id = manutencao.MotoId })
                }
            };

            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova manutenção.
        /// </summary>
        /// <param name="manutencaoDto">Objeto com os dados da manutenção a ser criada.</param>
        /// <returns>Manutenção criada com ID gerado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Manutencao>> CreateManutencao([FromBody] Manutencao manutencaoDto)
        {
            // Verifica se a moto existe
            var moto = await _context.Motos.FindAsync(manutencaoDto.MotoId);
            if (moto == null)
                return BadRequest($"Moto com Id {manutencaoDto.MotoId} não existe.");

            // Cria a manutenção
            var manutencao = new Manutencao
            {
                MotoId = manutencaoDto.MotoId,
                TipoServico = manutencaoDto.TipoServico,
                DataServico = manutencaoDto.DataServico,
                Status = manutencaoDto.Status,
                Descricao = manutencaoDto.Descricao
            };

            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = manutencao.Id }, manutencao);
        }

        /// <summary>
        /// Atualiza uma manutenção existente pelo ID.
        /// </summary>
        /// <param name="id">ID da manutenção a ser atualizada.</param>
        /// <param name="manutencao">Objeto da manutenção com alterações.</param>
        /// <returns>Sem conteúdo se atualizado com sucesso.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateManutencao(int id, Manutencao manutencao)
        {
            if (id != manutencao.Id) return BadRequest();

            _context.Entry(manutencao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Manutencoes.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma manutenção pelo ID.
        /// </summary>
        /// <param name="id">ID da manutenção a ser removida.</param>
        /// <returns>Sem conteúdo se removido com sucesso.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteManutencao(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return NotFound();

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
