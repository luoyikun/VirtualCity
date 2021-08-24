using Framework.UI;
using ProtoDefine;
using SGF.Codec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cameratest : MonoBehaviour {
    public Transform target;
    public Vector2 sensitivity = new Vector2(3, 3);
    private Vector2 _input_rotation;
    private Quaternion _rotation;
    public float minAngle = -90;
    public float maxAngle = 90;
    public Vector2 originRotation = new Vector2();
    bool IsDrag=false;
    private Transform _t;
    public float distance = 5;
    public float minDistance = 0;
    public float maxDistance = 10;
    public Vector2 targetOffset = new Vector2();
    private float _wanted_distance;
    public float zoomSpeed = 1;
    public float zoomSmoothing = 16;
    bool m_isClickUi = false;
    bool Dragged;
    private Vector3 mousePosLast = Vector3.zero;
    private float ClickAfter = 0.0f;
    private bool bTemporarySelect = false;
    float value = 0;
    // Use this for initialization
    void Start () {
        _t = transform;
        _wanted_distance = distance;
        //_input_rotation = originRotation;
    }
    private void OnEnable()
    {
        NetEventManager.Instance.AddEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
    }
    private void OnDisable()
    {
        NetEventManager.Instance.RemoveEventListener(MsgIdDefine.RspGetPlayerByIdMessage, OnGetPlayerByIdMessage);
    }
    void OnGetPlayerByIdMessage(byte[] buf)
    {
        RspGetPlayerByIdMessage RspGPBIM = PBSerializer.NDeserialize<RspGetPlayerByIdMessage>(buf);
        UIManager.Instance.PushPanel(UIPanelName.userinfopanel, false, true, (paragm) => { paragm.GetComponent<userinfopanel>().Init(RspGPBIM.users[0]); });
    }
    // Update is called once per frame
    void LateUpdate () {
        if (Input.GetMouseButton(0))
        {
            _input_rotation = originRotation;
            ClickAfter = 0.0f;
            mousePosLast = Input.mousePosition;
            Dragged = false;
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
            RayHitPlayer();
            m_isClickUi = false;
        }
        if (m_isClickUi == true)
        {
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _wanted_distance += zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _wanted_distance -= zoomSpeed;
        }
        if (Input.touchCount > 1)
        {
            float distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) ;
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (distance < Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position))
                {
                    _wanted_distance -= zoomSpeed;
                }
                else if (distance < Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position))
                {
                    _wanted_distance += zoomSpeed;
                }
            }
        }
        _wanted_distance = Mathf.Clamp(_wanted_distance, minDistance, maxDistance);
        if (Input.GetMouseButton(0))
        {
            //_input_rotation = new Vector2();
                    _input_rotation.x += Input.GetAxis("Mouse X") * sensitivity.x;
                ClampRotation();
                    _input_rotation.y -= Input.GetAxis("Mouse Y") * sensitivity.y;
                _input_rotation.y = Mathf.Clamp(_input_rotation.y, minAngle, maxAngle);
                _rotation = Quaternion.Euler(_input_rotation.y, _input_rotation.x, 0);
                originRotation = _input_rotation;
            _t.rotation = _rotation;
        }
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                _input_rotation.x += Input.GetAxis("Mouse X") * sensitivity.x;
                ClampRotation();
                _input_rotation.y -= Input.GetAxis("Mouse Y") * sensitivity.y;
                _input_rotation.y = Mathf.Clamp(_input_rotation.y, minAngle, maxAngle);
                _rotation = Quaternion.Euler(_input_rotation.y, _input_rotation.x, 0);
                originRotation = _input_rotation;
                _t.rotation = _rotation;
            }
        }
        distance = Mathf.Clamp(Mathf.Lerp(distance, _wanted_distance, Time.deltaTime * zoomSmoothing), minDistance, maxDistance);
        _t.position = _rotation * new Vector3(targetOffset.x, 0.0f, -distance) + target.position + new Vector3(0, targetOffset.y, 0);
        //distance = Mathf.Clamp(Mathf.Lerp(distance, _wanted_distance, Time.deltaTime * zoomSmoothing), minDistance, maxDistance);
        //Vector3 wanted_position = _rotation * new Vector3(targetOffset.x, 0, -_wanted_distance - 0.2f) + target.position + new Vector3(0, targetOffset.y, 0);
        //Vector3 current_position = _rotation * new Vector3(targetOffset.x, 0, 0) + target.position + new Vector3(0, targetOffset.y, 0);
    }
    public void RayHitPlayer()
    {
        Ray ray = this.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "WanJia")
            {
                Debug.Log(hit.collider.gameObject.name);
                //ReqGetPlayerByIdMessage ReqGPBIM = new ReqGetPlayerByIdMessage();
                //ReqGPBIM.accountIds = new List<long?>();
                //ReqGPBIM.accountIds.Add(long.Parse(hit.collider.gameObject.name));
                //ChatSocket.Instance.SendMsgProto(MsgIdDefine.ReqGetPlayerByIdMessage, ReqGPBIM, EnSocket.Chat);
            }
        }
    }
    private void ClampRotation()
    {
        if (originRotation.x < -180)
        {
            originRotation.x += 360;
        }
        else if (originRotation.x > 180)
        {
            originRotation.x -= 360;
        }

        if (_input_rotation.x - originRotation.x < -180)
        {
            _input_rotation.x += 360;
        }
        else if (_input_rotation.x - originRotation.x > 180)
        {
            _input_rotation.x -= 360;
        }
    }
}
