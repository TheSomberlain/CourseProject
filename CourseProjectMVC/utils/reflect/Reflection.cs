using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace _1stWebApp.utils.reflect
{
    public static class Reflection
    {
        public static void UpdateEntity(dynamic entity, dynamic model)
        {
            foreach (PropertyInfo modelProp in model.GetType().GetProperties())
            {
                foreach (PropertyInfo entityProp in entity.GetType().GetProperties())
                {
                    if (entityProp.Name == modelProp.Name
                       && modelProp.GetValue(model, null) != null)
                    {
                        entityProp.SetValue(entity, modelProp.GetValue(model, null));
                    }
                }
            }
        }

        public static void IterateThroughProps(dynamic entity, Action<PropertyInfo> action)
        {
            foreach (PropertyInfo entityProp in entity.GetType().GetProperties())
            {
                action(entityProp);
            }
        }
    }
}
