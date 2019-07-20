/****************************************************
	文件：ServerStart.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 9:56   	
	功能：服务器入口
*****************************************************/

using System.Threading;

class ServerStart
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();

        while (true)
        {
            ServerRoot.Instance.Update();
            Thread.Sleep(20);
        }
    }
}