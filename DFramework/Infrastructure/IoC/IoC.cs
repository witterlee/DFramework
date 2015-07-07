

using System;
using System.Collections.Generic;
using DFramework.Utilities;

namespace DFramework
{
    public class IoC
    {
        private static IDependencyResolver _innerResolver;
        private static IDependencyResolver InnerResolver
        {
            get
            {
                if (_innerResolver == null)
                    throw new DFrameworkExcepiton("Framework must config Ioc first.");

                return _innerResolver;
            }
        }


        public static void InitializeWith(IDependencyResolverFactory resolverFactory)
        {
            Check.Argument.IsNotNull(resolverFactory, "resolverFactory");
            _innerResolver = resolverFactory.CreateInstance();
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeInterface">来源类型</param>
        /// <param name="to">注册类型</param>
        /// <param name="lifeStyle">生命周期</param>        
        public static void Register(Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            InnerResolver.Register(typeImpl, lifeStyle);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeInterface">来源类型</param>
        /// <param name="typeImpl">注册类型</param>
        /// <param name="lifeStyle">生命周期</param>

        public static void Register(Type typeInterface, Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            InnerResolver.Register(typeInterface, typeImpl, lifeStyle);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeInterface">来源类型</param>
        /// <param name="typeImpl">注册类型</param>
        /// <param name="name">注册名称</param>
        /// <param name="lifeStyle">生命周期</param>

        public static void Register(Type typeInterface, Type typeImpl, string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            InnerResolver.Register(typeInterface, typeImpl, name);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TImpl">实现类型</typeparam>
        /// <param name="lifeStyle">生命周期</param>
        public static void Register<TInterface, TImpl>(LifeStyle lifeStyle = LifeStyle.Transient)
        {
            InnerResolver.Register<TInterface, TImpl>(lifeStyle);
        }


        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TImpl">实现类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <param name="lifeStyle">生命周期</param>
        public static void Register<TInterface, TImpl>(string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            InnerResolver.Register<TInterface, TImpl>(name, lifeStyle);
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="lifeStyle">生命周期</param>        
        public static void Register<T>(LifeStyle lifeStyle = LifeStyle.Transient) where T : class
        {
            InnerResolver.Register<T>(lifeStyle);
        }

        /// <summary>
        /// 注册对象(单例)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">对象实例</param>        
        public static void Register<T>(T instance) where T : class
        {
            if (typeof(Type) == typeof(T))
                throw new IoCException("not allow register T is System.Type,Do you want to use IoC.Register(type, LifeStyle.Transient);");
            InnerResolver.Register(instance);
        }
        /// <summary>
        /// 注册对象(单例)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">对象实例</param>
        /// <param name="name">对象名称</param>

        public static void Register<T>(T instance, string name) where T : class
        {
            InnerResolver.Register(instance, name);
        }

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>

        public static T Resolve<T>()
        {
            return InnerResolver.Resolve<T>();
        }
        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <returns></returns>

        public static T Resolve<T>(string name)
        {
            return InnerResolver.Resolve<T>(name);
        }

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        public static T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return InnerResolver.Resolve<T>(parameters);
        }

        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        public static T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return InnerResolver.Resolve<T>(name, parameters);
        }

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        public static T Resolve<T>(Dictionary<string, object> parameters)
        {
            return InnerResolver.Resolve<T>(parameters);
        }

        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        public static T Resolve<T>(string name, Dictionary<string, object> parameters)
        {
            return InnerResolver.Resolve<T>(name, parameters);
        }

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>

        public static object Resolve(Type type)
        {
            return InnerResolver.Resolve(type);
        }
        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <returns></returns>

        public static object Resolve(Type type, string name)
        {
            return InnerResolver.Resolve(type, name);
        }

        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>

        public static object Resolve(Type type, params KeyValuePair<string, object>[] parameters)
        {
            return InnerResolver.Resolve(type, parameters);
        }


        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns> 

        public static object Resolve(Type type, string name, params KeyValuePair<string, object>[] parameters)
        {
            return InnerResolver.Resolve(type, name, parameters);
        }
        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>

        public static object Resolve(Type type, Dictionary<string, object> parameters)
        {
            return InnerResolver.Resolve(type, parameters);
        }


        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns> 

        public static object Resolve(Type type, string name, Dictionary<string, object> parameters)
        {
            return InnerResolver.Resolve(type, name, parameters);
        }
        /// <summary>
        /// 返回容器中已注册的所有该类型实例
        /// 不同IOC容器的ResolveAll返回的不一样，请注意特定IOC的返回实现
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>IEnumerable.T</returns>

        public static IEnumerable<T> ResolveAll<T>()
        {
            return InnerResolver.ResolveAll<T>();
        }

        /// <summary>
        /// 返回容器中已注册的所有该类型实例
        /// 不同IOC容器的ResolveAll返回的不一样，请注意特定IOC的返回实现
        /// </summary>
        /// <param name="type">对象类型</typeparam>
        /// <returns></returns>

        public static IEnumerable<object> ResolveAll(Type type)
        {
            return InnerResolver.ResolveAll(type);
        }


        public static void Clear()
        {
            if (InnerResolver != null)
            {
                InnerResolver.Dispose();
            }
        }
    }
}
