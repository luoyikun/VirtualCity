using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetShader : MonoBehaviour {
    public List<Renderer> m_listRend = new List<Renderer>();
    // Use this for initialization
    private void Awake()
    {
        //UpdateShader();
        StartCoroutine(YieldUpdate());
    }

    IEnumerator YieldUpdate()
    {
        UpdateShader();
        yield return null;
    }
    void UpdateShader()
    {

        for (int i = 0; i < m_listRend.Count; i++)
        {
            if (m_listRend[i] != null)
            {
                for (int j = 0; j < m_listRend[i].materials.Length; j++)
                {
                    Material material = m_listRend[i].materials[j];

                    Shader shader = material.shader;
                    //Debug.Log("shader.name:" + shader.name);
                    material.shader = Shader.Find(shader.name);
                }
            }
        }
    }
}
