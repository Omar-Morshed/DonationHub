using DonationHub.Domain.Enums;
using FluentValidation;

namespace DonationHub.Application.Features.Admin.CreateStaff;

public class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty().MinimumLength(8);

        RuleFor(x => x.Request.FullName)
            .NotEmpty().MaximumLength(150);

        RuleFor(x => x.Request.PreferredLanguage)
            .Must(lang => lang is "ar" or "en")
            .WithMessage("Preferred language must be 'ar' or 'en'.");

        RuleFor(x => x.Request.Role)
            .NotEmpty()
            .Must(BeAValidStaffRole)
            .WithMessage($"Role must be a valid staff role ({string.Join(", ", GetAllowedStaffRoles())}).");
    }

    private static bool BeAValidStaffRole(string role)
    {
        // 1. نمنع إن حد يبعت رقم زي "1" بدل الاسم "Admin"
        if (int.TryParse(role, out _))
            return false;

        // 2. نحاول نحول النص للـ Enum (مع تجاهل حالة الحروف admin / Admin)
        if (!Enum.TryParse<UserRole>(role, ignoreCase: true, out var parsedRole))
            return false;

        // 3. نتأكد إنه متعرف فعلاً في الـ Enum وإنه مش Donor
        return Enum.IsDefined(parsedRole) && parsedRole != UserRole.Donor;
    }

    // دالة مساعدة بتجيب كل الأدوار ما عدا Donor تلقائياً لرسالة الخطأ
    private static IEnumerable<string> GetAllowedStaffRoles() =>
        Enum.GetValues<UserRole>()
            .Where(r => r != UserRole.Donor)
            .Select(r => r.ToString());
}
