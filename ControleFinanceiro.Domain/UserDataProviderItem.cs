using Domain.Abstractions;

namespace ControleFinanceiro.Domain;

public class UserDataProviderItem : IDataProviderItem
{
    public int Id { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public string Display => Name;
}