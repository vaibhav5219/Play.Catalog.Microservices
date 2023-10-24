using System;

namespace Play.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
        // string Name { get; set; }
        // string Description { get; set; }
        // decimal Price { get; set; }
        // DateTimeOffset CreatedDate { get; set; }
    }
}

/*
For Building this class project

dotnet new classlib -n Play.Common --framework net6.0

dotnet add package MongoDB.Driver

dotnet add package Microsoft.Extensions.Configuration

dotnet add package Microsoft.Extensions.Configuration.Binder

dotnet add package Microsoft.Extensions.DependencyInjection
*/