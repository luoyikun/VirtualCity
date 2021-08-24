using Framework.Tools;
using ProtoDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnuseProgress : MonoBehaviour {
    Devlopments m_deveInfo;
    long m_endTime;//剩下收益时间戳
    public Text m_textLeft;
    public Slider m_slider;
    // Use this for initialization
    void Start () {
		
	}

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.RemoveTask(UpdateProgress);
        }
    }
    public void SetInfo(Devlopments info)
    {
        m_deveInfo = info;

        long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());
        m_endTime = curServerTime + (long)m_deveInfo.rewardTime;

        
        UpdateProgress();

        TimeManager.Instance.AddTask(1.0f, true, UpdateProgress);
    }

    void UpdateProgress()
    {
        if (gameObject == null)
        {
            TimeManager.Instance.RemoveTask(UpdateProgress);
            return;
        }

        long startTime = SyncTime.Server2Stamp(m_deveInfo.buildDate);
        //long endTime = SyncTime.Server2Stamp(m_deveInfo.rewardDate);

        //long totalTime = endTime - startTime;
        TimeSpan left = SyncTime.GetLeftTime(m_endTime);

        //long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());
        //m_slider.value = (float)(curServerTime - startTime) / (float)totalTime;


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

        if (isLeft == true)
        {
            m_textLeft.text = "剩余收益:" + strLeft;
        }
        else {
            Debug.Log("剩余收益到了");
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
            TimeManager.Instance.RemoveTask(UpdateProgress);
            
        }
        //if (left.TotalSeconds <= 0.0f)
        //{
        //    Debug.Log("剩余收益到了");
        //    //buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id); // 建造时间到了，请求
        //    gameObject.SetActive(false);
        //    buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id);
        //    TimeManager.Instance.RemoveTask(UpdateProgress);
        //}

    }
}
