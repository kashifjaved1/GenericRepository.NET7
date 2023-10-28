using Repository.Abstractions;

namespace Repository.Generic;

/// <summary>
/// Abstraction of Unit Of Work pattern
/// </summary>
public interface IUnitOfWork
{
    IRepository Repository { get; }
}