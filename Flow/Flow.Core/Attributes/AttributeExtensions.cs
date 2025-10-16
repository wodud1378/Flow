using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Core.Model;
using Flow.Core.Updates.Interfaces;

namespace Flow.Core.Attributes
{
    public static class AttributeExtensions
    {
        public static bool IsManualUpdate(this IUpdate update, UpdateType updateType = UpdateType.Update)
        {
            var type = update.GetType();
            if (!TryGetAttribute(type, out ManualUpdateAttribute attribute))
                return false;

            return attribute.UpdateType == updateType;
        }

        public static List<IUpdate> ToSortedList(this IEnumerable<IUpdate> updates)
        {
            var list = updates.ToList();
            list.Sort((x, y) =>
            {
                int xOrder = 0;
                int yOrder = 0;
                if (TryGetAttribute(x.GetType(), out ManualUpdateOrderAttribute xAttr))
                    xOrder = xAttr.Order;

                if (TryGetAttribute(y.GetType(), out ManualUpdateOrderAttribute yAttr))
                    yOrder = yAttr.Order;
                
                return xOrder.CompareTo(yOrder);
            });
            
            return list;
        }

        public static bool TryGetAttribute<T>(this Type type, out T attribute) where T : Attribute
        {
            attribute = type
                .GetCustomAttributes(typeof(T), false)
                .Cast<T>()
                .FirstOrDefault();

            return attribute != null;
        }
    }
}