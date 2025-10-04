namespace UserService.API.Constants;

public static class UserRoutes
{
    private const string Base = "/users";
    private const string Freelancer = $"{Base}/freelancer";
    private const string Client = $"{Base}/client";
    
    public const string ClientMe = $"{Client}/me";
    public const string FreelancerMe = $"{Freelancer}/me";
}