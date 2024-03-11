namespace API.Controllers.DTO;

public class NewOrder
{
    public int AppointmentId { get; set; }
    public IEnumerable<int> ProductIds { get; set; }
}
