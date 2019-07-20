/****************************************************
	文件：CfgSvc.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/16 13:26   	
	功能：配置数据服务
*****************************************************/

using System.Xml;
using System.Collections.Generic;
using System;

public class CfgSvc
{
    private static CfgSvc instance = null;

    public static CfgSvc Instance {
        get {
            if (instance == null)
            {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    public void Init()
    {
        InitGuideCfg();
        InitStrongCfg();
        InitTaskRewardDicCfg();
        PECommon.Log("CfgSvc Init Done.");

    }

    #region 自动引导配置
    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();
    private void InitGuideCfg() {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\Dragonfly\Documents\Unity Projects\DarkGod\Assets\Resources\ResCfgs\guide.xml");
        XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodList.Count; i++) {
            XmlElement ele = nodList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null) {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

            GuideCfg agc = new GuideCfg() { ID = ID };

            foreach (XmlElement e in nodList[i].ChildNodes) {
                switch (e.Name) {
                    //case "npcID":
                    //    agc.npcID = int.Parse(e.InnerText);
                    //    break;
                    //case "dilogArr":
                    //    agc.dilogArr = e.InnerText;
                    //    break;
                    //case "actID":
                    //    agc.actID = int.Parse(e.InnerText);
                    //    break;
                    case "coin":
                        agc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        agc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideDic.Add(ID, agc);
        }
        PECommon.Log("GuideCfg Init Done.");
    }

    public GuideCfg GetGuideCfg(int id) {
        GuideCfg agc = null;
        if (guideDic.TryGetValue(id, out agc)) {
            return agc;
        }
        return null;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardDicCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\Dragonfly\Documents\Unity Projects\DarkGod\Assets\Resources\ResCfgs\taskreward.xml");
        XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodList.Count; i++)
        {
            XmlElement ele = nodList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

            TaskRewardCfg agc = new TaskRewardCfg() { ID = ID };

            foreach (XmlElement e in nodList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "count":
                        agc.count = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        agc.exp = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        agc.coin = int.Parse(e.InnerText);
                        break;
                }
            }
            taskRewardDic.Add(ID, agc);
        }
        PECommon.Log("TaskRewardCfg Init Done.");
    }

    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg agc = null;
        if (taskRewardDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return agc;
    }
    #endregion

    #region 强化升级配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\Dragonfly\Documents\Unity Projects\DarkGod\Assets\Resources\ResCfgs\strong.xml");
        XmlNodeList nodList = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodList.Count; i++)
        {
            XmlElement ele = nodList[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

            StrongCfg sd = new StrongCfg() { ID = ID };

            foreach (XmlElement e in nodList[i].ChildNodes)
            {
                int val = int.Parse(e.InnerText);
                switch (e.Name)
                {
                    case "pos":
                        sd.pos = val;
                        break;
                    case "starlv":
                        sd.starlv = val;
                        break;
                    case "addhp":
                        sd.addhp = val;
                        break;
                    case "addhurt":
                        sd.addhurt = val;
                        break;
                    case "adddef":
                        sd.adddef = val;
                        break;
                    case "minlv":
                        sd.minlv = val;
                        break;
                    case "coin":
                        sd.coin = val;
                        break;
                    case "crystal":
                        sd.crystal = val;
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sd.pos, out dic))
            {
                dic.Add(sd.starlv, sd);
            }
            else
            {
                dic = new Dictionary<int, StrongCfg>();
                dic.Add(sd.starlv, sd);
                strongDic.Add(sd.pos, dic);
            }
        }
        PECommon.Log("StrongCfg Init Done.");
    }

    public StrongCfg GetStrongCfg(int pos, int startlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;

        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(startlv))
            {
                sd = dic[startlv];
            }
        }
        return sd;
    }
    #endregion
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class GuideCfg : BaseData<GuideCfg>
{
    //public int npcID;//触发任务目标NPC索引号
    //public string dilogArr;
    //public int actID;
    public int coin;
    public int exp;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg> {
    //public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData> {
    public int prgs;
    public bool taked;
}


public class BaseData<T>
{
    public int ID;
}
