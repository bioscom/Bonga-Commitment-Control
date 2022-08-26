using System;

/// <summary>
/// Summary description for BaseEntity
/// </summary>
public abstract class BaseEntity
{
    public Int64? ID { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public virtual void Set()
    {

    }
}
