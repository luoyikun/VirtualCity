using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftScrollItem : MonoBehaviour {
    public ScrollCallback m_callback;
    public Text m_text;
    LeftScroll m_leftPar;
    public int m_idx = 0;
    public Toggle m_toggle;
    public bool m_isSelect = false;
   
    private void Awake()
    {
        
        m_callback.callback = ScrollCellIndextest;

        ClickListener.Get(gameObject).onClick = OnBtn;
        //m_toggle.onValueChanged.AddListener((param) => { OnToggle(param); });
    }

    private void Start()
    {
        m_leftPar = transform.parent.parent.parent.GetComponent<LeftScroll>();
    }

    public void OnSelect()
    {
        transform.GetComponent<Image>().color = Color.green;
    }


    public void OnUnSelect()
    {
        transform.GetComponent<Image>().color = Color.white;
    }
    void OnBtn(GameObject obj)
    {
        m_leftPar.m_selectIdx = m_idx;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform trans = transform.parent.GetChild(i);
            trans.GetComponent<Image>().color = Color.white;

        }
    
        transform.GetComponent<Image>().color = Color.green;
    }
    void ScrollCellIndextest(int idx)
    {
        m_idx = idx;
        string name = "Cell " + idx.ToString();

        m_text.text = m_idx.ToString();
        if (m_leftPar != null)
        {
            if (m_idx == m_leftPar.m_selectIdx)
            {
                transform.GetComponent<Image>().color = Color.green;
            }
            else
            {
                transform.GetComponent<Image>().color = Color.white;
                //m_toggle.isOn = false;
            }
        }
    }

}
