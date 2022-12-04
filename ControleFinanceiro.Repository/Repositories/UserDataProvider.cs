using ControleFinanceiro.Domain;
using Domain.Abstractions;
using Repository.Abstractions;
using Utils;

namespace ControleFinanceiro.Repository.Repositories;

public sealed class UserDataProvider : IUserDataProvider
{
    private readonly IUserRepository _repository;
    private List<IDataProviderItem> _items = new();

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

}