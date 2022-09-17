using ApiLibros.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public LibrosController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> Get()
        {
            return await dbContext.Libros.Include(x => x.categorias).ToListAsync();

        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            dbContext.Add(libro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] //api/libros/1
        public async Task<ActionResult> Put(Libro libro, int id)
        {
            if (libro.Id != id)
            {
                return BadRequest("El id del libro no coincide con el establecido en el url");

            }

            dbContext.Update(libro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Libros.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("La informacion a sido borrada");
            }
            dbContext.Remove(new Libro()
            {
                Id = id

            });
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }

}

