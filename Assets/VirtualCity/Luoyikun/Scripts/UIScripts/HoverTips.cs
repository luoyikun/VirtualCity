using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EnHoverDir
{
    Up,
    Down
}

//悬浮提示不受ui栈管理
public class HoverTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static GameObject m_panel = null;

    public EnHoverDir m_dir = EnHoverDir.Up;

    public string m_showText;
    public Vector2 m_offSet = Vector2.zero;
    static public HoverTips Get(GameObject go)
    {
        HoverTips listener = go.GetComponent<HoverTips>();
        if (listener == null) listener = go.AddComponent<HoverTips>();
        return listener;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_panel == null)
        {
            m_panel = PublicFunc.CreateTmp(UIManager.Instance.GetLoadObject("hoverpanel"));
            Transform tempParent = UIManager.Instance.GetParent(UIManager.CanvasType.Screen);
            m_panel.transform.SetParent(tempParent, false);
            m_panel.transform.SetAsLastSibling();
        }
        m_panel.SetActive(true);


        m_panel.transform.Find("Text").GetComponent<Text>().text = m_showText;

        float offset = transform.GetComponent<RectTransform>().sizeDelta.y + m_panel.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
        float y = 0;
         switch (m_dir)
        {
            case EnHoverDir.Up:
                y = eventData.position.y + offset + m_offSet.y;
                break;
            case EnHoverDir.Down:
                y = eventData.position.y - offset + m_offSet.y;
                break;
            default:
                break;
        }
        float x = eventData.position.x + m_offSet.x;
        
        m_panel.transform.position = new Vector3(x, y, 0);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_panel != null)
        {
            m_panel.SetActive(false);
        }
    }
}
