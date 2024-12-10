using NLog;
using System;
using System.Windows.Markup;

namespace SignalAcquisitionDemo.Helper
{
    public class LogHelper
    {
        #region 静态成员
        /// <summary>
        /// 日志接口
        /// </summary>
        static Logger logger = null;

        static bool enable = true;
        /// <summary>
        /// 是否启用状态
        /// </summary>
        public static bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }
        #endregion

        static LogHelper()
        {
            CreateConfigFileLogger();
        }

        #region 创建基于配置文件的日志组件
        /// <summary>
        /// 根据配置文件创建日志组件实例  
        /// </summary>
        /// <param name="configfilePath">组件配置文件路径</param>
        /// <param name="repositoryName"></param>
        /// <param name="loggerName"></param>
        //public static void CreateConfigFileLogger(string configfilePath, string repositoryName, string loggerName)
        //{

        //    var repository = LogManager.CreateRepository(repositoryName);
        //    XmlConfigurator.Configure(repository, new FileInfo(configfilePath));
        //    logger = LogManager.GetLogger(repository.Name, loggerName);
        //}

        /// <summary>
        /// 根据配置文件创建日志组件实例
        /// </summary>
        /// <param name="configfilePath">组件配置文件路径</param>
        public static void CreateConfigFileLogger()
        {
            if (logger == null)
            {
                var loggesr = LogManager.LoadConfiguration("NLog.Config");
                logger = loggesr.GetCurrentClassLogger();
                //CreateConfigFileLogger(configfilePath, nameof(LogHelper), string.Empty);
            }
        }
        #endregion

        #region 输出日志方法
        /// <summary>
        /// 记录Trace信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Trace(string msg)
        {
            if (enable && logger != null)
            {
                logger.Trace(msg);
            }
        }

        /// <summary>
        /// 记录Trace信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Trace(string msg, byte[] bytes)
        {
            if (enable && logger != null && logger.IsTraceEnabled)
            {
                logger.Trace(string.Format(msg, BitConverter.ToString(bytes).Replace("-", " ")));
            }
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            if (enable && logger != null)
            {
                logger.Debug(msg);
            }
        }

        /// <summary>
        /// 记录Info信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            if (enable && logger != null)
            {
                logger.Info(msg);
            }
        }


        /// <summary>
        /// 记录Warn信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg)
        {
            if (enable && logger != null)
            {
                logger.Warn(msg);
            }
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="msg">异常信息</param>
        public static void Error(string msg)
        {
            if (enable && logger != null)
            {
                logger.Error(msg);
            }
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void Exception(Exception ex)
        {
            if (enable && logger != null)
            {
                logger.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// 记录Fatal
        /// </summary>
        /// <param name="obj"></param>
        public static void Fatal(object obj)
        {
            if (enable && logger != null)
            {
                logger.Fatal(obj);
            }
        }
        #endregion
    }
}
