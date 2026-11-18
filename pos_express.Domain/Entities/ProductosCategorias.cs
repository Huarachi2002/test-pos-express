using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Domain.Entities
{
    public class ProductosCategorias
    {
        public int IdDetalle { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
    }
}
