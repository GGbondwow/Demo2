using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //播放摄像机 左转动画 然后再显示选角面板
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                //显示选角面板
                UIManager.Instance.showPanel<ChooseHeroPanel>();
                print("选择角色");
                print(Application.persistentDataPath + "/" + "PlayerData" + ".json");
            });
            //隐藏开始界面
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            //之后在这里 显示设置界面
            UIManager.Instance.showPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            //制作一个关于面板 之后在这里显示
        });
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();//该API编辑器下不能退出,只有发布之后才能退出游戏
        });
    }
}
