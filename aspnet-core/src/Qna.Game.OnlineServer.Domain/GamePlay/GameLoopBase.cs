using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Qna.Game.OnlineServer.GamePlay;

public abstract class GameLoopBase<T, TS> : IGameLoop<T, TS>
    where T : class, IGamePlayData, new()
    where TS : struct, Enum
{
    public TS State { get; protected set; }
    public T GamePlayData { get; protected set; }

    public Guid Id => GamePlayData.Id;

    public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider =>
        LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);
    
    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    private AbpAsyncTimer _timer;
    private CancellationTokenSource _cancellationTokenSource;

    public abstract Room.Room Room { get; }
    public abstract bool IsFinished { get; }
    public abstract void Setup(Room.Room room);

    public void Start()
    {
        if (_timer != null)
        {
            throw new UserFriendlyException("game loop already start");
        }
        _timer = LazyServiceProvider.LazyGetRequiredService<AbpAsyncTimer>();
        _timer.Period = 1000 / 15; // 15 FPS // TODO: bring this to Game.Game entity
        _timer.Elapsed = Timer_Elapsed;
        _cancellationTokenSource = new CancellationTokenSource();
        _timer.Start(_cancellationTokenSource.Token);
    }

    public void AbortGame()
    {
        _timer.Stop(_cancellationTokenSource.Token);
        _cancellationTokenSource.Cancel(false);
        _timer = null;
    }

    protected void ChangeToState(TS newState)
    {
        if (State.Equals(newState))
        {
            return; // ignore if same state
        }
        var oldState = State;
        State = newState;
        Logger.LogDebug($"Game {GamePlayData.Id} change to state {newState}");
        OnGameChangeState(oldState, newState);
    }
    
    protected abstract void OnGameChangeState(TS oldState, TS newState);
    protected abstract void OnGameUpdate(int milliseconds);
    
    private Task Timer_Elapsed(AbpAsyncTimer timer)
    {
        OnGameUpdate(timer.Period);
        return Task.CompletedTask;
    }

}