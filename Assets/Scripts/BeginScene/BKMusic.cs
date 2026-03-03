using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
        //通过数据来设置 音乐的大小和开关
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

    //开关背景音乐的方法
    public void SetIsOpen(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }
    //调整背景音乐大小的方法
    public void ChangeValue(float v)
    {
        audioSource.volume = v;
    }
}
