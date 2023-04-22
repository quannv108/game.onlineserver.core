using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Repository;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Qna.Game.OnlineServer.Game.Data;

public class GameDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Game, long> _gameRepository;

    public GameDataSeedContributor(IRepository<Game, long> gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateOrUpdateGamesAsync();
    }

    private async Task CreateOrUpdateGamesAsync()
    {
        var games = GetGames();
        var needUpdateGames = new List<Game>();
        var needInsertGames = new List<Game>();
        foreach (var game in games)
        {
            var existedGame =
                await _gameRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Type == game.Type);
            if (existedGame != null && existedGame.Equals(game))
            {
                existedGame.Name = game.Name;
                existedGame.IsActive = game.IsActive;
                
                needUpdateGames.Add(existedGame);
            }else if (existedGame == null)
            {
                needInsertGames.Add(game);
            }
        }

        if (needUpdateGames.Count != 0)
        {
            await _gameRepository.UpdateManyAsync(needUpdateGames);
        }

        if (needInsertGames.Count != 0)
        {
            await _gameRepository.InsertManyAsync(needInsertGames);
        }
    }

    private static List<Game> GetGames()
    {
        return new List<Game>()
        {
            new Game
            {
                Type = GameType.TicTacToe,
                Name = "TicTacToe",
                IsActive = true
            }
        };
    }
}