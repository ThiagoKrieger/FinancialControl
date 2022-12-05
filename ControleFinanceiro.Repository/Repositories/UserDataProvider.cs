using ControleFinanceiro.Domain;
using Domain.Abstractions;
using Repository.Abstractions;
using Utils;

namespace ControleFinanceiro.Repository.Repositories;

public sealed class UserDataProvider : IUserDataProvider
{
    private readonly IUserRepository _repository;
    private readonly List<IDataProviderItem> _items = new();

    public UserDataProvider(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<IDataProviderItem>> GetItems(CancellationToken token)
    {
        var entities = await _repository.GetAsync(token);
        
        foreach (var entity in entities)
        {
            var item = entity.ProjectToNew<UserDataProviderItem>();
            
            _items.Add(item);
        }

        return _items;
    }

    public async Task<IList<IDataProviderItem>> GetItemsForMajorUsers(CancellationToken token)
    {
        var entities = await _repository.GetAsync(token);
        
        foreach (var entity in entities.Where(user => user.Age >= 18))
        {
            var item = entity.ProjectToNew<UserDataProviderItem>();
            
            _items.Add(item);
        }

        return _items;
    }
}