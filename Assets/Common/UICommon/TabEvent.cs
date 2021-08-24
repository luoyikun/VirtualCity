using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabEvent : MonoBehaviour {

    public delegate void OnValueChange(string id,bool value, Transform obj);
    public OnValueChange m_onValueChange;
    public string m_id = "0";
    Transform m_trans;
    // Use this for initialization
    void Start () {
        m_trans = transform;
        transform.GetComponent<Toggle>().onValueChanged.AddListener(OnEvValueChange);

    }

    void OnEvValueChange(bool valueChange)
    {
        if (m_onValueChange != null)
        {
            m_onValueChange(m_id, valueChange, m_trans);
        }
    }

    public void SetOn(bool isOn)
    {
        transform.GetComponent<Toggle>().isOn = isOn;
    }
}
