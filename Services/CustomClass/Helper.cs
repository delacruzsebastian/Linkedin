using System;

namespace Services.CustomClass
{
    public static class Helper
    {
        public static bool GuidIsNullOrEmpty(Guid? value)
        {
            return !value.HasValue || value == Guid.Empty;
        }
    }
}
