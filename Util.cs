using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace PythonServices
{
    internal static class Util
    {
        static Process process = null;
        /// <summary>
        /// 运行给定的命令
        /// </summary>
        /// <param name="commands">命令字符串</param>
        /// <param name="runInBackground"></param>
        internal static void RunCommand(string[] commands, bool runInBackground = false)
        {
            var commandMode = runInBackground ? "/C" : "/K";

            if (process == null)
            {
                process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Normal
                };
                //if (runInBackground)
                //    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    startInfo.FileName = "/bin/bash";
                    commandMode = "-c";
                }
            //startInfo.Arguments = $"{commandMode} {command}";
                startInfo.Arguments = $"{commandMode}";
                process.StartInfo = startInfo;
                // 常规信息输出
                process.OutputDataReceived += (sender, e) =>
                {
                    Console.WriteLine(e.Data);
                };
                // 错误信息输出
                process.ErrorDataReceived += (sender, e) =>
                {
                    Console.WriteLine(e.Data);
                };

                process.Start();

                process.BeginOutputReadLine();
            }

            var inputreader = process.StandardInput;
            inputreader.AutoFlush = true;
            foreach (var command in commands)
            {
                inputreader.WriteLine($"{command}");
                //Console.WriteLine($"已执行命令：{command}");
            }
        }
        /// <summary>
        /// 关闭命令行进程
        /// </summary>
        internal static void CloseCommand()
        {
            if (process != null)
            {
                //process.WaitForExit();
                process.Close();

                process = null;
            }
        }
    }
}
