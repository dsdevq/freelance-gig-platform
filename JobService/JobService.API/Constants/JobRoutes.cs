namespace JobService.API.Constants;

public static class JobRoutes
{
    public const string Base = "/api/jobs";
    public const string GetAll = "/";
    public const string GetById = "/{id:guid}";
    public const string GetByClientId = "/client/{clientId:guid}";
    public const string GetByFreelancerId = "/freelancer/{freelancerId:guid}";
    public const string GetByStatus = "/status/{status}";
    public const string Create = "/";
    public const string Update = "/{id:guid}";
    public const string Delete = "/{id:guid}";
}
