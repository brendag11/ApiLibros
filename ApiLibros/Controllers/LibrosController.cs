using ApiLibros.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("api/libros")] // nombre de la ruta del controlador
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public LibrosController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet] //api/libros
        [HttpGet("listado")] //api/libros/listado
        [HttpGet("/listado")] // /listado      Aquí se sobrescribe la ruta del controlador
      
        public async Task<ActionResult<List<Libro>>> Get() 
        {
            return await dbContext.Libros.Include(x => x.categorias).ToListAsync();

        }

        [HttpGet("primero")] //api/libros/primero       
        public async Task<ActionResult<Libro>> PrimerLibro([FromHeader] int valor, [FromQuery] string libro, [FromQuery] int libroid)
        {
                return await dbContext.Libros.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param=El Principito}")] //{id}/libro
        public async Task<ActionResult<Libro>> Get(int id, string param)
        {
            var libro = await dbContext.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }


        [HttpGet("{titulo}")] // libros/(titulo)
        public async Task<ActionResult<Libro>> Get([FromRoute]string titulo)
        {
            var libro = await dbContext.Libros.FirstOrDefaultAsync(x => x.Titulo.Contains(titulo));
            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }


        [HttpPost]
        public async Task<ActionResult> Post( [FromBody] Libro libro)
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

