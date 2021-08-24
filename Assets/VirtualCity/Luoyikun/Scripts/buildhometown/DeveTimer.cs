using Framework.Event;
using Framework.Tools;
using ProtoDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//开发建筑的定时器:建造时间，收益中时间
public class DeveTimer : MonoBehaviour {


    public Devlopments m_deveInfo;
    DevlopmentProperties m_devePro;

    public float m_totalLeftGetRewardTime;
    public float m_curRewardTime;
    //public long startTime;
    //public long endTime;
    //public long curTime;
    //public long totalTime;//总时长的s数


    // Use this for initialization
    void Start () {
		
	}

    private void OnDisable()
    {
        RemoveAllTime();
    }

    private void OnDestroy()
    {
        
    }

    public void RemoveAllTime()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.RemoveTask(OnIncomingFinish);
            TimeManager.Instance.RemoveTask(UpdateUnuse);
        }
    }
    public void SetInfo(Devlopments info)
    {
        RemoveAllTime();
        m_deveInfo = info;
        m_devePro = DataMgr.m_dicDevlopmentProperties[(long)m_deveInfo.modelId];
        m_totalLeftGetRewardTime = 0;
        m_curRewardTime = 0;
        switch (m_deveInfo.status)
        {
            case (short)EnDeveState.Building:
                TimeManager.Instance.AddTask(1.0f, true, OnBuilding);
                break;
            case (short)EnDeveState.InComing:
                OnIncoming();
                //OnUnuse();
                break;
            case (short)EnDeveState.Noincome:
                //ShowLastIncome();
                OnIncoming();
                break;
            default:
                break;
        }
    }
    void ShowLastIncome()
    {
        if (m_deveInfo.rewardNum >= (int)(m_deveInfo.rewardUnit))
        {
            //Debug.Log()
            EventManager.Instance.DispatchEvent(Common.EventStr.CreateDeveGetGold, new EventDataEx<string>(m_deveInfo.id));
        }
    }

    void OnUnuse()
    {
        //long curServerTime = SyncTime.DateTime2Stamp(SyncTime.GetSystemTime());
        //long endTime = curServerTime + (long)m_deveInfo.rewardTime;
        TimeManager.Instance.AddTask((float)m_deveInfo.rewardTime, false, UpdateUnuse);
    }

    void UpdateUnuse()
    {
        buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id);
        TimeManager.Instance.RemoveTask(UpdateUnuse);
    }
    void OnBuilding()
    {
        long endTime = SyncTime.Server2Stamp(m_deveInfo.rewardDate);
        TimeSpan left = SyncTime.GetLeftTime(endTime);
        if (left.TotalSeconds <= 0.0f)
        {
            // 建造时间为 0 ，请求改变状态
            buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id);
            TimeManager.Instance.RemoveTask(OnBuilding);
        }
    }

    void OnIncoming()
    {
        Debug.Log("rewardNum:" + m_deveInfo.rewardNum);
        Debug.Log("rewardUnit:" + m_deveInfo.rewardUnit);
        if (m_deveInfo.rewardNum < (int)(m_deveInfo.rewardUnit))
        {
            float precent = (float)(m_deveInfo.rewardUnit - m_deveInfo.rewardNum) / (float)m_deveInfo.rewardUnit;

            m_totalLeftGetRewardTime = precent * DataMgr.m_deveGetGoldUnitTime;
            //TimeManager.Instance.AddTask(leftTime, false, OnIncomingFinish);

            TimeManager.Instance.AddTask(1.0f,true, OnIncomingFinish);
        }
        else if (m_deveInfo.rewardNum >=  (int)(m_deveInfo.rewardUnit))
        {
            //Debug.Log()
            EventManager.Instance.DispatchEvent(Common.EventStr.CreateDeveGetGold, new EventDataEx<string>(m_deveInfo.id));
        }
    }

    void OnIncomingFinish()
    {
        m_curRewardTime += 1.0f;
        if (m_curRewardTime >= m_totalLeftGetRewardTime)
        {
            Debug.Log("得到金币到了");
            TimeManager.Instance.RemoveTask(OnIncomingFinish);
            buildhometown.m_instance.SendGetOneDeveInfo(m_deveInfo.id);
            
        }
    }
}
