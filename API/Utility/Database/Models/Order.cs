namespace API.Utility.Database.Models;

public class Order
{
    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public Appointment Appointment { get; set; }
    public List<Product> Products { get; set; }
}
