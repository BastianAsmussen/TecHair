namespace API.Utility.Database.Models;

public class Order(Appointment appointment)
{
    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public Appointment Appointment { get; set; } = appointment;
    public List<Product> Products { get; set; } = new();
}
