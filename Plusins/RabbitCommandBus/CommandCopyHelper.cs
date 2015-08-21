using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DFramework.DynamicReflection;

namespace DFramework.RabbitCommandBus
{
    public class CommandCopyHelper
    {
        private static readonly ConcurrentDictionary<Type, IDictionary<string, Proc<object, object>>> TypePropertySetterMapping = new ConcurrentDictionary<Type, IDictionary<string, Proc<object, object>>>();
        private static readonly ConcurrentDictionary<Type, IDictionary<string, DynamicReflection.Func<object, object>>> TypePropertyGetterMapping = new ConcurrentDictionary<Type, IDictionary<string, DynamicReflection.Func<object, object>>>();
        public static bool Copy(ICommand commandSource, ICommand commandTarget)
        {
            var sourceType = commandSource.GetType();
            if (commandSource.GetType() != commandTarget.GetType()) return false;

            IDictionary<string, Proc<object, object>> propertySetterMaps;
            IDictionary<string, DynamicReflection.Func<object, object>> propertyGetterMaps;

            var isRegisterSetter = TypePropertySetterMapping.TryGetValue(sourceType, out propertySetterMaps);
            var isRegisterGetter = TypePropertyGetterMapping.TryGetValue(sourceType, out propertyGetterMaps);

            if (!isRegisterSetter || !isRegisterGetter)
            {
                var properties = sourceType.GetProperties();
                var propSetterAction = new Dictionary<string, Proc<object, object>>();
                var propGetterAction = new Dictionary<string, DynamicReflection.Func<object, object>>();

                foreach (var p in properties)
                {
                    try
                    {
                        var setter = Dynamic<object>.Instance.Property<object>.Explicit.Setter.CreateDelegate(p);

                        if (setter != null) propSetterAction.Add(p.Name, setter);

                        var getter = Dynamic<object>.Instance.Property<object>.Explicit.Getter.CreateDelegate(p);
                        if (getter != null) propGetterAction.Add(p.Name, getter);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("reflect command getter/setter exception", ex);
                    }
                }
                propertySetterMaps = propSetterAction;
                propertyGetterMaps = propGetterAction;
                TypePropertySetterMapping.AddOrUpdate(sourceType, propSetterAction, (t, p) => propSetterAction);
                TypePropertyGetterMapping.AddOrUpdate(sourceType, propGetterAction, (t, p) => propGetterAction);
            }

            if (propertySetterMaps != null)
            {
                foreach (var pSetter in propertySetterMaps)
                {
                    DynamicReflection.Func<object, object> pGetter;

                    if (propertyGetterMaps != null && propertyGetterMaps.TryGetValue(pSetter.Key, out pGetter))
                    {
                        try
                        {
                            pSetter.Value.Invoke(commandTarget, pGetter.Invoke(commandSource));
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("invoke command getter/setter exception", ex);
                            return false;
                        }
                    }

                }
            }

            return true;
        }
    }
}
