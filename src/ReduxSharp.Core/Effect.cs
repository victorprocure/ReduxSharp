using MediatR.Pipeline;

using Microsoft.Extensions.DependencyInjection;

namespace ReduxSharp.Core;
public abstract class Effect<TAction> : IRequestPreProcessor<TAction>
    where TAction:notnull
{
    private readonly IServiceProvider _serviceProvider;

    protected Effect(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Process(TAction request, CancellationToken cancellationToken)
    {
        var dispatcher = _serviceProvider.GetRequiredService<Dispatcher>();

        await ApplyEffectAsync(request, dispatcher, cancellationToken).ConfigureAwait(false);
    }

    protected abstract Task ApplyEffectAsync(TAction action, Dispatcher dispatcher, CancellationToken cancellationToken = default);
}