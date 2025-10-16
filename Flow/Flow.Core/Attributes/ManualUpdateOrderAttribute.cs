using System;

namespace Flow.Core.Attributes
{
    public class ManualUpdateOrderAttribute : Attribute
    {
        public int Order { get; }

        public ManualUpdateOrderAttribute(int order)
        {
            Order = order;
        }
    }
}