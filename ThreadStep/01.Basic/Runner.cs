using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01.Basic
{
    public class Runner
    {
        /// <summary>
        /// 1.基本
        /// </summary>
        public static void Base()
        {
            ///查看Thread的构造函数，可以看到有2个重载，1个是ThreadStart,1个是ParameterizedThreadStart，分别是无参和带1个object参数的委托。k

            ///1.thread t=new thread();这个构造函数默认只能传1个无餐的委托；
            Thread t = new Thread(Tester01Base.Counte);
            t.Name = "1.thread t=new thread();";

            t.Start();
            ///2.ParameterizedThreadStart;这个构造函数接受1个OBJECT类型的参数，自己再对这个object进行拆分，分成自己想要的参数，比如string []等;
            ParameterizedThreadStart para = new ParameterizedThreadStart(Tester01Base.ParaCounte);
            Thread parat = new Thread(para);
            parat.Name = "2.ParameterizedThreadStart:thread";
            parat.Start(5);
            ///3.t.Join()用于等待t线程执行完成；
            Thread twait = new Thread(Tester01Base.Counte);
            twait.Name = "3.t.Join()";
            //  twait.Start(1000);***********本来是无参的线程，虽然你传了参数编译可以通过，但是运行会出现异常。
            parat.Join();
            twait.Start();//只有parat执行完成过后，当前线程（这里是主线程）才会继续下去.
            //4.Abort进行线程终止:z这不是终止线程的科学方法
            Thread tabort = new Thread(new ParameterizedThreadStart(Tester01Base.AbortOne));
            tabort.Name = "4.Abort进行线程终止";
            tabort.Start(twait);
            //5.ThreadState检测线程状态
            ParameterizedThreadStart paraState = new ParameterizedThreadStart(Tester01Base.ParaCounte);
            Thread tstate = new Thread(paraState);
            tstate.Name = "5.ThreadState检测线程状态";
            new Thread(() =>
            {
                while (tstate.ThreadState != ThreadState.Stopped)
                {
                    Thread.Sleep(1000);
                    Tester01Base.ConsoleThreadState(tstate);//*********使用lambda表达式传递参数；注意最好不要传递变量（但是这里这样做了），最好只是传递常量

                }

            }).Start();
            //6. t.Priority用于设置献策会给你的优先级
            tstate.Priority = ThreadPriority.Highest;
            tstate.Start(20);
            Console.WriteLine(Thread.CurrentThread.ThreadState);
        }
        /// <summary>
        /// 进阶1：后台线程
        /// </summary>
        public static void BackgroudThread()
        {
            ///一旦前台线程执行完，则马上退出进程（主进程会调用其它后台线程的abort方法终止其他们）；
            //需要注意的是，之前的实验用了Console.ReadKey()致使前台线程一直没有执行完毕，所以才会2个线程都执行完。

            Thread tbefore = new Thread(new ParameterizedThreadStart(Tester01Base.ParaCounte));
            Thread tbackground = new Thread(new ParameterizedThreadStart(Tester01Base.ParaCounte));
            tbackground.IsBackground = true;
            tbefore.Start(5);
            tbackground.Start(20);

        }
        /// <summary>
        /// 线程传递参数方法：
        /// 1.使用ParameterParameterizedThreadStart+t.start(Object para)传递1个object类型的参数
        /// </summary>
        public static void TransParameter()
        {
            // 1.使用ParameterParameterizedThreadStart + t.start(Object para)传递1个object类型的参数
            Thread t1 = new Thread(Tester01Base.ParaCounte);
            t1.Start(5);
            //2.定义线程环境类
            ThreadCount tc = new ThreadCount(10);
            Thread t = new Thread(tc.Count);
            t.Start();
            int len = 5;
            //3.使用lambda表达式：使用变量则是闭包：多个线程则是共享的这个变量;（尽量使用常量）
            Thread tlam = new Thread(() =>
            {
                for (int i = 0; i < len; i++)
                {
                    Thread.Sleep(300);
                    Console.WriteLine("para by lambda1:" + i);
                }
            });
            tlam.Start();//********线程启动过后，更改了len的值，线程1还是会依照修改后的变量：
            len = 10;
            // tlam.Start();
            Thread tlam2 = new Thread(() =>
            {
                for (int i = 0; i < len; i++)
                {
                    Thread.Sleep(300);
                    Console.WriteLine("para by lambda2:" + i);
                }
            });
            tlam2.Start();
        }
        public class ThreadCount
        {
            private int _count;
            public ThreadCount(int count)
            {
                _count = count;

            }
            public void Count()
            {
                for (int i = 0; i < _count; i++)
                {
                    Thread.Sleep(300);
                    Console.WriteLine("para by class:" + i);
                }
            }
        }
        /// <summary>
        /// 不使用lock进行线程同步
        /// </summary>

        public static void UnLockUse()//********对于没有使用lock进行锁的情况，最终结果不固定；因为线程同步出了问题
        {

            Tester01Base.Counter c = new Tester01Base.Counter();
            c.value = 0;
            var t1 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {

                    Tester01Base.Incrememt(c);
                }
            });
            var t2 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    Tester01Base.Decrement(c);
                }
            });
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine(c.value);
        }
        /// <summary>
        /// 使用lock进行线程同步
        /// </summary>

        public static void LockUse()//**********8使用lock进行同步，得到的结果一直为0；但是会有性能损失;
        {
            Tester01Base.LockCounter c = new Tester01Base.LockCounter();
            c.value = 0;
            var t1 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    lock (c._root)
                    {
                        Tester01Base.Incrememt(c);
                    }

                }
            });
            var t2 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    lock (c._root)
                    {
                        Tester01Base.Decrement(c);
                    }
                }
            });
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine(c.value);

        }

        /// <summary>
        /// 2个线程互相等待对方的资源造成死锁
        /// </summary>
        public static void DeadLock()
        {
            object _root1 = new object();
            object _root2 = new object();
          var t1=  new Thread(() =>
            {
                lock (_root1)
                {
                    Thread.Sleep(1000);
                    lock (_root2);
                    Console.WriteLine("Ok-1");
                }


            });
         var t2=   new Thread(() =>
            {
                lock (_root2)
                {
                    Thread.Sleep(1000);
                    lock (_root1);
                    Console.WriteLine("Ok-2");
                }


            });
            new Thread(() => {
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("t1:"+t1.ThreadState);
                    Console.WriteLine("t2:" + t2.ThreadState);

                }
            }).Start();
            t1.Start();
            t2.Start();
     
        }
        /// <summary>
        /// 使用Moniror.TryEnter尝试获取1个资源，但是不强制锁定他;
        /// </summary>
        public static void MonitorLock()
        {
            object _root1 = new object();
            object _root2 = new object();
            var t1 = new Thread(() =>
            {
                lock (_root1)
                {
                    Thread.Sleep(1000);
                    lock (_root2);
                    Console.WriteLine("Ok-1");
                }


            });
            var t2 = new Thread(() =>
            {
                lock (_root2)
                {
                    Thread.Sleep(1000);
                    if (Monitor.TryEnter(_root1))
                    {
                        Console.WriteLine("get root1");
                    }
                    else {
                        Console.WriteLine("get root1 timeout");
                    }
                    Console.WriteLine("Ok-2");
                }


            });
            new Thread(() => {
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("t1:" + t1.ThreadState);
                    Console.WriteLine("t2:" + t2.ThreadState);

                }
            }).Start();
            t1.Start();
            t2.Start();

        }

        ///线程的异常处理:
        ///注意：可以看到，在1个线程（比如主线程）是捕捉不了另外1个线程的异常状态的。所以，异常必须在那个线程本身使用try catch;
        /// ******记住：哪个线程的异常，就让那个线程自己去处理，不要试图用1个线程去捕捉另外1个线程的异常;
        public static void ThreadExcepitonHandle()
        {
            try
            {
                Thread t = new Thread(ThrowException);
                t.Start();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        

        }
        public static void ThrowException()
        {
            Console.WriteLine("current thread :"+Thread.CurrentThread.Name);
            throw new Exception("i thow it");

        }
        public static void CatchThrowException()
        {
            try
            {
                Console.WriteLine("current thread :" + Thread.CurrentThread.Name);
                throw new Exception("i thow it");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
     

        }
    }
}
