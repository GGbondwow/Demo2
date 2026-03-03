using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //用于存储显示着的面板 每显示一个面板就存入这个字典
    //隐藏面板时 直接获取字典中的对应面板 进行隐藏
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();
    //场景中的Canvas对象,用于设置为面板的父对象
    private Transform canvasTrans;

    private UIManager()
    {
        //得到场景中的Canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //过场景不移除,保证这个游戏过程中只有一个canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }
    //显示面板
    public T showPanel<T>() where T : BasePanel
    {
        //我们只需要保证 泛型T的类型和面板预设体名字一样 定一个这样的规则,就可以非常方便地让我们使用了
        string panelName = typeof(T).Name;
        //判断字典中 是否已经显示了这面板
        if (panelDic.ContainsKey(panelName) )
            return panelDic[panelName] as T;
        //显示面板 根据面板的名字 动态的创建预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //把这个对象放到场景中的Canvas下面
        panelObj.transform.SetParent(canvasTrans,false);
        //指向面板上 显示逻辑 并且应该把它保存起来
        T panel = panelObj.GetComponent<T>();
        //把这个面板脚本 存储到字典中 方便之后的获取和隐藏
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.Show();
        return panel;
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否淡出完毕过后才删除面板 </param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //根据泛型得到名字
        string panelName = typeof (T).Name;
        //判断当前显示的面板 有没有想要隐藏的
        if(panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //让面板 淡出完毕后 再删除
                panelDic[panelName].Hide(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里面存储的面板脚本
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典里面存储的面板脚本
                panelDic.Remove(panelName);
            }
            
        }
    }
    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        return null;
    }
}
