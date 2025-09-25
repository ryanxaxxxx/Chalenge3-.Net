using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoApi.Data;
using MotoApi.Models;

namespace MotoApi.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os usuários com paginação e links HATEOAS.
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão 1).</param>
        /// <param name="pageSize">Quantidade de itens por página (padrão 10).</param>
        /// <returns>Lista paginada de usuários com links HATEOAS.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = await _context.Usuarios
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = usuarios.Select(u => new
            {
                u.Id,
                u.Nome,
                u.Email,
                links = CreateLinks("Usuario", u.Id)
            });

            return Ok(result);
        }

        /// <summary>
        /// Retorna um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Objeto do usuário correspondente ao ID fornecido.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuario">Objeto do usuário a ser criado.</param>
        /// <returns>O usuário criado com o ID gerado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Atualiza um usuário existente pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="usuario">Objeto do usuário com as alterações.</param>
        /// <returns>Sem conteúdo se atualizado com sucesso.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser removido.</param>
        /// <returns>Sem conteúdo se removido com sucesso.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
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

