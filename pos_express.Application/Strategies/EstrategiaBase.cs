using pos_express.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Application.Strategies
{
    public class EstrategiaBase: IEstrategiaNegocio
    {
        public decimal CalcularPrecioProducto(decimal costo)
        {
            return costo * 1.50m;
        }

        public decimal CalcularDescuentoVenta(decimal subTotal, int totalCategorias)
        {
            if (totalCategorias == 1)
            {
                return subTotal * 0.30m;
            }
            return 0m;
        }

        public (bool EsValido, string? MensageError) ValidarStockParaVenta(int stockActual, int cantidadVenta)
        {
            if (stockActual < cantidadVenta)
            {
                return (false, $"Stock insuficiente. Disponible: {stockActual}, Solicitado: {cantidadVenta}");
            }

            return (true, null);
        }

    }
}
