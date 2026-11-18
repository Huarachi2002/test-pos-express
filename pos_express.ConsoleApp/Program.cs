using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using pos_express.Application.Services;
using pos_express.Application.Strategies;
using pos_express.Domain.Entities;
using pos_express.Domain.Interfaces;
using pos_express.Domain.Strategies;
using pos_express.Infrastructure.Data;
using pos_express.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=FarmacorpPos;Trusted_Connection=True;TrustServerCertificate=True";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

string estrategiaConfig = builder.Configuration["EstrategiaNegocio"] ?? "Base";

if (estrategiaConfig.Equals("GanaMax", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IEstrategiaNegocio, EstrategiaGanaMax>();
}
else
{
    builder.Services.AddScoped<IEstrategiaNegocio, EstrategiaBase>();
}

builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<CodigoBarrasService>();
builder.Services.AddScoped<VentaService>();

using IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
    var productoService = services.GetRequiredService<ProductoService>();
    var categoriaService = services.GetRequiredService<CategoriaService>();
    var codigoBarrasService = services.GetRequiredService<CodigoBarrasService>();
    var ventaService = services.GetRequiredService<VentaService>();

    if (!Console.IsOutputRedirected)
    {
        Console.Clear();
    }
    Console.WriteLine("===============================================");
    Console.WriteLine("SISTEMA FARMACORP POS EXPRESS");
    Console.WriteLine("==============================================");

    await context.Database.EnsureCreatedAsync();
    Console.WriteLine("Base de Datos lista");

    var tipoExistente = await unitOfWork.TiposProducto.GetByIdAsync(1);
    if (tipoExistente == null)
    {
        await unitOfWork.TiposProducto.AddAsync(new TiposProducto { Descripcion = "Medicamentos y Farmacia" });
        await unitOfWork.CompleteAsync();
    }

    // Registrar Producto ERP
    Console.WriteLine("\nRegistro de Nuevo Producto ERP");
    decimal costoInicial = 100.00m;
    int stockInicial = 50;
    Console.WriteLine($"Registrando 'Paracetamol 500mg' con Costo: {costoInicial:F2} Bs. y Stock: {stockInicial}");

    int idProducto = await productoService.RegistrarProductoErp(
        nombre: "Paracetamol 500mg",
        costo: costoInicial,
        stock: stockInicial,
        idTipoProducto: 1,
        fechaVencimiento: DateTime.UtcNow.AddYears(2)
    );

    var productoExp = await unitOfWork.ExpProductos.GetByIdAsync(idProducto);
    var productoErp = await unitOfWork.ErpProductos.GetByIdAsync(idProducto);

    Console.WriteLine($"Producto creado exitosamente");
    Console.WriteLine($"IdProducto: {productoExp!.IdProducto}");
    Console.WriteLine($"Nombre: {productoExp.Nombre}");
    Console.WriteLine($"Costo ERP: {productoErp!.Costo:F2} Bs.");
    Console.WriteLine($"Precio Calculado (Estrategia {estrategiaConfig}): {productoExp.Precio:F2} Bs.");
    Console.WriteLine($"UniqueCodigo: {productoErp.UniqueCodigo}");

    // Registrar Codigo de Barras
    Console.WriteLine("\nAsignacion de codigo de barras");
    var barra = await codigoBarrasService.AsignarCodigoBarra(idProducto);
    Console.WriteLine($"Codigo de Barra Asignado:");
    Console.WriteLine($"IdCodigoBarra: {barra.IdCodigoBarra}");
    Console.WriteLine($"Codigo de Barra Autogenerado: {barra.UniqueCodigo}");

    // Asignacion de Categoria
    Console.WriteLine("\nAsignacion de categoria al Producto");
    int idCategoria = await categoriaService.RegistrarCategoria("Analgesicos");
    await categoriaService.AsignarCategoriaProducto(idProducto, idCategoria);
    Console.WriteLine($"Categoria 'Analgesicos' asignada al producto");

    // Registro Venta
    Console.WriteLine("\nRegistro de Venta Express");
    int cantidadVenta = 5;
    string cliente = "Carlos Fernandez";

    int idVenta = await ventaService.RegistrarVenta(
        idProducto: idProducto,
        fecha: DateTime.UtcNow,
        Cliente: cliente,
        cantidad: cantidadVenta
    );

    var venta = await unitOfWork.VentaExpress.GetByIdAsync(idVenta);
    var erpActualizado = await unitOfWork.ErpProductos.GetByIdAsync(idProducto);

    Console.WriteLine($"Venta Realizada Exitosamente (Id{venta!.Id})");
    Console.WriteLine($"Cliente: {venta.Cliente}");
    Console.WriteLine($"Producto: {venta.Producto}");
    Console.WriteLine($"UniqueCodigo registrado en venta: {venta.UniqueProducto}");
    Console.WriteLine($"Cantidad: {venta.Cantidad}");
    Console.WriteLine($"Precio: {venta.Precio:F2} Bs.");
    Console.WriteLine($"Subtotal: {(venta.Cantidad * venta.Precio):F2} Bs.");
    Console.WriteLine($"Descuento (Estrategia {estrategiaConfig}): {venta.Descuento:F2} Bs.");
    Console.WriteLine($"Total: {venta.Total:F2} Bs.");
    Console.WriteLine($"Stock ERP Actualizado: {erpActualizado!.Stock} (Anterior: {stockInicial})");

    Console.WriteLine("\n==================================");
    Console.WriteLine("PRUEBA TERMINADA");
    Console.WriteLine("====================================");
    Console.WriteLine("\nPresiona cualquier tecla para finalizar...");
    if (!Console.IsInputRedirected)
    {
        Console.ReadKey();
    }
}
