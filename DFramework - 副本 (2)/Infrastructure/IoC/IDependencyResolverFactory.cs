
namespace DFramework
{
    /// <summary>
    /// 依赖反转工厂接口
    /// </summary>
    public interface IDependencyResolverFactory
    {
        IDependencyResolver CreateInstance();
    }
}
