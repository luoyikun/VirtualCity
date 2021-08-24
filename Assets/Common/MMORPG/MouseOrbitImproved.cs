using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class MouseOrbitImproved : MonoBehaviour
{
    public bool m_isScale = true;
    public Transform target;
    public float distance = 8.0f;
    public float xSpeed = 70.0f;
    public float ySpeed = 50.0f;

    public float yMinLimit = 0f;
    public float yMaxLimit = 90f;

    public float distanceMin = 8f;
    public float distanceMax = 15f;
    public float zoomSpeed = 0.5f;

    //private Rigidbody rigidbody;

    private float x = 0.0f;
    private float y = 0.0f;

    private float fx = 0f;
    private float fy = 0f;
    private float fDistance = 0;


    int m_fingerId = -1; //  当摇杆移动，控制镜头的手指
    bool m_isClickUi = false;
    Dictionary<int, bool> m_dicTouch = new Dictionary<int, bool>();
    Transform m_trans;
     float t = 0.2f;
    public float m_minDis = 1.0f;
    public bool m_isNear = false;
    // Use this for initialization
    void Start()
    {
        m_trans = this.transform;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        fx = x;
        fy = y;

        //rigidbody = GetComponent<Rigidbody>();

        //// Make the rigid body not change rotation
        //if (rigidbody != null)
        //{
        //    rigidbody.freezeRotation = true;
        //}
        UpdateRotaAndPos();
        fDistance = distance;
    }

    void Update()
    {

        if (Input.touchCount == 0)
        {
            m_fingerId = -1; //当前没有手指
            m_dicTouch.Clear();
        }


        if (Input.touchCount == 1)
        {
            List<int> deleteFinger = new List<int>();
            foreach (var item in m_dicTouch)
            {
                if (item.Key != Input.GetTouch(0).fingerId)
                {
                    deleteFinger.Add(item.Key);
                }
            }

            for (int i = 0; i < deleteFinger.Count; i++)
            {
                //Debug.Log("单指：删除：" + deleteFinger[i]);
                m_dicTouch.Remove(deleteFinger[i]);
            }

            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        //Debug.Log("单指：增加true：" + Input.GetTouch(0).fingerId);
                        m_dicTouch[Input.GetTouch(0).fingerId] = true;
                    }
                    else
                    {
                        //Debug.Log("单指：增加false：" + Input.GetTouch(0).fingerId);
                        m_dicTouch[Input.GetTouch(0).fingerId] = false;
                    }

                    //string debug = "";
                    //foreach (var item in m_dicTouch)
                    //{
                    //    debug += item.Key;
                    //    debug += ":";
                    //    debug += item.Value;
                    //    debug += "---";
                    //}
                    //Debug.Log(debug);
                    break;
                case TouchPhase.Moved:

                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
          
        }

        if (Input.touchCount == 2)
        {
            List<int> deleteFinger = new List<int>();

            foreach (var item in m_dicTouch)
            {
                bool isDelete = true;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (item.Key == Input.touches[i].fingerId)
                    {
                        isDelete = false;
                        break;
                    }
                }
                if (isDelete == true)
                {
                    deleteFinger.Add(item.Key);
                }
            }

            for (int i = 0; i < deleteFinger.Count; i++)
            {
                //Debug.Log("双指：删除：" + deleteFinger[i]);
                m_dicTouch.Remove(deleteFinger[i]);
            }

            for (int i = 0; i < Input.touchCount; i++)
            {
                switch (Input.touches[i].phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                        {
                            //Debug.Log("双指：增加true：" + Input.touches[i].fingerId);
                            m_dicTouch[Input.touches[i].fingerId] = true;

                        }
                        else
                        {
                            //Debug.Log("双指：增加false：" + Input.touches[i].fingerId);
                            m_dicTouch[Input.touches[i].fingerId] = false;
                        }
                        break;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }
           
            if (IsCanScale() == true && m_isScale == true)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                fDistance = Mathf.Clamp(distance + deltaMagnitudeDiff * zoomSpeed, distanceMin, distanceMax);
            }

        }

        if (Application.isMobilePlatform == false && m_isScale == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                fDistance = Mathf.Clamp(distance - zoomSpeed, distanceMin, distanceMax);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                fDistance = Mathf.Clamp(distance + zoomSpeed, distanceMin, distanceMax);
            }
        }

        distance = Mathf.Lerp(distance, fDistance, 0.25f);
    }

    void LateUpdate()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.GetMouseButton(0) && IsCanRotate())
            {
                Touch input = new Touch();
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].fingerId == m_fingerId)
                    {
                        input = Input.touches[i];
                    }
                }
                if (target)
                {
                    float dx = Input.GetAxis("Mouse X");
                    float dy = Input.GetAxis("Mouse Y");
                    if (Input.touchCount > 0)
                    {
                        dx = input.deltaPosition.x;
                        dy = input.deltaPosition.y;
                    }

                    x += dx * xSpeed * Time.deltaTime;//*distance
                    y -= dy * ySpeed * Time.deltaTime;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    m_isClickUi = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_isClickUi = false;
            }
            if (m_isClickUi == false && Input.GetMouseButton(0))
            {
                if (target)
                {
                    float dx = Input.GetAxis("Mouse X");
                    float dy = Input.GetAxis("Mouse Y");


                    x += dx * xSpeed * 20 * Time.deltaTime;//*distance
                    y -= dy * ySpeed * 20* Time.deltaTime;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                }
            }
        
        
        }


        fx = Mathf.Lerp(fx, x, 0.2f);
        fy = Mathf.Lerp(fy, y, 0.2f);

        UpdateRotaAndPos();
    }

    bool m_isLastHit = false;
    void UpdateRotaAndPos()
    {
        if (target)
        {
            bool isCurHit = false;
            Quaternion rotation = Quaternion.Euler(fy, fx, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            Vector3 canSetPos = position;

            //m_trans.position = position;

            //Debug.DrawLine(target.position, m_trans.position, Color.red);
            //主角朝着这个方向发射射线
            RaycastHit hit;


            if (Physics.Linecast(target.position, position, out hit))
            {         
                Debug.Log("摄像机碰到:" + hit.collider.gameObject);
     
                //当碰撞的不是摄像机也不是地形 那么直接移动摄像机的坐标
                Vector3 posHit = hit.point;
                Vector3 dir = posHit - target.position;
                //Vector3 posEnd = posHit - dir * 0.5f;
                canSetPos = posHit - dir * 0.5f;
                isCurHit = true;
                //m_trans.position = posEnd;
                //m_trans.position = position;  
            }

            //摄像机与地面的检测，不要穿过地
            if (canSetPos.y < target.parent.position.y)
            {
                Debug.Log("摄像机碰到了地");
                canSetPos.y = target.parent.position.y;

                Vector3 relativePos = target.position - canSetPos;
                rotation = Quaternion.LookRotation(relativePos);
                isCurHit = true;
            }

            //float oldDis = (m_trans.position - target.position).sqrMagnitude;

            //float newDis = (canSetPos - target.position).sqrMagnitude;

            //if ((isCurHit == false && m_isLastHit == false) || (isCurHit == true && m_isLastHit == false) || (isCurHit == true && m_isLastHit == true)) //拉进是突进
            //{
            //    m_trans.position = canSetPos;
            //}
            //else if (m_isLastHit == true && isCurHit == false)
            //{
            //    Debug.Log("镜头远离墙体");
            //    Vector3 mainCamPos = m_trans.position;
            //    Vector3 newPos = Vector3.Lerp(mainCamPos, canSetPos, getCurrentDeltaTime() * movementLerpSpeed);
            //    m_trans.position = newPos;
            //}

            m_trans.position = canSetPos;
            m_trans.rotation = rotation;
            m_isLastHit = isCurHit;
        }
    }
    float movementLerpSpeed =0.1f;
    float currentDeltaTime;
    public float getCurrentDeltaTime()
    {
        currentDeltaTime = Time.deltaTime;


        return currentDeltaTime;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    bool IsCanRotate()
    {
        bool ret = false;
        if (m_dicTouch.Count == 1)
        {
            foreach (var item in m_dicTouch)
            {
                if (item.Value == false)
                {
                    ret = true;
                    m_fingerId = item.Key;
                }
            }
        }
        else {
            int inUI = 0;
            int outUI = 0;
            foreach (var item in m_dicTouch)
            {
                if (item.Value == true)
                {
                    inUI++;
                }
                else {
                    outUI++;
                    m_fingerId = item.Key;
                }
            }
            if (inUI == 1 && outUI == 1)
            {
                ret = true;
            }
        }

        return ret;
    }

    bool IsCanScale()
    {
        bool ret = true;
        if (m_dicTouch.Count == 2)
        {
            foreach (var item in m_dicTouch)
            {
                if (item.Value == true)
                {
                    ret = false;
                    break;
                }
            }
        }
        return ret;
    }
}
