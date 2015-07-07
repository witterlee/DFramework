using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using DFramework.Utilities;

namespace DFramework.Autofac
{
    public class AutofacDependencyResolver : DisposableResource, IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacDependencyResolver() : this(new ContainerBuilder().Build()) { }

        public AutofacDependencyResolver(IContainer container)
        {
            Check.Argument.IsNotNull(container, "container");

            this._container = container;
        }


        public void Register<T>(LifeStyle lifeStyle = LifeStyle.Transient) where T : class
        {
            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType<T>();

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register(Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeImpl, "typeImpl");

            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType(typeImpl);

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register(Type typeInterface, Type typeImpl, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeInterface, "typeInterface");
            Check.Argument.IsNotNull(typeImpl, "typeImpl");

            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType(typeImpl).As(typeInterface);

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register(Type typeInterface, Type typeImpl, string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotNull(typeInterface, "typeInterface");
            Check.Argument.IsNotNull(typeImpl, "typeImpl");

            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType(typeImpl).Named(name, typeInterface);

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register<TInterface, TImpl>(LifeStyle lifeStyle = LifeStyle.Transient)
        {
            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType(typeof(TImpl)).As(typeof(TInterface));

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register<TInterface, TImpl>(string name, LifeStyle lifeStyle = LifeStyle.Transient)
        {
            Check.Argument.IsNotEmpty(name, "name");
 
            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterType(typeof(TImpl)).As(typeof(TInterface));

            RegisterConfrimLifeStyle(registerBuilder, lifeStyle);

            builder.Update(_container);
        }

        public void Register<T>(T instance) where T : class
        {
            Check.Argument.IsNotNull(instance, "instance");

            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterInstance(instance);

            builder.Update(_container);
        }

        public void Register<T>(T instance, string name) where T : class
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotEmpty(name, "name");
 
            var builder = new ContainerBuilder();

            var registerBuilder = builder.RegisterInstance(instance).Named(name, typeof(T));

            builder.Update(_container);
        }

        public T Resolve<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");
 
            try
            {
                return _container.ResolveNamed<T>(name);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToParameters(parameters);
                    return _container.Resolve<T>(_params);
                }
                return _container.Resolve<T>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            Check.Argument.IsNotEmpty(name, "name");
 
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToParameters(parameters);
                    return _container.ResolveNamed<T>(name, _params);
                }
                return _container.ResolveNamed<T>(name);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(Dictionary<string, object> parameters)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToParameters(parameters);
                    return _container.Resolve<T>(_params);
                }
                return _container.Resolve<T>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public T Resolve<T>(string name, Dictionary<string, object> parameters)
        {
            Check.Argument.IsNotEmpty(name, "name");
 
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    var _params = this.ConvertToParameters(parameters);
                    return _container.ResolveNamed<T>(name, _params);
                }
                return _container.ResolveNamed<T>(name);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                return _container.Resolve(type);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, string name)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");
 
            try
            {
                return _container.ResolveNamed(name, type);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, params KeyValuePair<string, object>[] parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    var _params = this.ConvertToParameters(parameters);
                    return _container.Resolve(type, _params);
                }
                return _container.Resolve(type);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }

        public object Resolve(Type type, string name, params KeyValuePair<string, object>[] parameters)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");
 
            if (parameters != null && parameters.Length > 0)
            {
                var _params = this.ConvertToParameters(parameters);
                return _container.ResolveNamed(name, type, _params);
            }
            return _container.Resolve(type);
        }

        public object Resolve(Type type, Dictionary<string, object> parameters)
        {
            Check.Argument.IsNotNull(type, "type");

            if (parameters != null && parameters.Count > 0)
            {
                var _params = this.ConvertToParameters(parameters);
                return _container.Resolve(type, _params);
            }
            return _container.Resolve(type);
        }

        public object Resolve(Type type, string name, Dictionary<string, object> parameters)
        {
            Check.Argument.IsNotNull(type, "type");
            Check.Argument.IsNotEmpty(name, "name");
 
            if (parameters != null && parameters.Count > 0)
            {
                var _params = this.ConvertToParameters(parameters);
                return _container.ResolveNamed(name, type, _params);
            }
            return _container.Resolve(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            Check.Argument.IsNotNull(type, "type");

            try
            {
                var _type = typeof(IEnumerable<>).MakeGenericType(type);

                return (IEnumerable<object>)_container.Resolve(type);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }
        /// <summary>
        /// Autofac返回的是所有非Named和Keyed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            try
            {
                return _container.Resolve<IEnumerable<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new IoCException(ex);
            }
        }


        [DebuggerStepThrough]
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _container.Dispose();
                }

                base.Dispose(disposing);
                _disposed = true;
            }
        }

        private bool _disposed;


        #region 生命周期注册
        private void RegisterConfrimLifeStyle(IRegistrationBuilder<object, object, object> confrimStyle, LifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case LifeStyle.Transient:
                    confrimStyle.InstancePerDependency();
                    break;
                case LifeStyle.Singleton:
                    confrimStyle.SingleInstance();
                    break;
                case LifeStyle.PerHttpRequest:
                    confrimStyle.InstancePerLifetimeScope();
                    break;
                case LifeStyle.PerThread:
                    confrimStyle.InstancePerLifetimeScope();
                    break;
                default:
                    confrimStyle.SingleInstance();
                    break;
            }
        }
        #endregion

        #region 参数类型转换
        private List<Parameter> ConvertToParameters(params KeyValuePair<string, object>[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                var _params = new List<Parameter>();
                foreach (var param in parameters)
                {
                    _params.Add(new NamedParameter(param.Key, param.Value));
                }
                return _params;
            }
            return null;
        }

        private List<Parameter> ConvertToParameters(Dictionary<string, object> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                var _params = new List<Parameter>();
                foreach (var param in parameters)
                {
                    _params.Add(new NamedParameter(param.Key, param.Value));
                }
                return _params;
            }
            return null;
        }

        #endregion
    }
}
