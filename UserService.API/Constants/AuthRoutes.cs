namespace UserService.API.Constants;

public static class AuthRoutes
{
    private const string Base = "/auth";
    public const string RegisterClient = $"{Base}/register-client";
    public const string RegisterFreelancer = $"{Base}/register-freelancer";
    public const string Login = $"{Base}/login";
    public const string Refresh = $"{Base}/refresh";
}