namespace Domain.Abstractions;

public interface IDataProviderItem
{
    public int Id { get; set; }
    public string Display { get; }
}
