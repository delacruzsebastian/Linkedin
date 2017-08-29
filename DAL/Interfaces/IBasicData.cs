namespace DAL.Interfaces
{
    using System;

    public interface IBasicData : IKey
    {
        DateTime AddDate { get; set; }
        DateTime UpdateDate { get; set; }
        DateTime? DeleteDate { get; set; }
    }
}
