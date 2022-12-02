﻿using ControleFinanceiro.Domain.Models.Enums;
using Domain.Abstractions;

namespace ControleFinanceiro.Domain.Models;

public class TransactionViewModel : IEntity
{
    public int Id { get; set; }
    public string Description { get; set; }
    public float Value { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
}