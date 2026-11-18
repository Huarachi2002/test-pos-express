using System;
using System.Threading.Tasks;
using pos_express.Domain.Entities;
using pos_express.Domain.Strategies;
using pos_express.Domain.Interfaces;

namespace pos_express.Application.Services
{
    public class ProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEstrategiaNegocio _estrategia;

        public ProductoService(IUnitOfWork unitOfWork, IEstrategiaNegocio estrategia)
        {
            _unitOfWork = unitOfWork;
            _estrategia = estrategia;
        }

        public async Task<int> RegistrarProductoErp(string nombre, decimal costo, int stock, int idTipoProducto, DateTime fechaVencimiento, string? observaciones = null)
        {
            decimal precioVenta = _estrategia.CalcularPrecioProducto(costo);

            var expProducto = new ExpProductos
            {
                Nombre = nombre,
                Precio = precioVenta,
                FechaVencimiento = fechaVencimiento,
                IdTipoProducto = idTipoProducto,
                Observaciones = observaciones ?? string.Empty
            };

            await _unitOfWork.ExpProductos.AddAsync(expProducto);
            await _unitOfWork.CompleteAsync();

            var erpProducto = new ErpProductos
            {
                IdProducto = expProducto.IdProducto,
                Costo = costo,
                Stock = stock
            };

            await _unitOfWork.ErpProductos.AddAsync(erpProducto);
            await _unitOfWork.CompleteAsync();

            return expProducto.IdProducto;
        }
    }
}
