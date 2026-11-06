using System;
using DonationHub.Domain.Enums;

namespace DonationHub.Domain;
//* Used for auditing the the Payment Attempts (failed or succeeded)
public class PaymentAttempt
{
    public Guid Id { get; private set; }
    public Guid DonationId { get; private set; }
    public string StripePaymentCheckoutSessionId { get; private set; }
    public string? StripePaymentIntentId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentAttemptStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string? FailureCode { get; private set; } //* the failure code from the stripe
}
