using pos_express.Domain.Entities;
using pos_express.Domain.Interfaces;
using pos_express.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace pos_express.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IRepository<ExpProductos>? _expProductos;
        private IRepository<ErpProductos>? _erpProductos;
        private IRepository<Categorias>? _categorias;
        private IRepository<ProductosCategorias>? _productosCategorias;
        private IRepository<CodigoBarras>? _codigosBarras;
        private IRepository<TiposProducto>? _tiposProducto;
        private IRepository<VentaExpress>? _ventaExpress;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<ExpProductos> ExpProductos =>
            _expProductos ??= new Repository<ExpProductos>(_context);

        public IRepository<ErpProductos> ErpProductos =>
            _erpProductos ??= new Repository<ErpProductos>(_context);

        public IRepository<Categorias> Categorias =>
            _categorias ??= new Repository<Categorias>(_context);

        public IRepository<ProductosCategorias> ProductosCategorias =>
            _productosCategorias ??= new Repository<ProductosCategorias>(_context);

        public IRepository<CodigoBarras> CodigosBarras =>
            _codigosBarras ??= new Repository<CodigoBarras>(_context);

        public IRepository<TiposProducto> TiposProducto =>
            _tiposProducto ??= new Repository<TiposProducto>(_context);

        public IRepository<VentaExpress> VentaExpress =>
            _ventaExpress ??= new Repository<VentaExpress>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
