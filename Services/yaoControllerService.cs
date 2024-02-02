using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using csDemo;
public class YaoControllerService : GRPCController.GRPCControllerBase
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<YaoControllerService> _logger;

    public YaoControllerService(IHostApplicationLifetime appLifetime, ILogger<YaoControllerService> logger)
    {
        _appLifetime = appLifetime;
        _logger = logger;
    }

    public override Task<Empty> Shutdown(Empty request, ServerCallContext context)
    {
        _logger.LogInformation("Shutdown request received");
        _appLifetime.StopApplication();
        return Task.FromResult(new Empty());
    }
}
