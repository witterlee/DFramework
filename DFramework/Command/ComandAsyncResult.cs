using System;

namespace DFramework
{
    /// <summary>
    /// 待返回值的命令
    /// </summary>
    public class ComandAsyncResult<TResult> : Command
    {
        public TResult CommandResult { get; set; }
    } 
}
