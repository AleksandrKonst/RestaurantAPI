namespace Restaurant.AuditLogger;

public class NewClientMessage {
    public string Code { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}