using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01.Basic
{
    public class Tester01Base
    {
        /// <summary>
        /// 午餐方法
        /// </summary>

        public static void Counte()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);///暂停线程：1个静态方法。
            }

        }
        /// <summary>
        /// 带1个参数的方法
        /// </summary>
        public static void ParaCounte(object len)
        {
            int length = (int)len;
            for (int i = 0; i < length; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }

        }
        public static void AbortOne(object t)
        {
            Thread th = (Thread)t;
            th.Abort();///强制终止t线程
        }
        /// <summary>
        /// 检测线程状态
        /// </summary>
        /// <param name="t"></param>
        public static void ConsoleThreadState(object t)
        {
            Thread th = (Thread)t;
            Console.WriteLine(th.Name + " s state is:" + th.ThreadState);//检测线程状态;

        }

        public static void Incrememt(Counter c)
        {
            c.value++;
        }
        public static void Decrement(Counter c)
        {
            c.value--;
        }
        public class Counter
        {
            public int value { get; set; }

        }
        public class LockCounter : Counter
        {
            public readonly object _root = new object();
        }
    }
}
