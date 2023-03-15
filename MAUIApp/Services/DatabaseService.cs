using MAUIApp.Models;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace MAUIApp.Services;
public class DatabaseService
{

    private const string _databaseFileName = "DDoWDatabase.db3";
    private const SQLiteOpenFlags _flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;
    private string _databasePath => Path.Combine(FileSystem.AppDataDirectory, _databaseFileName);

    private SQLiteAsyncConnection? Database;

    public DatabaseService()
    {
    }

    [MemberNotNull(nameof(Database))]
    private async Task Init()
    {
        if (Database is not null)
        {
            return;
        }

        Database = new SQLiteAsyncConnection(_databasePath, _flags);

        await Database.CreateTableAsync<WikiArticle>();
        await Database.CreateTableAsync<Interaction>();
    }

    public async Task<IEnumerable<Interaction>> GetInteractionsAsync()
    {
        await Init();
        return await Database.Table<Interaction>().ToListAsync();
    }

    public async Task<IEnumerable<Interaction>> GetLikesAsync()
    {
        await Init();
        return await Database.Table<Interaction>().Where(x => x.Type == InteractionType.Like).ToListAsync();
    }

    public async Task<IEnumerable<Interaction>> GetDislikesAsync()
    {
        await Init();
        return await Database.Table<Interaction>().Where(x => x.Type == InteractionType.Dislike).ToListAsync();
    }

    public async Task<IEnumerable<WikiArticle>> GetCardStackAsync()
    {
        await Init();
        return await Database.Table<WikiArticle>().ToListAsync();
    }

    public async Task InsertInteraction(Interaction interaction)
    {
        await Init();
        await Database.InsertAsync(interaction);
    }

    public async Task InsertCardStackArticles(IEnumerable<WikiArticle> articles)
    {
        await Init();
        foreach (WikiArticle article in articles)
        {
            await Database.InsertOrReplaceAsync(article);
        }
    }

    public async Task DeleteCardStackArticles(IEnumerable<string> articleIds)
    {
        await Init();
        foreach (string articleId in articleIds)
        {
            await Database.DeleteAsync<WikiArticle>(articleId);
        }
    }

    public async Task ClearInteractions()
    {
        await Init();
        await Database.DeleteAllAsync<Interaction>();
    }

    public async Task ClearCardStack()
    {
        await Init();
        await Database.DeleteAllAsync<WikiArticle>();
    }
}
