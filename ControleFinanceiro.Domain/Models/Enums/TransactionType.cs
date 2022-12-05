using System.Text.Json.Serialization;

namespace ControleFinanceiro.Domain.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Outcome
}