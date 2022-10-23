namespace webApiMessenger.Domain.Entities;

/// <summary>
/// Абстрактная сущность, для инкапсульрования типа Id
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}