using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _03使用线程池
{
    public delegate string GetString(string str);
    class Tester
    {
        ///1.在线程池中使用委托
        private GetString getDele;

        public void Run()
        {
            getDele = Say;
            //0.1同步调用
          string strResult = getDele.Invoke("张三");
            //0.2异步调用,callback是AsyncCallback委托，参数为IAsyncResult的void类型方法
            //注意：使用BeginInvoke方法异步调用的时候，委托本身以及回调函数都是线程池线程执行的方法
            IAsyncResult re = getDele.BeginInvoke("李四", SayCallback, "干嘛用的呢");///第三个参数是传给回调函数用的
            //0.3指定等待异步操作完成
            re.AsyncWaitHandle.WaitOne();
            ///使用EndInvoke等待操作完成:注意，回调函数完全有可能没有完成
            string result = getDele.EndInvoke(re);
            Console.WriteLine("结果为："+result);
            Console.ReadKey();
        }
        public string Say(string str)
        {
            Console.WriteLine("你想获得字符串:"+str);
            Console.WriteLine("当前是线程池线程："+Thread.CurrentThread.IsThreadPoolThread);
            return "你得到了:"+str;

        }
        /// <summary>
        /// 最终没有执行完，因为这是线程池线程，后台线程
        /// </summary>
        /// <param name="re"></param>
        public void SayCallback(IAsyncResult re)
        {
            Console.WriteLine("say 操作完成");
            Console.WriteLine("当前状态:"+re.AsyncState);
            Console.WriteLine("是否完成:" + re.IsCompleted);
            Console.WriteLine("是否线程池线程:" + Thread.CurrentThread.IsThreadPoolThread);
            var obj = re.AsyncState;
            Console.WriteLine("传给我了什么东西" + re.AsyncState);
        }
    }
}
