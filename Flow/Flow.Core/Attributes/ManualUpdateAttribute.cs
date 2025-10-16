using System;
using Flow.Core.Model;

namespace Flow.Core.Attributes
{
    public class ManualUpdateAttribute : Attribute
    {
        public UpdateType UpdateType { get; }
        
        public ManualUpdateAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }
    }
}