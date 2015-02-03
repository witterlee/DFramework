
namespace DFramework
{
    using System.Collections.Generic;
    using System;
    /// <summary>
    /// 依赖反转接口
    /// </summary>
    public interface IDependencyResolver : IDisposable
    {
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeImpl">注册类型（如实现类）</param>
        /// <param name="lifeStyle">生命周期</param>
        void Register(Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient);
        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeInterface">来源类型(如接口类)</param>
        /// <param name="typeImpl">注册类型（如实现类）</param>
        /// <param name="lifeStyle">生命周期</param>
        void Register(Type typeInterface, Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="typeInterface">来源类型(如接口类)</param>
        /// <param name="typeImpl">注册类型（如实现类）</param>
        /// <param name="name">注册名称</param>
        /// <param name="lifeStyle">生命周期</param>
        void Register(Type typeInterface, Type typeImpl, string name, LifeStyle lifeStyle = LifeStyle.Transient);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TImpl">实现类型</typeparam>
        /// <param name="lifeStyle">生命周期</param>
        void Register<TInterface, TImpl>(LifeStyle lifeStyle = LifeStyle.Transient);

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <typeparam name="TImpl">实现类型</typeparam>
        /// <param name="name">注册名称</param>
        /// <param name="lifeStyle">生命周期</param>
        void Register<TInterface, TImpl>(string name, LifeStyle lifeStyle = LifeStyle.Transient);

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="lifeStyle">生命周期</param>
        void Register<T>(LifeStyle lifeStyle = LifeStyle.Transient) where T : class;

        /// <summary>
        /// 注册对象(单例)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">对象实例</param>
        void Register<T>(T instance) where T : class;

        /// <summary>
        /// 注册对象(单例)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="instance">对象实例</param>
        /// <param name="name">对象名称</param>
        void Register<T>(T instance, string name) where T : class;

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <returns></returns>
        T Resolve<T>(string name);

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        T Resolve<T>(params KeyValuePair<string, object>[] parameters);

        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters);

        /// <summary>
        /// 返回对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        T Resolve<T>(Dictionary<string, object> parameters);

        /// <summary>
        /// 返回容器中已注册的、指定名称的对象实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数</param>
        /// <returns></returns>
        T Resolve<T>(string name, Dictionary<string, object> parameters);

        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 返回容器中已注册的指定名称对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <returns></returns>
        object Resolve(Type type, string name);

        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>
        object Resolve(Type type, params KeyValuePair<string, object>[] parameters);


        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>
        object Resolve(Type type, string name, params KeyValuePair<string, object>[] parameters);

        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>
        object Resolve(Type type, Dictionary<string, object> parameters);


        /// <summary>
        /// 返回容器中已注册的对象实例
        /// </summary> 
        /// <param name="type">对象类型</param>
        /// <param name="name">对象名称</param>
        /// <param name="parameters">构造函数参数键值对</param>
        /// <returns></returns>
        object Resolve(Type type, string name, Dictionary<string, object> parameters);

        /// <summary>
        /// 返回容器中已注册的所有该类型实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>IEnumerable.T</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 返回容器中已注册的所有该类型实例
        /// </summary>
        /// <param name="type">对象类型</typeparam>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type type);
    }
}
