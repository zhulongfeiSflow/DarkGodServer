/****************************************************
	文件：DBMgr.cs
	作者：朱龙飞
	邮箱: 398670608@qq.com
	日期：2019/07/12 17:25   	
	功能：数据库管理类
*****************************************************/

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

class DBMgr {
    private static DBMgr instance = null;

    public static DBMgr Instance {
        get {
            if (instance == null) {
                instance = new DBMgr();
            }
            return instance;
        }
    }
    private MySqlConnection conn;

    public void Init() {
        conn = new MySqlConnection("server=localhost;User Id = root;password=;Database=darkgod;charset=utf8");
        conn.Open();
        PECommon.Log("DBMgr Init Done.");

        //QueryPlayerData("qqq","asds");
    }

    public PlayerData QueryPlayerData(string acct, string pass) {
        bool isNew = true;
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read()) {
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass)) {
                    //密码正确，返回玩家数据
                    playerData = new PlayerData {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond"),
                        crystal = reader.GetInt32("crystal"),

                        hp = reader.GetInt32("hp"),
                        ad = reader.GetInt32("ad"),
                        ap = reader.GetInt32("ap"),
                        addef = reader.GetInt32("addef"),
                        apdef = reader.GetInt32("apdef"),
                        dodge = reader.GetInt32("dodge"),
                        pierce = reader.GetInt32("pierce"),
                        critical = reader.GetInt32("critical"),

                        guideid = reader.GetInt32("guideid"),
                        time = reader.GetInt64("time"),
                        fuben = reader.GetInt32("fuben"),

                        //TOADD
                    };

                    #region 强化升级
                    //数据示意：1#2#3#4#5#6
                    string[] strongStrArr = reader.GetString("strong").Split('#');

                    int[] _strongArr = new int[6];
                    for (int i = 0; i < strongStrArr.Length; i++) {
                        if (strongStrArr[i] == "") {
                            continue;
                        }
                        if (int.TryParse(strongStrArr[i], out int startlv)) {
                            _strongArr[i] = startlv;
                        }
                        else {
                            PECommon.Log("Parse Strong Data Error.", LogType.Error);
                        }
                    }
                    playerData.strongArr = _strongArr;
                    #endregion

                    #region 任务数据
                    //示意数据： 1|1|0#2|1|0#3|1|0#3|1|0#4|1|0
                    string[] taskStrArr = reader.GetString("task").Split('#');
                    playerData.taskArr = new string[6];
                    for (int i = 0; i < taskStrArr.Length; i++) {
                        if (taskStrArr[i] == "") {
                            continue;
                        }
                        else if (taskStrArr[i].Length >= 5) {
                            playerData.taskArr[i] = taskStrArr[i];
                        }
                        else {
                            throw new Exception("Task Analysis DataError!");
                        }
                    }
                    #endregion


                    //TODO
                }
            }
        }
        catch (System.Exception ex) {
            PECommon.Log("Query PlayerData By Acct&Pass Error:" + ex, LogType.Error);
        }
        finally {
            if (reader != null) {
                reader.Close();
            }

            if (isNew) {
                //不存在账号数据，创建新的默认账号数据，并返回
                playerData = new PlayerData {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,
                    crystal = 500,

                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,

                    guideid = 1001,
                    strongArr = new int[6],

                    time = TimerSvc.Instance.GetNowTime(),
                    taskArr = new string[6],
                    fuben =10001,
                    //TOADD
                };

                //初始化任务奖励数据
                //示意数据： 1|1|0#2|1|0#3|1|0#3|1|0#4|1|0
                for (int i = 0; i < playerData.taskArr.Length; i++) {
                    playerData.taskArr[i] = (i + 1) + "|0|0";
                }

                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }

        //TODO
        return playerData;
    }

    /// <summary>
    /// 插入一条新的玩家数据
    /// </summary>
    private int InsertNewAcctData(string acct, string pass, PlayerData pd) {
        int id = -1;
        try {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal" + ",hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strong=@strong,time=@time,task=@task,fuben=@fuben", conn);

            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);

            cmd.Parameters.AddWithValue("guideid", pd.guideid);

            cmd.Parameters.AddWithValue("strong", string.Join("#", pd.strongArr));
            cmd.Parameters.AddWithValue("time", pd.time);
            cmd.Parameters.AddWithValue("task", string.Join("#", pd.taskArr));
            cmd.Parameters.AddWithValue("fuben", pd.fuben);

            //TODO
            cmd.ExecuteNonQuery();

            id = (int)cmd.LastInsertedId;
        }
        catch (System.Exception ex) {
            PECommon.Log("Insert PlayerData Error:" + ex, LogType.Error);
        }

        return id;
    }

    public bool QueryNameData(string name) {
        bool exist = false;
        MySqlDataReader reader = null;

        try {
            MySqlCommand cmd = new MySqlCommand("select * from account where name= @name", conn);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read()) {
                exist = true;
            }
        }
        catch (System.Exception ex) {
            PECommon.Log("Query Name State Error:" + ex, LogType.Error);
        }
        finally {
            if (reader != null) {
                reader.Close();
            }
        }

        return exist;
    }

    public bool UpdatePlayerData(int id, PlayerData pd) {
        try {
            MySqlCommand cmd = new MySqlCommand(
            "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal" + ",hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid,strong=@strong,time=@time,task=@task,fuben=@fuben where id =@id", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            cmd.Parameters.AddWithValue("crystal", pd.crystal);

            cmd.Parameters.AddWithValue("hp", pd.hp);
            cmd.Parameters.AddWithValue("ad", pd.ad);
            cmd.Parameters.AddWithValue("ap", pd.ap);
            cmd.Parameters.AddWithValue("addef", pd.addef);
            cmd.Parameters.AddWithValue("apdef", pd.apdef);
            cmd.Parameters.AddWithValue("dodge", pd.dodge);
            cmd.Parameters.AddWithValue("pierce", pd.pierce);
            cmd.Parameters.AddWithValue("critical", pd.critical);

            cmd.Parameters.AddWithValue("guideid", pd.guideid);

            cmd.Parameters.AddWithValue("strong", string.Join("#", pd.strongArr));
            cmd.Parameters.AddWithValue("time", pd.time);
            cmd.Parameters.AddWithValue("task", string.Join("#", pd.taskArr));
            cmd.Parameters.AddWithValue("fuben", pd.fuben);

            //TOADD Others
            cmd.ExecuteNonQuery();
        }
        catch (System.Exception ex) {
            PECommon.Log("Updata PlayerData Error:" + ex, LogType.Error);
            return false;
        }
        return true;
    }
}