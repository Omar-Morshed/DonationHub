using System;

namespace DonationHub.Domain;

public class BaseEntity<T>
{
    public T Id { get; set; }
}
