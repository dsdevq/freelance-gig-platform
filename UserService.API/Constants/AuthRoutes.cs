namespace UserService.API.Constants;

public static class AuthRoutes
{
    private const string Base = "/auth";
    private const string Register = $"{Base}/register";
    public const string RegisterClient = $"{Register}/client";
    public const string RegisterFreelancer = $"{Register}/freelancer";
    public const string Login = $"{Base}/login";
    public const string Refresh = $"{Base}/refresh";
}