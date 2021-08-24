using Framework.Tools;
using Framework.UI;
using ProtoDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 只能用来显示建造时间还有多久结束
/// </summary>
public class CommonBuildProgress : MonoBehaviour {

    public Devlopments m_deveInfo;
    public GameObject m_btnSpeedUp;
    public long startTime;
    public long endTime;
    public long curTime;
    public long totalTime;//总时长的s数
    public Slider m_slider;
    public Text m_textLeft;

    // Use this for initialization
    void Start () {
        ClickListener.Get(m_btnSpeedUp).onClick = OnBtnClickSpeedUp;

    }

    void OnBtnClickSpeedUp(GameObject obj)
    {
        UIManager.Instance.PushPanel(UIPanelName.htspeeduppanel, false, true,
            (param)=>{
                htspeeduppanel speedUp = param.GetComponent<htspeeduppanel>();
                speedUp.SetInfo(m_deveInfo);
                }
            );
    }
    private void OnDestroy()
    {
        TimeManager.Instance.RemoveTask(UpdateProgress);
    }

    public void SetInfo(Devlopments info)
    {
        m_deveInfo = info;
        SetBtnSpeedUpVisible();
        UpdateProgress();
        TimeManager.Instance.RemoveTask(UpdateProgress);
        TimeManager.Instance.AddTask(1.0f, true, UpdateProgress);
    }

    void SetBtnSpeedUpVisible()
    {
        switch (buildhometown.m_instance.m_myOther) 
        {
            case EnMyOhter.My:
                m_btnSpeedUp.SetActive(true);

                if (m_deveInfo.speedUpTimes >= DataMgr.m_slefHPTimes)
                {
                    m_btnSpeedUp.SetActive(false);
                }

                break;
            case EnMyOhter.Other:
                m_btnSpeedUp.SetActive(true);
                if (m_deveInfo.helpRecod != null)
                {
                    if (m_deveInfo.helpRecod.ContainsKey(DataMgr.m_account.id.ToString()))
                    {
                        m_btnSpeedUp.SetActive(false);
                    }
                }

                if (m_deveInfo.friendHelp >= DataMgr.m_friendHPTimes)
                {
                    m_btnSpeedUp.SetActive(false);
                }

                if (m_deveInfo.speedUpTimes >= DataMgr.m_slefHPTimes)
                {
                    m_btnSpeedUp.SetActive(false);
                }
                break;
        }
    }

    void UpdateProgress()
    {
        startTime = SyncTime.Server2Stamp(m_deveInfo.buildDate);
        endTime = SyncTime.Server2Stamp(m_deveInfo.rewardDate);

        totalTime = endTime - startTime;

        long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());

        m_slider.value = (float)(curServerTime - startTime) / (float)totalTime;

        TimeSpan left = SyncTime.GetLeftTime(endTime);
        string strLeft = "";
        bool isLeft = false;
        if (left.Days != 0)
        {
            strLeft += left.Days + "天";
            isLeft = true;
        }
        if (left.Hours != 0)
        {
            strLeft += left.Hours + "时";
            isLeft = true;
        }
        if (left.Minutes != 0)
        {
            strLeft += left.Minutes + "分";
            isLeft = true;
        }

        if (left.Seconds != 0)
        {
            strLeft += left.Seconds + "秒";
            isLeft = true;
        }

        if (isLeft == false)
        {
            Debug.Log("建造时间到了");
            //buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id); // 建造时间到了，请求
            gameObject.SetActive(false);
            TimeManager.Instance.RemoveTask(UpdateProgress);
        }
        else {
            m_textLeft.text = "建成:" + strLeft;
        }

        

        //if (left.TotalSeconds <= 0.0f)
        //{
        //    Debug.Log("建造时间到了");
        //    //buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id); // 建造时间到了，请求
        //    gameObject.SetActive(false);
        //    TimeManager.Instance.RemoveTask(UpdateProgress);
        //}
    }
}
