using ApiLibros.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("api/categorias")]

    public class CategoriasController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public CategoriasController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetAll()
        {
            return await dbContext.Categorias.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            return await dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            var existeLibro = await dbContext.Libros.AnyAsync(x => x.Id == categoria.LibroId);
            if (!existeLibro)
            {
                return BadRequest($"No existe el libro con el id: {categoria.LibroId}");
            }
            dbContext.Add(categoria);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Categoria categoria, int id)
        {
            var existe = await dbContext.Categorias.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound("La categoria del libro especificada no existe.");

            }
            if (categoria.Id != id)
            {
                return BadRequest("El id de la categoria no coincide con el establecido en la url .");

            }

            dbContext.Update(categoria);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Categorias.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no se ha encontrado");
            }

            //var valideRelation = await dbContext.LibroClase.AnyAsync

            dbContext.Remove(new Categoria { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();

        }


    }
}
