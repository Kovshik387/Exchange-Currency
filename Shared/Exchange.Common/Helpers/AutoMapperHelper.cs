using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AutoMapper;

namespace Exchange.Common.Helpers;

public static class AutoMappersRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("wavechat."));

        services.AddAutoMapper(assemblies);
    }
}
