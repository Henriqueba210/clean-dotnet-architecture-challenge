namespace WebApi.Integration.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(
                d => d.ServiceType == typeof(IPostCodeIoService) ||
                     d.ServiceType == typeof(HybridCache)).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IPostCodeIoService>(sp => 
                PostCodeResponseBuilder.CreateMockPostCodeService().Object);

            services.AddSingleton<HybridCache>(sp => 
                HybridCacheBuilder.CreateMockCache().Object);
        });

        builder.UseEnvironment("Testing");
    }
}