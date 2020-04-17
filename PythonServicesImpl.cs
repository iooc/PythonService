using Newtonsoft.Json;
using PythonServices.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PythonServices
{
    /// <summary>
    /// python 服务的默认实现
    /// </summary>
    public class PythonServicesImpl : IPythonServices
    {
        PythonServicesOptions Options { get; set;}

        readonly string VenvFullPath;

        public PythonServicesImpl(PythonServicesOptions options)
        {
            Options = options;

            VenvFullPath = GetVenvFullPath();
        }

        public bool CreateVenv()
        {
            Util.RunCommand(new[] { $"python -m venv \"{VenvFullPath}\"" });
            return true;
        }

        public void PipInstallModule(string module)
        {
            if (!IsModuleInstalled(module))
                Util.RunCommand(new[] { module });
        }

        public void Start()
        {
            if (!IsVenvInstalled())
                CreateVenv();

            var activate = Path.Combine(VenvFullPath, "Scripts", "activate.bat");
            var cmds = new[] { 
                $"cd {Options.PythonAppPath} && D:",
                $"{activate}",
                $"python {Options.Startup}.py --port {Options.Port}" };
            //Util.RunCommand($"cd {Options.PythonAppPath} && D:");
            Util.RunCommand(cmds);
            Console.WriteLine("   内置 python 服务已启动！");
        }

        public void Stop()
        {
            Util.CloseCommand();
        }
        /// <summary>
        /// 获取 python 服务返回结果
        /// </summary>
        /// <typeparam name="T">DTO 模型类</typeparam>
        /// <param name="nameOrPath">名称或路径</param>
        /// <param name="method">请求方式</param>
        /// <returns></returns>
        public async Task<T> GetServiceResult<T>(string nameOrPath, string method = "get")
        {
            var http = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri($"http://localhost:{Options.Port}/{nameOrPath}")
            };

            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeObject<T>(result);

            return json;
        }

        public async Task<string> GetServiceResult(string nameOrPath, string method = "get")
        {
            var http = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri($"http://localhost:{Options.Port}/{nameOrPath}")
            };

            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        //Process PythonProcess(ProcessStartInfo startInfo, string commandName)
        //{
        //    try
        //    {
        //        var process = Process.Start(startInfo);

        //        // See equivalent comment in OutOfProcessNodeInstance.cs for why
        //        process.EnableRaisingEvents = true;

        //        return process;
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = $"Failed to start '{commandName}'. To resolve this:.\n\n"
        //                    + $"[1] Ensure that '{commandName}' is installed and can be found in one of the PATH directories.\n"
        //                    + $"    Current PATH enviroment variable is: { Environment.GetEnvironmentVariable("PATH") }\n"
        //                    + "    Make sure the executable is in one of those directories, or update your PATH.\n\n"
        //                    + "[2] See the InnerException for further details of the cause.";
        //        throw new InvalidOperationException(message, ex);
        //    }
        //}
        /// <summary>
        /// 检查模块是否已安装
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private bool IsModuleInstalled(string module)
        {
            if (!IsVenvInstalled())
                return false;

            string moduleDir = Path.Combine(VenvFullPath, "Lib", module);
            return Directory.Exists(moduleDir) && File.Exists(Path.Combine(moduleDir, "__init__.py"));
        }
        /// <summary>
        /// 检查是否已创建并安装虚拟环境
        /// </summary>
        /// <returns></returns>
        private bool IsVenvInstalled()
        {
            return File.Exists(Path.Combine(VenvFullPath, "Scripts", "pip.exe"));
        }
        /// <summary>
        /// 获取虚拟环境完整路径
        /// </summary>
        /// <returns></returns>
        private string GetVenvFullPath()
        {
            if (Options == null)
                throw new Exception("您还未为服务配置任何参数");
            if (string.IsNullOrWhiteSpace(Options.PythonVenvName))
                throw new Exception("您还未指定虚拟环境的名称");
            
            var venvpath = Path.GetFullPath($"./pyapp/{Options.PythonVenvName}");
            if (!string.IsNullOrWhiteSpace(Options.PythonVenvPath))
                venvpath = Path.Combine(
                    Path.GetFullPath(Options.PythonVenvPath),
                    Options.PythonVenvName);

            return venvpath;
        }
    }
}
