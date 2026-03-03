using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 10f;
    //当前显示还是隐藏的标识
    public bool isShow = false;
    private UnityAction hideCallBack;
    protected virtual void Awake()
    {
        //一开始获取面板上挂载的组件
        canvasGroup = this.GetComponent<CanvasGroup>();
        //如果忘记添加 则自动添加
        if (canvasGroup == null )
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //淡入
        //当处于显示状态时 如果透明度不等于1,就会不停地加到1 到达1过后 就停止变化了
        if (isShow && canvasGroup.alpha !=1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //淡出
        else if(!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                //让面板淡出完成后 再去执行的一些逻辑
                hideCallBack?.Invoke();
            }
                
        }
    }
    /// <summary>
    /// 注册控件事件的方法 所有的子面板 都需要去注册一些控件事件
    /// 所以写成抽象方法 让子类必须去实现
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// 显示自己时做的逻辑
    /// </summary>
    public virtual void Show()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }
    //隐藏自己时做的逻辑
    public virtual void Hide(UnityAction callBack) 
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallBack = callBack;
    }
}
