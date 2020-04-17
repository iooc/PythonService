using System;
using System.Collections.Generic;
using System.Text;

namespace PythonServices.Configuration
{
    public class PythonServicesFactory
    {
        /// <summary>
        /// 创建 python 服务
        /// </summary>
        /// <param name="options">python 服务可选配置</param>
        /// <returns></returns>
        public static IPythonServices CreatePythonServices(PythonServicesOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var service = new PythonServicesImpl(options/*.NodeInstanceFactory*/);

            Console.CancelKeyPress += (sender, e) =>
            {
                service.Stop();
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                service.Stop();
            };

            return service;
        }
    }
}
