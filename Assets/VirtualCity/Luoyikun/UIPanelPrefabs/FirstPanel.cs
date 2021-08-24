using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPanel : UGUIPanel {
    [SerializeField] Slider m_slider;
    [SerializeField] Text m_textDown;
    [SerializeField] Text m_textTop;
    // Use this for initialization
    void Start () {
        
        m_slider.gameObject.SetActive(false);
        m_textDown.gameObject.SetActive(false);
        m_textTop.gameObject.SetActive(false);

        
	}

    public override void OnOpen()
    {
        Debug.Log("Create Firstpanel");
        EventManager.Instance.AddEventListener(Common.EventStr.UpdateProgress, SetSliderValue);
    }

    public override void OnClose()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.UpdateProgress, SetSliderValue);
    }
    private void OnDestroy()
    {
        
    }

    
    public void SetSliderValue(EventData note)
    {
        m_slider.gameObject.SetActive(true);

        m_textTop.gameObject.SetActive(true);

        var data = note as EventDataEx<float>;
        float speed = data.GetData();
        if (DataMgr.m_downTotal != 0 && speed >= 0.0f)
        {
            m_slider.value = (float)DataMgr.m_downCur / (float)DataMgr.m_downTotal;
            string sPrecent = string.Format("{0} MB / {1} MB", (DataMgr.m_downCur / 1024d / 1024d).ToString("0.00"), (DataMgr.m_downTotal / 1024d / 1024d).ToString("0.00"));
            m_textTop.text ="已经下载" + sPrecent + "文件";
            //m_textDown.text = speed.ToString();

            if (m_slider.value == 1.0f)
            {
                Debug.Log("更新面板更新完成，开始游戏");
            }
        }
        else
        {
            m_textTop.text = "正在释放资源";

            m_slider.value = (float)DataMgr.m_downCur / (float)DataMgr.m_downTotal;
        }
    }
}
