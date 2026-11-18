using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos_express.Domain.Entities
{
    public class CodigoBarras
    {
        public int IdCodigoBarra { get; set; }
        public string UniqueCodigo { get; set; }
        public bool Activo { get; set; } = true;
        public int IdProducto { get; set; }
        
        public CodigoBarras()
        {
            UniqueCodigo = $"{DateTime.UtcNow:yyMMdd}{Random.Shared.Next(1000, 9999)}";
        }
    }
}
