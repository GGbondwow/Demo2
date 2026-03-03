using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr 
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;

    public PlayerObject player;
    //所有的出怪点
    public List<MonsterPoint> points = new List<MonsterPoint>();
    //记录当前 还有多少波怪物
    private int nowWaveNum = 0;
    //记录一共有多少波怪物
    private int maxWaveNum = 0;

    //记录当前场景上的怪物数量
    //private int nowMonsterNum = 0;

    //记录当前场景上怪物的列表
    private List<MonsterObject> monsterList = new List<MonsterObject>();
    private GameLevelMgr() { }

    //1.切换到游戏场景时 我们需要动态的创建玩家
    public void InitInfo(SceneInfo info)
    {
        //显示游戏界面
        UIManager.Instance.showPanel<GamePanel>();
        //玩家的创建 获取之前记录的当前选中的玩家数据
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        //首先获取场景当中 玩家的出生位置
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        //实例化玩家预设体 然后把他的位置角度 设置为 场景当中出生点一致
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res), heroPos.position, heroPos.rotation);
        //对玩家对象进行初始化
        player = heroObj.GetComponent<PlayerObject>();
        //初始化玩家的基础属性
        player.InitPlayerInfo(roleInfo.atk, info.money);

        //让摄像机 看向动态创建出来的玩家
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);

        //初始化中央保护区的血量
        MainTowerObject.Instance.UpdateHp(info.towerHp, info.towerHp);


    }
    //2.我们需要通过游戏管理器 判断是否获胜
    //要知道 场景中 是否还有怪物没有出 以及场景中 是否有还没死亡的怪物

    //用于记录出怪点的方法
    public void AddMonsterPoint(MonsterPoint point)
    {
        points.Add(point);
    }
    /// <summary>
    /// 更新一共有多少波怪
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num)
    {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    public void ChangeNowWaveNum(int num)
    {
        nowWaveNum -= num;
        if (nowWaveNum < 0)
        {
            nowWaveNum = 0;
        }
        //更新界面
        UIManager.Instance.GetPanel<GamePanel>().UpdateWaveNum(nowWaveNum, maxWaveNum);
    }
    /// <summary>
    /// 检测是否胜利
    /// </summary>
    /// <returns></returns>
    public bool CheckOver()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].CheckOver())
            {
                return false;
            }
        }
        if(monsterList.Count > 0)
        {
            return false;
        }
        Debug.Log("游戏胜利");
        return true;
    }
    ///// <summary>
    ///// 改变当前场景上怪物的数量
    ///// </summary>
    ////// <param name="num"></param>
    ////public void ChangeMonsterNum(int num)
    ////{
    ////    nowMonsterNum += num;
    ////}
    
    /// <summary>
    /// 记录怪物到怪物列表中
    /// </summary>
    /// <param name="obj"></param>
    public void AddMonster(MonsterObject obj)
    {
        monsterList.Add(obj);
    }
    /// <summary>
    /// 将怪物从列表中移除 死亡时使用
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveMonster(MonsterObject obj)
    {
        monsterList.Remove(obj);
    }
    /// <summary>
    /// 寻找满足距离条件的单个怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public MonsterObject FindMonster(Vector3 pos,int range)
    {
        //在怪物列表中找到满足距离条件的怪物 返回出去 用于 塔攻击
        for(int i = 0;i < monsterList.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                return monsterList[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 寻找满足条件的所有怪物
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<MonsterObject> FindMonsters(Vector3 pos,int range)
    {
        //去寻找满足条件的所有怪物 并且把他们记录在一个列表中
        List<MonsterObject> list = new List<MonsterObject>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!monsterList[i].isDead && Vector3.Distance(pos, monsterList[i].transform.position) <= range)
            {
                list.Add(monsterList[i]);
            }
        }
        return list;
    }
    /// <summary>
    /// 清空当前关卡记录的数据 避免影响下一次切关卡
    /// </summary>
    public void ClearInfo()
    {
        points.Clear();
        monsterList.Clear();
        nowWaveNum = maxWaveNum = 0;
        player = null;
    }
}
