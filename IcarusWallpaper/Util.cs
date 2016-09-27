using System;
using System . Collections . Generic;
using System . Diagnostics;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace IcarusWallpaper
{
    static class Util
    {
        public static bool IsAnotherInstanceExist ()
        {
            var currentProcess = Process . GetCurrentProcess ();
            var currentFileName = currentProcess . MainModule . FileName;
            var processes = Process . GetProcessesByName ( currentProcess . ProcessName );
            if (processes.Length > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Process GetRunningInstance ()
        {
            //获取当前进程
            Process currentProcess = Process . GetCurrentProcess ();
            string currentFileName = currentProcess . MainModule . FileName;
            //创建新的 Process 组件的数组，并将它们与本地计算机上共享指定的进程名称的所有进程资源关联。
            Process [] processes = Process . GetProcessesByName ( currentProcess . ProcessName );
            //遍历正在有相同名字运行的进程
            foreach ( Process process in processes )
            {
                if ( process . MainModule . FileName == currentFileName )
                {
                    if ( process . Id != currentProcess . Id )//排除当前的进程
                        return process;//返回已启动的进程实例
                }
            }
            return null;
        }
    }
}
