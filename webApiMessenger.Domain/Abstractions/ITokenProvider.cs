namespace webApiMessenger.Domain.Abstractions;

public interface ITokenProvider
{
    public Task<string> GetJwt(string login);
}