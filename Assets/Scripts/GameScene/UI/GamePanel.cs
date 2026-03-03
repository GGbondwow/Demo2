using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;
    public Text txtWave;
    public Text txtMoney;
    public Button btnQuit;

    //Hp的初始宽 可以在外面去控制它 到底有多宽
    public float hpw = 500f;
    //下方造塔组合控件的父对象 主要用于控制显隐
    public Transform botTrans;
    //管理三个复合控件
    public List<TowerBtn> towerBtns = new List<TowerBtn>();
    //当前进入和选中的造塔点
    private TowerPoint nowSelTowerPoint;
    //用来标识 是否检测 造塔输入
    private bool checkInput;
    public override void Init()
    {
        //监听按钮事件
        btnQuit.onClick.AddListener(() =>
        {
            //隐藏游戏界面
            UIManager.Instance.HidePanel<GamePanel>();
            //返回到开始界面
            SceneManager.LoadScene("BeginScene");

        });
        //一开始隐藏下方造塔相关的UI
        botTrans.gameObject.SetActive(false);

        //锁定鼠标
        Cursor.lockState = CursorLockMode.Confined;
    }
    /// <summary>
    /// 更新安全区域血量函数
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="maxHP">最大血量</param>
    public void UpdateTowerHp(int hp,int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        //更新血条的长度
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpw, 38);
        //imgHP.rectTransform.sizeDelta = new Vector2((float)hp / maxHP * hpw, 38);
    }
    /// <summary>
    /// 更新剩余波数量
    /// </summary>
    /// <param name="nowNum">当前波数</param>
    /// <param name="maxNum">最大波数</param>
    public void UpdateWaveNum(int nowNum,int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }
    /// <summary>
    /// 更新金币数量
    /// </summary>
    /// <param name="money">当前获得的金币</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }
    /// <summary>
    /// 更新当前选中造塔点 界面的一些变化
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        //根据造塔点的信息 决定 界面上的显示内容
        nowSelTowerPoint = point;
        //如果传入数据为空
        if (nowSelTowerPoint == null)
        {
            checkInput = false;
            //隐藏下方造塔按钮
            botTrans.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            //显示下方造塔按钮
            botTrans.gameObject.SetActive(true);
            
            //如果没有造过塔
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "数字键" + (i + 1));
                }
            }
            //如果造过塔
            else
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "空格键");
            }
        }
        

    }

    protected override void Update()
    {
        base.Update();
        //主要用于造塔点 键盘输入
        if (!checkInput)
            return;
        //如果没有造过塔 那么就检测123按钮去建造塔
        if(nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreatTower(nowSelTowerPoint.chooseIDs[0]);
            }else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreatTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreatTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        //如果造过塔 就检测空格键去建造
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreatTower(nowSelTowerPoint.nowTowerInfo.nextLev);
            }
        }
    }



}
