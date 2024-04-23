using System.Threading.Channels;

using MediatR;

using Microsoft.Extensions.Logging;

namespace ReduxSharp.Core;

public sealed class Dispatcher : IDisposable
{
    private readonly Channel<Action> _mediatrQueue;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IMediator _mediator;
    private readonly ILogger<Dispatcher> _logger;

    public Dispatcher(IMediator mediator, ILogger<Dispatcher> logger)
    {
        var options = new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _mediatrQueue = Channel.CreateBounded<Action>(options);
        _cancellationTokenSource = new();

        Task.Run(async () => await BackgroundProcessingAsync(_cancellationTokenSource.Token).ConfigureAwait(false));
        _mediator = mediator;
        _logger = logger;
    }

    public async Task DispatchAsync<TAction>(TAction action)
        where TAction : notnull
    {
        await _mediatrQueue.Writer.WriteAsync(DispatchToMediator, _cancellationTokenSource.Token).ConfigureAwait(false);

        void DispatchToMediator()
        {
            var actionType = action.GetType();
            var props = actionType.GetGenericArguments();
            var stateProp = props[0];
            var wrapperType = typeof(StateChangeRequest<,>).MakeGenericType(actionType, stateProp);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException("unable to create request wrapper");
            _mediator.Send((IBaseRequest)wrapper, _cancellationTokenSource.Token);
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Don't want to hang program on one exception")]
    private async Task BackgroundProcessingAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var dispatchAction = await _mediatrQueue.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                dispatchAction();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to dispatch action to mediatr");
            }
        }
    }
}