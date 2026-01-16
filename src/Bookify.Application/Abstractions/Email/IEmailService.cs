namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(
        Domain.Users.ValuesObjects.Email recipient,
        string subject,
        string body);
}