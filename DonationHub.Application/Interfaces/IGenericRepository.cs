using System;
using DonationHub.Domain;

namespace DonationHub.Application.Interfaces;

//? How to prevent deleting for a specific entity ?
//* Make an interface IDeletable, so when i need to delete for a repo .. it will implement the IDeletable Interface ! (Interface Principle from SOLID)

//? if it is a Generic BaseEntity<> (because the entity has a different id types) what type should be passed in ??
public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
    
    //? Why returning Task<T> instead of T ?
    //* For non-blocking to our Application (Async / Await uses the Threading Pool instead of making a manual thread !)
    Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
}
