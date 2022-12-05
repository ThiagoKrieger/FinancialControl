using Domain.Abstractions;

namespace Repository.Abstractions;

public interface IDataProvider
{
    Task<IList<IDataProviderItem>> GetItems(CancellationToken token);
}