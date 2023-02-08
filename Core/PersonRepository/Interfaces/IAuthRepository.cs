using Data.Model;
namespace Core.PersonRepository.Interfaces
{
    public interface IAuthRepository
    {
        string RequestToken(LoginModel login);
        string GetKey();
        string GetIssuer();
    }
}