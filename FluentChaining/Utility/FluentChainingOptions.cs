using Microsoft.Extensions.DependencyInjection;

namespace FluentChaining;

public class FluentChainingOptions
{
    public ServiceLifetime ChainLifetime { get; set; } = ServiceLifetime.Scoped;
    public bool AllowDuplicates { get; set; }
}