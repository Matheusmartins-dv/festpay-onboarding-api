namespace Festpay.Onboarding.Domain.Entities;

public class EntityBase
{
    public Guid Id { get; private set; }
    public DateTime CreatedUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? DeactivatedUtc { get; private set; }

    public virtual void Validate() { }

    public virtual void EnableDisable()
    {
        if (DeactivatedUtc.HasValue)
            DeactivatedUtc = null;
        else
            DeactivatedUtc = DateTime.UtcNow;
    }

    protected void RestoreBase(Guid id, DateTime createdUtc, DateTime? deactivatedUtc = null)
    {
        Id = id;
        CreatedUtc = createdUtc;
        DeactivatedUtc = deactivatedUtc;
    }
}
