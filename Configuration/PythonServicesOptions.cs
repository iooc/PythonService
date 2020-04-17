using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace PythonServices.Configuration
{
    /// <summary>
    /// Python 服务的可选配置
    /// </summary>
    public class PythonServicesOptions
    {
        /// <summary>
        /// 初始化 Python 服务配置类
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PythonServicesOptions(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            //EnvironmentVariables = new Dictionary<string, string>();
        }
        /// <summary>
        /// python 运行时所属路径
        /// </summary>
        public string PythonPath { get; set; } = null;
        /// <summary>
        /// python 虚拟环境所属路径
        /// </summary>
        public string PythonVenvPath { get; set; } = null;
        /// <summary>
        /// python 应用执行路径
        /// </summary>
        public string PythonAppPath { get; set; } = null;
        /// <summary>
        /// 虚拟环境名称
        /// </summary>
        public string PythonVenvName { get; set; }
        ///// <summary>
        ///// 用户获取平台变量配置
        ///// </summary>
        //public IDictionary<string, string> EnvironmentVariables { get; set; }
        /// <summary>
        /// 应用框架类型（默认为 Flask）
        /// </summary>
        public AppFramework FrameworkType { get; set; } = AppFramework.Flask;
        /// <summary>
        /// 内部 python web 服务默认端口
        /// </summary>
        public int Port { get; set; } = 6728;
        /// <summary>
        /// python 启动文件名，默认为 app (不带扩展名)
        /// </summary>
        public string Startup { get; set; } = "app";
    }
    /// <summary>
    /// 应用框架的种类
    /// </summary>
    public enum AppFramework
    {
        Normal,
        WebNormal,
        Flask,
        Django,
        Bottle
    }
}
