using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Log(Level level, Object message)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Logger.Log(null, level, message, null);
        }
    }
}
