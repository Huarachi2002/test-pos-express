using pos_express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace pos_express.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ExpProductos> ExpProductos { get; }
        IRepository<ErpProductos> ErpProductos { get; }
        IRepository<Categorias> Categorias { get; }
        IRepository<ProductosCategorias> ProductosCategorias { get; }
        IRepository<CodigoBarras> CodigosBarras { get; }
        IRepository<TiposProducto> TiposProducto { get; }
        IRepository<VentaExpress> VentaExpress { get; }
        Task<int> CompleteAsync();
    }
}
