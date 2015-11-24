using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. Runner.Base();
            //
            //2.step1:backgroud
            // Runner.BackgroudThread();
            //3.传递参数
            //  Runner.TransParameter();
            //4.使用lock:
            // Runner.UnLockUse();
            //Runner.LockUse();
            //5.使用Monitor
            // Runner.DeadLock();
           // Runner.MonitorLock();
            Runner.ThreadExcepitonHandle();
            Runner.CatchThrowException();
            Console.ReadKey();

            
        }
    }
}
