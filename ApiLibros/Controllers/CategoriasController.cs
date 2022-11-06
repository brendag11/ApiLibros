using ApiLibros.DTOs;
using ApiLibros.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("categorias")]

    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CategoriasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoCategoria")]
        public async Task<ActionResult<List<Categoria>>> GetAll()
        {
            return await dbContext.Categorias.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerCategoria")]
        public async Task<ActionResult<CategoriaDTOConLibros>> GetById(int id)
        {
            var categoria = await dbContext.Categorias
              .Include(categoriaDB => categoriaDB.LibroCategoria)
              .ThenInclude(libroCategoriaDB => libroCategoriaDB.Libro)
              .Include(seccionDB => seccionDB.Seccions)
              .FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }
            categoria.LibroCategoria = categoria.LibroCategoria.OrderBy(x => x.Orden).ToList();
            return mapper.Map<CategoriaDTOConLibros>(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaCreacionDTO categoriaCreacionDTO)
        {
            if (categoriaCreacionDTO.LibrosIds == null)
            {
                return BadRequest("No se puede crear una categoria sin libros.");
            }

            var librosIds = await dbContext.Libros
                .Where(libroBD => categoriaCreacionDTO.LibrosIds.Contains(libroBD.Id)).
                Select(x => x.Id).ToListAsync();

            if (categoriaCreacionDTO.LibrosIds.Count != librosIds.Count)
            {
                return BadRequest("No existe uno de los libros enviados");
            }

            var categoria = mapper.Map<Categoria>(categoriaCreacionDTO);

            OrdenarPorAlumnos(categoria);

            dbContext.Add(categoria);
            await dbContext.SaveChangesAsync();

            var categoriaDTO = mapper.Map<CategoriaDTO>(categoria);

            return CreatedAtRoute("obtenerCategoria", new { id = categoria.Id }, categoriaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CategoriaCreacionDTO categoriaCracionDTO, int id)
        {
            var categoriaDB = await dbContext.Categorias
                  .Include(x => x.LibroCategoria)
                  .FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null)
            {
                return NotFound();
            }

            categoriaDB = mapper.Map(categoriaCracionDTO, categoriaDB);

            OrdenarPorAlumnos(categoriaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Categorias.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El recurso no se ha encontrado");
            }

            dbContext.Remove(new Categoria { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();

        }
        private void OrdenarPorAlumnos(Categoria categoria)
        {
            if (categoria.LibroCategoria != null)
            {
                for (int i = 0; i < categoria.LibroCategoria.Count; i++)

                {
                    categoria.LibroCategoria[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoriaPatchDTO> patchDocument)
        {
            if (patchDocument == null) { return BadRequest(); }

            var categoriaDB = await dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null) { return NotFound(); }

            var categoriaDTO = mapper.Map<CategoriaPatchDTO>(categoriaDB);

            patchDocument.ApplyTo(categoriaDTO);

            var isValid = TryValidateModel(categoriaDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(categoriaDTO, categoriaDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }

}