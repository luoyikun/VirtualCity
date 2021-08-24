using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour {
    Transform m_tarns;
	// Use this for initialization
	void Start () {
        m_tarns = this.transform;

    }
	
	// Update is called once per frame
	void Update () {
        m_tarns.Rotate(Vector3.forward * 25 * Time.deltaTime, Space.Self);
    }
}
