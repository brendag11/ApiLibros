using ApiLibros.Entidades;
using ApiLibros.Filtros;
using ApiLibros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("api/libros")] // nombre de la ruta del controlador
    //[Authorize]


    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger <LibrosController>  logger;
        private readonly IWebHostEnvironment env;
      
      
        public LibrosController(ApplicationDbContext context, IService service, ServiceTransient serviceTransient,
        ServiceScoped serviceScoped, ServiceSingleton serviceSingleton, ILogger<LibrosController> logger, IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {

            throw new NotImplementedException();
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                LibrosControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                LibrosControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                LibrosControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet] //api/libros
        [HttpGet("listado")] //api/libros/listado
        [HttpGet("/listado")] // /listado      Aquí se sobrescribe la ruta del controlador

        //[ResponseCache(Duration = 15)]
        //[Authorize]
        //[ServiceFilter(typeof(FiltroDeAccion))]

        public async Task<ActionResult<List<Libro>>> GetLibros()
        {
            throw new NotImplementedException();
            logger.LogInformation("Se obtiene la lista del libro");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Libros.Include(x => x.categorias).ToListAsync();

        }

        [HttpGet("primero")] //api/libros/primero       
        public async Task<ActionResult<Libro>> PrimerLibro([FromHeader] int valor, [FromQuery] string libro, [FromQuery] int libroid)
        {
                return await dbContext.Libros.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param?}")] //{id}/libro
        public async Task<ActionResult<Libro>> Get(int id, string param)
        {
            var libro = await dbContext.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                return NotFound();
            }
            return libro;
        }


        [HttpGet("obtenerTitulo/{titulo}")] // libros/(titulo)
        public async Task<ActionResult<Libro>> Get([FromRoute]string titulo)
        {
            var libro = await dbContext.Libros.FirstOrDefaultAsync(x => x.Titulo.Contains(titulo));
            if (libro == null)
            {
                logger.LogError("No se encuentra el titulo ingresado. ");
                return NotFound();
            }

            return libro;
        }


        [HttpPost]
        public async Task<ActionResult> Post( [FromBody] Libro libro)
        {
            // Ejemplo para validar desde eñ cpntrolador con la BD con ayuda de dcContext

            var existeLibroMismoNombre = await dbContext.Libros.AnyAsync(x => x.Titulo == libro.Titulo);
            if (existeLibroMismoNombre)
            {
                return BadRequest("Ya existe un libro con este mismo nombre");
            }

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

