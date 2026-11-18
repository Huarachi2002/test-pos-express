using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Domain.Entities
{
    public class ExpProductos
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaVencimiento { get; set; } = DateTime.UtcNow;
        public string? Observaciones { get; set; } = string.Empty;
        public int IdTipoProducto {get; set; }

        public ErpProductos? ErpProducto { get; set; }
    }
}
