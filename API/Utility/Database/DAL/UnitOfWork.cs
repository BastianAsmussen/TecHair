using API.Utility.Database.Models;

namespace API.Utility.Database.DAL;

public sealed class UnitOfWork : IDisposable
{
    private readonly DataContext _context = new();

    private GenericRepository<Authorization>? _authorizationRepository;
    public GenericRepository<Authorization> AuthorizationRepository => _authorizationRepository ??= new GenericRepository<Authorization>(_context);

    private GenericRepository<User>? _userRepository;
    public GenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);
    
    private GenericRepository<Employee>? _employeeRepository;
    public GenericRepository<Employee> EmployeeRepository => _employeeRepository ??= new GenericRepository<Employee>(_context);
    
    private GenericRepository<Appointment>? _appointmentRepository;
    public GenericRepository<Appointment> AppointmentRepository => _appointmentRepository ??= new GenericRepository<Appointment>(_context);
    
    private GenericRepository<Product>? _productRepository;
    public GenericRepository<Product> ProductRepository => _productRepository ??= new GenericRepository<Product>(_context);
    
    private GenericRepository<Price>? _priceRepository;
    public GenericRepository<Price> PriceRepository => _priceRepository ??= new GenericRepository<Price>(_context);

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed;

    private async void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) await _context.DisposeAsync();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
