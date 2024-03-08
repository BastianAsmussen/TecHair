using API.Utility.Database.Models;

namespace API.Utility.Database.DAL;

public sealed class UnitOfWork : IDisposable
{
    private readonly DataContext _context = new();

    private GenericRepository<Appointment>? _appointmentRepository;

    private GenericRepository<Authorization>? _authorizationRepository;

    private bool _disposed;

    private GenericRepository<Employee>? _employeeRepository;

    private GenericRepository<Price>? _priceRepository;

    private GenericRepository<Product>? _productRepository;

    private GenericRepository<User>? _userRepository;

    private GenericRepository<Order>? _orderRepository;

    public GenericRepository<Authorization> AuthorizationRepository =>
        _authorizationRepository ??= new GenericRepository<Authorization>(_context);

    public GenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);

    public GenericRepository<Employee> EmployeeRepository =>
        _employeeRepository ??= new GenericRepository<Employee>(_context);

    public GenericRepository<Appointment> AppointmentRepository =>
        _appointmentRepository ??= new GenericRepository<Appointment>(_context);

    public GenericRepository<Product> ProductRepository =>
        _productRepository ??= new GenericRepository<Product>(_context);

    public GenericRepository<Price> PriceRepository => _priceRepository ??= new GenericRepository<Price>(_context);

    public GenericRepository<Order> OrderRepository => _orderRepository ??= new GenericRepository<Order>(_context);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    private async void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) await _context.DisposeAsync();

        _disposed = true;
    }
}