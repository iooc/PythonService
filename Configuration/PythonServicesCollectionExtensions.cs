using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace PythonServices.Configuration
{
    /// <summary>
    /// 扩展 IServiceCollection 接口以增加 Python 服务功能
    /// </summary>
    public static class PythonServicesCollectionExtensions
    {
        /// <summary>
        /// 为应用添加 python 服务功能
        /// </summary>
        /// <param name="serviceCollection">服务管理集合</param>
        /// <param name="setupAction">服务的可选配置</param>
        public static void AddPythonServices(
            this IServiceCollection serviceCollection, 
            Action<PythonServicesOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            serviceCollection.AddSingleton(typeof(IPythonServices), serviceProvider =>
            {
                // First we let NodeServicesOptions take its defaults from the IServiceProvider,
                // then we let the developer override those options
                var options = new PythonServicesOptions(serviceProvider);
                setupAction(options);

                return PythonServicesFactory.CreatePythonServices(options);
            });
        }
    }
}
