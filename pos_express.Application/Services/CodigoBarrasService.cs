using pos_express.Domain.Entities;
using pos_express.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace pos_express.Application.Services
{
    public class CodigoBarrasService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CodigoBarrasService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CodigoBarras> AsignarCodigoBarra(int idProducto)
        {
            var producto = await _unitOfWork.ExpProductos.GetByIdAsync(idProducto);
            if (producto == null)
            {
                throw new InvalidOperationException($"El producto comercial con ID {idProducto} no existe.");
            }

            var codigoBarra = new CodigoBarras
            {
                IdProducto = idProducto,
                Activo = true
            };

            await _unitOfWork.CodigosBarras.AddAsync(codigoBarra);
            await _unitOfWork.CompleteAsync();

            return codigoBarra;
        }
    }
}
