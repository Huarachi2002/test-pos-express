using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Domain.Entities
{
    public class ErpProductos
    {
        public int IdProducto { get; set; }
        public decimal Costo { get; set; }
        public string UniqueCodigo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public int Stock { get; set; }

        public ErpProductos()
        {
            UniqueCodigo = $"ERP-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";
        }

    }
}
