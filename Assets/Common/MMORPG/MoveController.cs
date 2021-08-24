using UnityEngine;
using System.Collections;
using Framework.Event;

public class MoveController : MonoBehaviour {

    public Transform m_camera;

    Animator ani;

    public UnityEngine.AI.NavMeshAgent nav;

    float h, v;

    Vector3 moveVec;

    public VariableJoystick m_joy;
    public bool m_isJoyCtrl = true;
    Vector3 m_toPos;
    Transform m_trans;
    float m_speed = 4;
    // Use this for initialization
    void Start () {

        m_trans = this.transform;
        ani = GetComponent<Animator>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        EventManager.Instance.AddEventListener(Common.EventStr.StopNav, OnEvStopNav);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.StopNav, OnEvStopNav);
    }

    void OnEvStopNav(EventData data)
    {
        StopNav();
    }
    public void SetPointByNav(Vector3 pos)
    {
        m_isJoyCtrl = false;
        m_toPos = pos;
        nav.SetDestination(pos);
        ani.SetBool("IsRun", true);
        //nav.isStopped
    }

	// Update is called once per frame
	void Update () {

        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");
        if (m_joy != null)
        {
            h = m_joy.Horizontal;
            v = m_joy.Vertical;
            moveVec = new Vector3(h, 0, v);

            if (h != 0 || v != 0)
            {
                m_isJoyCtrl = true;
                nav.ResetPath();
                ani.SetBool("IsRun", true);
                // 根据摄像机方向 进行移动
                moveVec = Quaternion.Euler(0, m_camera.eulerAngles.y, 0) * moveVec;
                nav.Move(moveVec.normalized * Time.deltaTime * m_speed);
                RotatePlayer();
            }
            else
            {
                if (m_isJoyCtrl == true)
                {
                    ani.SetBool("IsRun", false);
                }
            }
        }

        if (m_isJoyCtrl == false)
        {
            if (nav.remainingDistance != 0 && nav.remainingDistance < 0.5f)
            {
                StopNav();
            }
        }

        //if (m_isJoyCtrl == false)
        //{
        //    if (Vector3.Distance(m_trans.position, m_toPos) > 0.5f)
        //    {
        //        ani.SetBool("IsRun", true);
        //    }
        //    else {
        //        m_isJoyCtrl = true;
        //        ani.SetBool("IsRun", false);
        //        nav.ResetPath();
        //    }
        //}
	}

    public void StopNav()
    {
        if (nav.enabled == true && nav.isOnNavMesh)
        {
            nav.ResetPath();
        }
        ani.SetBool("IsRun", false);
        m_isJoyCtrl = true;
    }
    private void RotatePlayer()
    {
        //向量v围绕y轴旋转cameraAngle.y度
        Vector3 vec = Quaternion.Euler(0, 0, 0) * moveVec;
        Quaternion qua = Quaternion.LookRotation(vec);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * 100);
    }


    public bool IsMoving()
    {
        // -> agent.hasPath will be true if stopping distance > 0, so we can't
        //    really rely on that.
        // -> pathPending is true while calculating the path, which is good
        // -> remainingDistance is the distance to the last path point, so it
        //    also works when clicking somewhere onto a obstacle that isn'
        //    directly reachable.
        return nav.pathPending ||
       nav.remainingDistance > nav.stoppingDistance ||
       nav.velocity != Vector3.zero;
    }


    void OnDrawGizmos()
    {
        var path = nav.path;
        // color depends on status
        Color c = Color.white;
        switch (path.status)
        {
            case UnityEngine.AI.NavMeshPathStatus.PathComplete: c = Color.white; break;
            case UnityEngine.AI.NavMeshPathStatus.PathInvalid: c = Color.red; break;
            case UnityEngine.AI.NavMeshPathStatus.PathPartial: c = Color.yellow; break;
        }
        // draw the path
        for (int i = 1; i < path.corners.Length; ++i)
            Debug.DrawLine(path.corners[i - 1], path.corners[i], c);
    }


}
