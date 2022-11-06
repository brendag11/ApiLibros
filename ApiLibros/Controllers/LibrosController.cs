using ApiLibros.DTOs;
using ApiLibros.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers //El bueno
{
    [ApiController]
    [Route("libros")] // nombre de la ruta del controlador
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]

    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LibrosController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet] //api/libros
        [AllowAnonymous]
        public async Task<ActionResult<List<GetLibroDTO>>> Get()
        {

            var libros = await dbContext.Libros.ToListAsync();
            return mapper.Map<List<GetLibroDTO>>(libros);

        }

        [HttpGet("{id:int}", Name = "obtenerlibro")] //{id}/libro 
        public async Task<ActionResult<LibroDTOConCategorias>> Get(int id)
        {
            var libro = await dbContext.Libros
                 .Include(libroDB => libroDB.LibroCategoria)
                 .ThenInclude(libroCategoriaDB => libroCategoriaDB.Categoria)
                 .FirstOrDefaultAsync(libroBD => libroBD.Id == id);

            if (libro == null)
            {
                return NotFound();
            }
            return mapper.Map<LibroDTOConCategorias>(libro);
        }


        [HttpGet("{titulo}")] // libros/(titulo)
        public async Task<ActionResult<List<GetLibroDTO>>> Get([FromRoute] string titulo)
        {
            var libros = await dbContext.Libros.Where(libroBD => libroBD.Titulo.Contains(titulo)).ToListAsync();
            return mapper.Map<List<GetLibroDTO>>(libros);
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] LibroDTO libroDto)
        {
            // Ejemplo para validar desde eñ cpntrolador con la BD con ayuda de dcContext

            var existeLibroMismoNombre = await dbContext.Libros.AnyAsync(x => x.Titulo == libroDto.Titulo);
            if (existeLibroMismoNombre)
            {
                return BadRequest($"Ya existe un libro con el nombre {libroDto.Titulo}");
            }
            var libro = mapper.Map<Libro>(libroDto);

            dbContext.Add(libro);
            await dbContext.SaveChangesAsync();
            var libroDTO = mapper.Map<GetLibroDTO>(libro);

            return CreatedAtRoute("obtenerlibro", new { id = libro.Id }, libroDTO);

        }

        [HttpPut("{id:int}")] //api/libros/1
        public async Task<ActionResult> Put(LibroDTO libroCreacionDTO, int id)
        {
            var exist = await dbContext.Libros.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            libro.Id = id;

            dbContext.Update(libro);
            await dbContext.SaveChangesAsync();
            return NoContent();
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

