using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    public UnityAction overAction;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    //左转
    public void TurnLeft(UnityAction action)
    {
        animator.SetTrigger("Left");
        overAction = action;
    }
    //右转
    public void TurnRight(UnityAction action)
    {
        animator.SetTrigger("Right");
        overAction = action;
    }
    //当动画播放完后 会调用的方法
    public void PlayOver()
    {
        overAction?.Invoke();
        overAction = null;
    }
}
