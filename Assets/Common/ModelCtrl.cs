using Framework.Event;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCtrl : UGUIPanel
{
    [SerializeField] Camera m_cam;
    public Transform m_tar;

    public static ModelCtrl instance;

    public float m_xAngles = 0.0f;
    public float m_yAngles = 0.0f;

    //物体的旋转角度
    float m_rotateSpeed = 10.0f;

    public float m_scale = 1.0f;
    public float m_scaleStep = 0.5f;
    public float m_scaleMax = 10.0f;
    public float m_scaleMin = 1.0f;

    bool m_isRotate = false;
    bool m_isScale = false;
    private float m_yMinLimit = -90;
    private float m_yMaxLimit = 90;

    Vector2 m_oldPos0;
    Vector2 m_oldPos1;


    private float _ZoomDifference = 0;
    private float _ZoomSpeed = 100f;

    public bool m_isEnableRotate = true;
    public bool m_isEnableScale = true;
    public bool m_isEnableClick = true;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.ModelCtrl, OnEvModelCtrl);
       //m_tar.GetComponent<BoxCollider>().center = new Vector3(0.5f,0.5f,0.5f);
    }

    

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.ModelCtrl, OnEvModelCtrl);
    }

    void OnEvModelCtrl(EventData data)
    {
        var exdata = data as EventDataEx<Transform>;
        m_tar = exdata.GetData();
        m_scale = m_tar.localScale.x;
        m_scaleMax = 1.5f * m_scale;
        m_scaleMin = 0.5f * m_scale;
        m_scaleStep = (m_scaleMax - m_scaleMin) / 5.0f;
    }
    // Update is called once per frame
    void Update () {
        m_isRotate = false;
        m_isScale = false;
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 1)
            {
                //单手指移动，模型旋转
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    m_xAngles = Input.GetAxis("Mouse X");
                    m_yAngles = Input.GetAxis("Mouse Y");

                    m_isRotate = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    CamRay();
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                float _zoomTempDifference;

                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    if (m_tar != null)
                    {
                        m_scale = m_tar.localScale.x;
                    }
                }

                if ((touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) && m_tar != null)
                {
                    //缩放
                    _zoomTempDifference = (touch0.position - touch1.position).magnitude;

                    if (_ZoomDifference == 0)
                        _ZoomDifference = _zoomTempDifference;

                    m_scale = m_scale - (_ZoomDifference - _zoomTempDifference) / _ZoomSpeed;
                    _ZoomDifference = _zoomTempDifference;
                    m_scale = Mathf.Clamp(m_scale, m_scaleMin, m_scaleMax);
                }

                if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
                {
                    _ZoomDifference = 0;
                }

                m_isScale = true;
            }
        }
        else 
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                FullRenderScale(false, m_scaleStep);
                m_isScale = true;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                FullRenderScale(true, m_scaleStep);
                m_isScale = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                CamRay();
            }
            else if (Input.GetMouseButton(0))
            {
                m_xAngles = Input.GetAxis("Mouse X");
                m_yAngles = Input.GetAxis("Mouse Y");

                m_isRotate = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (m_isRotate == true && m_isEnableRotate == true && m_tar != null)
        {
            m_tar.Rotate(Vector3.up, -m_xAngles * m_rotateSpeed, Space.World);
            m_tar.Rotate(Vector3.right, m_yAngles * m_rotateSpeed, Space.World);
        }
        if (m_isScale == true && m_isEnableScale == true && m_tar != null)
        {
            m_tar.localScale = new Vector3(m_scale, m_scale, m_scale);
        }
    }

    void CamRay()
    {
        if (m_cam != null)
        {
            RaycastHit hit;
            int layerMask = (1 << DataMgr.m_layerPenDrag) | (1 << DataMgr.m_layerPenMove) | (1 << DataMgr.m_layerPenLimitMove);
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 10000.0f, ~layerMask);
            if (null != hit.transform)
            {
                UseEventDelegate(hit);
            }
        }
    }

    void UseEventDelegate(RaycastHit hitInfo)
    {
        if (hitInfo.transform != null)
        {
            Debug.Log("pen click:" + hitInfo.transform.name);
            if (KEventDelegate.IsHave(hitInfo.transform.gameObject) == true)
            {
                KEventDelegate.Get(hitInfo.transform.gameObject).OnClick(hitInfo.transform.gameObject);
            }
        }
    }

    void FullRenderScale(bool isBig, float scale)
    {
        if (isBig)
        {
            if (m_scale < m_scaleMax)
            {
                m_scale += scale;
            }
        }
        else
        {
            if (m_scale > m_scaleMin)
            {
                m_scale -= scale;
            }
        }

        m_scale = Mathf.Clamp(m_scale, m_scaleMin, m_scaleMax);
    }

}
