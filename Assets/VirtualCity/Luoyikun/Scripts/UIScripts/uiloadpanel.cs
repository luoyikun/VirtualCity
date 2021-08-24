using Framework.Pattern;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiloadpanel : MonoSingleton<uiloadpanel>
{
    public Transform m_load;
    public bool m_isOpen = false;
    Transform m_par;
    public GameObject m_objHua;

    bool m_isOpenByNewGuide = false;

    public void OpenByNewGuide()
    {
        return;
        Debug.Log("loading打开");
        m_isOpenByNewGuide = true;
        m_objHua.SetActive(false);

        if (m_par == null)
        {
            m_par = UIManager.Instance.GetParent(UIManager.CanvasType.Screen);
        }
        m_isOpen = true;
        transform.SetParent(m_par, false);
        transform.localPosition = Vector3.zero;
        transform.SetAsLastSibling();
        gameObject.SetActive(true);

    }

    public void CloseByNewGuide()
    {
        return;
        m_isOpenByNewGuide = false;
        m_isOpen = false;
        gameObject.SetActive(false);
    }
    public void Open(bool isHua = true)
    {
        Debug.Log("loading打开");

        m_objHua.SetActive(isHua);
        //if (m_isOpen == false)
        {
            if (m_par == null)
            {
                m_par = UIManager.Instance.GetParent(UIManager.CanvasType.Screen);
            }
            m_isOpen = true;
            transform.SetParent(m_par, false);
            transform.localPosition = Vector3.zero;
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }
        //transform.local = Vector3.zero;
    }

    public void Close()
    {
        if (m_isOpenByNewGuide == true)
        {
            return;
        }
        Debug.Log("loading关闭");
        //while (!(VirtualCityMgr.m_isGetGameData == true && VirtualCityMgr.m_isGetChatData == true))
        //   Debug.Log("loading关闭");
        //if (m_isOpen == true)
        {
            m_isOpen = false;
            gameObject.SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () {
        m_load.Rotate(Vector3.forward * 25 * Time.deltaTime, Space.Self);
    }
}
