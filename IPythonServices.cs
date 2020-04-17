using System.Threading.Tasks;

namespace PythonServices
{
    /// <summary>
    /// 需要在 Python 服务中供他处调用实现功能的声明
    /// </summary>
    public interface IPythonServices
    {
        //PythonServicesOptions PythonVenvPath { get; set; }
        /// <summary>
        /// 创建虚拟环境
        /// </summary>
        /// <returns>虚拟环境创建是否完成</returns>
        bool CreateVenv();
        /// <summary>
        /// 安装python模块
        /// </summary>
        /// <param name="module"></param>
        void PipInstallModule(string module);
        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();
        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();
        /// <summary>
        /// 调用指定名称 python 服务并返回结果
        /// </summary>
        /// <typeparam name="T">返回值 DTO 类型</typeparam>
        /// <param name="nameOrPath">服务名称或网络路径</param>
        /// <param name="method">服务请求方式</param>
        /// <returns></returns>
        Task<T> GetServiceResult<T>(string nameOrPath, string method = "get");
        /// <summary>
        /// 调用指定名称 python 服务并返回结果
        /// </summary>
        /// <param name="nameOrPath">服务名称或网络路径</param>
        /// <param name="method">服务请求方式</param>
        /// <returns></returns>
        Task<string> GetServiceResult(string nameOrPath, string method = "get");
    }
}
