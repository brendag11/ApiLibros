using ApiLibros.DTOs;
using ApiLibros.Entidades;
using AutoMapper;

namespace ApiLibros.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<LibroDTO, Libro>();
            CreateMap<Libro, GetLibroDTO>();
            CreateMap<Libro, LibroDTOConCategorias>().ForMember(libroDTO =>
            libroDTO.Categorias, opciones => opciones.MapFrom(MapLibroDTOCategorias));

            CreateMap<CategoriaCreacionDTO, Categoria>()
                .ForMember(categoria => categoria.LibroCategoria, opciones => opciones.MapFrom(MapLibroCategoria));
            CreateMap<Categoria, CategoriaDTO>();

            CreateMap<Categoria, CategoriaDTOConLibros>()
            .ForMember(categoriaDTO => categoriaDTO.Libros, opciones => opciones.MapFrom(MapCategoriaDTOLibros));

            CreateMap<CategoriaPatchDTO, Categoria>().ReverseMap();
            CreateMap<SeccionCreacionDTO, Seccions>();
            CreateMap<Seccions, SeccionDTO>();
        }

        private List<CategoriaDTO> MapLibroDTOCategorias(Libro libro, GetLibroDTO getLibroDTO)
        {
            var result = new List<CategoriaDTO>();

            if (libro.LibroCategoria == null) { return result; }

            foreach (var libroCategoria in libro.LibroCategoria)
            {
                result.Add(new CategoriaDTO()
                {
                    Id = libroCategoria.CategoriaId,
                    Titulo = libroCategoria.Categoria.Titulo,
                    Autor = libroCategoria.Categoria.Autor
                });
            }

            return result;
        }

        private List<GetLibroDTO> MapCategoriaDTOLibros(Categoria categoria, CategoriaDTO categoriaDTO)
        {
            var result = new List<GetLibroDTO>();

            if (categoria.LibroCategoria == null)
            {
                return result;
            }

            foreach (var librocategoria in categoria.LibroCategoria)
            {
                result.Add(new GetLibroDTO()
                {
                    Id = librocategoria.LibroId,
                    Titulo = librocategoria.Libro.Titulo,
                    Autor = librocategoria.Libro.Autor
                });
            }

            return result;
        }

        private List<LibroCategoria> MapLibroCategoria(CategoriaCreacionDTO categoriaCreacionDTO, Categoria categoria)
        {
            var resultado = new List<LibroCategoria>();

            if (categoriaCreacionDTO.LibrosIds == null) { return resultado; }
            foreach (var libroId in categoriaCreacionDTO.LibrosIds)
            {
                resultado.Add(new LibroCategoria() { LibroId = libroId });
            }
            return resultado;
        }
    }
}
