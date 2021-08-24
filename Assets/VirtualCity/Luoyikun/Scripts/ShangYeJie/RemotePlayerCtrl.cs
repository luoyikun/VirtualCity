using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemotePlayerCtrl : MonoBehaviour {
    public NavMeshAgent m_nav;
    public Animator m_ani;
    public Player m_playerInfo = new Player();
	// Use this for initialization
	void Start () {
        m_nav = transform.GetComponent<NavMeshAgent>();
        m_ani = transform.GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        if (m_nav != null && m_ani != null)
        {
            if (m_nav.remainingDistance > 0)
            {
                m_ani.SetBool("IsRun", true);
            }
            else {
                m_ani.SetBool("IsRun", false);
            }
        }
	}
}
