using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        void Info(string message);

        void Debug(string message);

        void Warn(string message);
        void Warn(string message, Exception exception);

        void Error(string message);
        void Error(string message, Exception exception);

        void Fatal(string message);
        void Fatal(string message, Exception exception);
    }
}
