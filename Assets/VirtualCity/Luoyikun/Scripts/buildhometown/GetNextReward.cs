using Framework.Tools;
using ProtoDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetNextReward : MonoBehaviour {
    Devlopments m_deveInfo;

    long m_leftTime;
    DeveTimer m_deveTime;
    public Text m_textLeft;

    public Slider m_slider;
    public void SetInfo(Devlopments info)
    {
        m_deveInfo = info;

        //long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());
        m_deveTime = buildhometown.m_dicBuildObj[info.id].GetComponent<DeveTimer>();

        //m_endTime = curServerTime + (long)deveTime.m_totalLeftGetRewardTime;
        //m_leftTime = 

        ////UpdateProgress();
        UpdateProgress();
        TimeManager.Instance.RemoveTask(UpdateProgress);
        TimeManager.Instance.AddTask(1.0f, true, UpdateProgress);
    }

    private void OnDestroy()
    {
        TimeManager.Instance.RemoveTask(UpdateProgress);
    }
    void UpdateProgress()
    {
        long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());

        long endTime = curServerTime + (long)m_deveTime.m_totalLeftGetRewardTime - (long)m_deveTime.m_curRewardTime;
        //long totalTime = endTime - startTime;
        TimeSpan left = SyncTime.GetLeftTime(endTime);

        //long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());
        m_slider.value = (float)(m_deveTime.m_curRewardTime) / (float)m_deveTime.m_totalLeftGetRewardTime;


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
            m_textLeft.text = "收益倒计时:" + strLeft;
        }
        else
        {
            Debug.Log("剩余收益到了");
            
            TimeManager.Instance.RemoveTask(UpdateProgress);
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }

}
