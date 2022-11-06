using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using AutoMapper;
using ApiLibros.DTOs;
using ApiLibros.Entidades;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("categorias/{categoriaId:int}/seccions")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SeccionsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public SeccionsController(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<SeccionDTO>>> Get(int categoriaId)
        {
            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }

            var seccions = await dbContext.Seccions.Where(seccionDB => seccionDB.CategoriaId == categoriaId).ToListAsync();

            return mapper.Map<List<SeccionDTO>>(seccions);
        }

        [HttpGet("{id:int}", Name = "obtenerSeccion")]
        public async Task<ActionResult<SeccionDTO>> GetById(int id)
        {
            var seccion = await dbContext.Seccions.FirstOrDefaultAsync(seccionDB => seccionDB.Id == id);

            if (seccion == null)
            {
                return NotFound();
            }

            return mapper.Map<SeccionDTO>(seccion);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(int categoriaId, SeccionCreacionDTO seccionCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }

            var seccion = mapper.Map<Seccions>(seccionCreacionDTO);
            seccion.CategoriaId = categoriaId;
            seccion.UsuarioId = usuarioId;
            dbContext.Add(seccion);
            await dbContext.SaveChangesAsync();

            var seccionDTO = mapper.Map<SeccionDTO>(seccion);

            return CreatedAtRoute("obtenerSeccion", new { id = seccion.Id, categoriaId = categoriaId }, seccionDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int categoriaId, int id, SeccionCreacionDTO seccionCreacionDTO)
        {
            var existeCategoria = await dbContext.Categorias.AnyAsync(categoriaDB => categoriaDB.Id == categoriaId);
            if (!existeCategoria)
            {
                return NotFound();
            }

            var existeSeccion = await dbContext.Seccions.AnyAsync(seccionDB => seccionDB.Id == id);
            if (!existeSeccion)
            {
                return NotFound();
            }

            var seccion = mapper.Map<Seccions>(seccionCreacionDTO);
            seccion.Id = id;
            seccion.CategoriaId = categoriaId;

            dbContext.Update(seccion);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
