using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace webApiMessenger.BalzorClient.Services;

public static class SignInOrUpServiceExtensions
{
    public static async Task<string?> SignIn(
        this SignInOrUpService userService, 
        string login, 
        string password, 
        ILocalStorageService localStorage, 
        ISessionStorageService sessionStorage, 
        bool persistToken = false)
    {
        var jwt = await userService.SignIn(login, password);
        if (jwt == null) return null;

        if (persistToken)
        {
            await localStorage.SetItemAsStringAsync("jwt", jwt);
        }
        else
        {
            await sessionStorage.SetItemAsStringAsync("jwt", jwt);
        }

        return jwt;
    }

    public static async Task<string?> SignUp(
        this SignInOrUpService userService,
        string login,
        string password,
        ILocalStorageService localStorage,
        ISessionStorageService sessionStorage,
        bool persistToken = false)
    {
        throw new NotImplementedException();
    }
}