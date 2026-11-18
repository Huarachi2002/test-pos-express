using pos_express.Domain.Entities;
using pos_express.Domain.Interfaces;
using pos_express.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Application.Services
{
    public class VentaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEstrategiaNegocio _estrategia;

        public VentaService(IUnitOfWork unitOfWork, IEstrategiaNegocio estrategia)
        {
            _unitOfWork = unitOfWork;
            _estrategia = estrategia;
        }

        public async Task<int> RegistrarVenta(int idProducto, DateTime fecha, string Cliente, int cantidad)
        {
            var erpProducto = await _unitOfWork.ErpProductos.GetByIdAsync(idProducto);
            if (erpProducto == null)
            {
                throw new InvalidOperationException($"El ErpProducto con ID {idProducto} no existe.");
            }

            var expProducto = await _unitOfWork.ExpProductos.GetByIdAsync(idProducto);
            if (expProducto == null)
            {
                throw new InvalidOperationException($"El ExpProducto con ID {idProducto} no existe.");
            }

            var (realizaVenta, message) = _estrategia.ValidarStockParaVenta(erpProducto.Stock, cantidad);
            if (!realizaVenta)
            {
                throw new InvalidOperationException(message);
            }

            var categoriasPorProducto = await _unitOfWork.ProductosCategorias.FindAsync(
                pc => pc.IdProducto == idProducto
            );

            int totalCategorias = categoriasPorProducto.Count();

            decimal subTotal = expProducto.Precio * cantidad;
            decimal descuento = _estrategia.CalcularDescuentoVenta(subTotal, totalCategorias);
            decimal total = subTotal - descuento;

            var ventaExpress = new VentaExpress
            {
                Fecha = fecha,
                Cliente = Cliente,
                Producto = expProducto.Nombre,
                UniqueProducto = erpProducto.UniqueCodigo,
                Cantidad = cantidad,
                Precio = expProducto.Precio,
                Descuento = descuento,
                Total = total,
                IdProducto = idProducto
            };

            await _unitOfWork.VentaExpress.AddAsync(ventaExpress);
            await _unitOfWork.CompleteAsync();

            erpProducto.Stock = erpProducto.Stock - cantidad;
            _unitOfWork.ErpProductos.Update(erpProducto);
            await _unitOfWork.CompleteAsync();

            return ventaExpress.Id;
        }
    }
}
