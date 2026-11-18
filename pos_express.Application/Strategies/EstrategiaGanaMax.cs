using pos_express.Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Application.Strategies
{
    public class EstrategiaGanaMax : IEstrategiaNegocio
    {
        public decimal CalcularDescuentoVenta(decimal subTotal, int totalCategorias)
        {
            if (totalCategorias == 1)
            {
                return subTotal * 0.10m;
            }
            return 0m;
        }

        public decimal CalcularPrecioProducto(decimal costo)
        {
            return costo * 1.80m;
        }

        public (bool EsValido, string? MensageError) ValidarStockParaVenta(int stockActual, int cantidadVenta)
        {
            if (stockActual < cantidadVenta)
            {
                return (false, $"Stock insuficiente. Disponible: {stockActual}");
            }

            int stockRestante = stockActual - cantidadVenta;
            if (stockRestante <= 10)
            {
                return (false, $"La venta no puede realizarse, limite de stock sobrepasara.");
            }

            return (true, null);
        }
    }
}
