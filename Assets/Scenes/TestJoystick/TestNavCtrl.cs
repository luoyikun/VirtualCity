using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNavCtrl : MonoBehaviour
{
    public LocalNavMeshBuilder m_navBuild;
    // Start is called before the first frame update
    void Start()
    {
        m_navBuild.StartNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        m_navBuild.UpdateNavMesh();
    }
}
