using Domain.Abstractions;

namespace Repository.Abstractions;

public interface IUserDataProvider : IDataProvider
{
    Task<IList<IDataProviderItem>> GetItemsForMajorUsers(CancellationToken token);
}