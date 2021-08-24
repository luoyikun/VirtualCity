using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("ForeachAllChild");
        StopCoroutine("ForeachAllChild");
	}
    IEnumerator ForeachAllChild()
    {
        foreach (Transform tran in transform.GetComponentsInChildren<Transform>())
        {
            if (tran.GetComponent<MeshRenderer>() != null)
            {
                for (int i = 0; i < tran.GetComponent<MeshRenderer>().materials.Length; i++)
                {
                    Material m_TargetMaterial = tran.GetComponent<MeshRenderer>().materials[i];
                    Shader m_TargetShader = m_TargetMaterial.shader;
                    m_TargetMaterial.shader = Shader.Find(m_TargetShader.name);
                }
            }
        }
        return null;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
