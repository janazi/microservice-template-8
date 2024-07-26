namespace MicroserviceTemplate.Domain.Entities;

public interface IBaseEntity : IBaseEntity<int> { }

public interface IBaseEntity<TKey> : IAuditableEntity
{
    TKey Id { get; set; }
    Guid UId { get; set; }
    bool IsDeleted { get; set; }
}

public interface IAuditableEntity
{
    DateTime DateCreated { get; set; }
    DateTime? DateModified { get; set; }
    Guid ModifiedBy { get; set; }
}