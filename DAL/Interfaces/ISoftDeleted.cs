using System;

namespace DAL.Interfaces
{
    public interface ISoftDeleted
    {
        DateTime? DeleteDate { get; set; }
    }
}
