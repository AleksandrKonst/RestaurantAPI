namespace Restaurant.Messages;

public class NewClientMessage {
    public string Code { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class NewClientPriceMessage : NewClientMessage {
    public int Price { get; set; }
    public string CurrencyCode { get; set; }

    public NewClientPriceMessage() {
    }

    public NewClientPriceMessage(NewClientMessage client, int price, string currencyCode) {
        this.Code = client.Code;
        this.Name = client.Name;
        this.Number = client.Number;
        this.Price = price;
        this.CurrencyCode = currencyCode;
    }
}