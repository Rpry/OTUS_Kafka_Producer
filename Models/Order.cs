using System;

namespace Producer.Models;

public class Order
{
    public long Id { get; set; }
    public OrderState State { get; set; }
    public DateTimeOffset ChangedAt { get; set; }
}