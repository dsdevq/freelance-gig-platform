﻿using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Common.Interfaces;


namespace UserService.Application;

public static class DependencyInjection
{
   public static void AddApplication(this IServiceCollection services)
   {
      services.AddScoped<IUserService, Services.UserService>();
   }
}