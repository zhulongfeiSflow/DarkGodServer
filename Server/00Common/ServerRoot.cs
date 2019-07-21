/****************************************************
	文件：ServerRoot.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/08 9:58   	
	功能：服务器初始化
*****************************************************/
public class ServerRoot
{
    private static ServerRoot instance = null;

    public static ServerRoot Instance {
        get {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init()
    {
        //数据层
        DBMgr.Instance.Init();

        //服务层
        CfgSvc.Instance.Init();
        CacheSvc.Instance.Init();
        NetSvc.Instance.Init();
        TimerSvc.Instance.Init();

        //业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        FubenSys.Instance.Init();

        //TimerSvc.Instance.AddTimeTask((int tid) =>
        //{
        //    PECommon.Log("xxxxxxxxxxxxxxxxxx");
        //}, 1000,PETimeUnit.Millisecond,0);
    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID += 1;
    }
}