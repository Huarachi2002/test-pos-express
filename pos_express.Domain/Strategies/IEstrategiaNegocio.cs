using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Domain.Strategies
{
    public interface IEstrategiaNegocio
    {
        decimal CalcularPrecioProducto(decimal costo);
        decimal CalcularDescuentoVenta(decimal subTotal, int totalCategorias);
        (bool EsValido, string? MensageError) ValidarStockParaVenta(int stockActual, int cantidadVenta);
    }
}
