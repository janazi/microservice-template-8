using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceTemplate.Domain.Entities;

public abstract class BaseEntity<TKey> : IBaseEntity<TKey> where TKey : struct, IConvertible
{
    [Column(Order = 0)]
    public TKey Id { get; set; }

    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UId { get; set; }

    /// <summary>
    /// Automatically set by Interceptor
    /// </summary>
    [Column(Order = 2)]
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Automatically set by Interceptor
    /// </summary>
    [Column(Order = 3)]
    public DateTime? DateModified { get; set; }

    [Column(Order = 4)]
    public Guid ModifiedBy { get; set; }

    [Column(Order = 5)]
    public bool IsDeleted { get; set; }
}