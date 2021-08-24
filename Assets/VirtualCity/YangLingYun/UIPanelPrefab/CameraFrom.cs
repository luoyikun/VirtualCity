using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraFrom : MonoBehaviour {
    public GameObject Character;
    public GameObject Camera;
    public GameObject RotateObj;
    //public GameObject scrollBar;
    bool m_isClickUi = false;
    // Use this for initialization
    // Use this for initialization
    void Start () {
        //transform.position = Character.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
    }
    private void LateUpdate()
    {

        transform.position = Character.transform.position + new Vector3(0, 2, 0);
        if (Input.GetMouseButtonDown(0))
        {
            if (Application.isMobilePlatform && Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    {
                        m_isClickUi = true;
                        break;
                    }
                }
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                m_isClickUi = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_isClickUi = false;
        }
        if (m_isClickUi == true)
        {
            return;
        }

    }
}
