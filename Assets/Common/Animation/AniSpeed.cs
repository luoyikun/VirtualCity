using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSpeed : MonoBehaviour
{
    Animation m_ani;
    public float m_speed = 1.0f;
    // Use this for initialization
    void Start()
    {
        m_ani = transform.GetComponent<Animation>();
        foreach (AnimationState state in m_ani)
        {
            state.speed = m_speed;
        }
    }

    private void OnBecameVisible()
    {
        if (m_ani != null)
        {
            m_ani.enabled = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (m_ani != null)
        {
            m_ani.enabled = false;
        }
    }
}
