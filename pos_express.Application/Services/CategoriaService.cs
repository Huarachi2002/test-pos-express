using pos_express.Domain.Entities;
using pos_express.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Application.Services
{
    public class CategoriaService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CategoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> RegistrarCategoria(string descripcion, int? idCategoriaPadre = null)
        {
            var categoria = new Categorias
            {
                Descripcion = descripcion,
                Activo = true,
                IdCategoriaPadre = idCategoriaPadre
            };

            await _unitOfWork.Categorias.AddAsync(categoria);
            await _unitOfWork.CompleteAsync();
            return categoria.IdCategoria;
        }

        public async Task AsignarCategoriaProducto(int idProducto, int idCategoria)
        {
            var producto = await _unitOfWork.ExpProductos.GetByIdAsync(idProducto);
            if (producto == null)
            {
                throw new InvalidOperationException($"El producto con ID {idProducto} no existe.");
            }

            var categoria = await _unitOfWork.Categorias.GetByIdAsync(idCategoria);
            if (categoria == null)
            {
                throw new InvalidOperationException($"La categoria con ID {idCategoria} no existe.");
            }

            if (!categoria.Activo)
            {
                throw new InvalidOperationException($"La categoria: {categoria.Descripcion} no esta activa");
            }

            var asignado = await _unitOfWork.ProductosCategorias.FindAsync(
                pc => pc.IdProducto == idProducto && pc.IdCategoria == idCategoria
            );

            if (asignado.Any())
            {
                throw new InvalidOperationException($"El producto ya tiene asignado la categoria '{categoria.Descripcion}'.");
            }

            var productoCategoria = new ProductosCategorias
            {
                IdProducto = idProducto,
                IdCategoria = idCategoria,
                FechaCreacion = DateTime.UtcNow
            };

            await _unitOfWork.ProductosCategorias.AddAsync(productoCategoria);
            await _unitOfWork.CompleteAsync();
            
        }

        public async Task<int> ObtenerCantidadCategoriaByProducto(int idProducto)
        {
            var categorias = await _unitOfWork.ProductosCategorias.FindAsync(
                pc => pc.IdProducto == idProducto
            );
            return categorias.Count();
        }
    }
}
