using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGlint : MonoBehaviour {

    /// <summary>
    /// 闪烁颜色
    /// </summary>
    public Color m_color = new Color(1, 0, 1, 1);

    float m_rate = 0.5f;

    List<Material> m_listMat = new List<Material>();
    Dictionary<Material, Color> m_dicColor = new Dictionary<Material, Color>();
     bool m_isDoing = false;
    private Coroutine m_glinting;

    public bool m_autoStart = false;


    // Use this for initialization
    void Start () {
        Renderer[] listRender = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < listRender.Length; i++)
        {
            for (int j = 0; j < listRender[i].materials.Length; j++)
            {
                m_listMat.Add(listRender[i].materials[j]);
            }
        }

        for (int i = 0; i < m_listMat.Count; i++)
        {
            m_dicColor[m_listMat[i]] = m_listMat[i].GetColor("_Color");
        }

        if (m_autoStart == true)
        {
            StartGlinting();
        }
    }

    private void OnDestroy()
    {
        StopGlinting();
    }
    public void SetColor(Color color)
    {
        m_color = color;

        if (m_glinting == null)
        {
            m_glinting = StartCoroutine(IEGlinting());
        }
    }
    public void StartGlinting()
    {
        //if (m_isDoing == true)
        //{
        //    return;
        //}
        //m_isDoing = true;


        if (m_glinting != null)
        {
            StopCoroutine(m_glinting);
        }
        m_glinting = StartCoroutine(IEGlinting());
    }

    public void StopGlinting()
    {
        //if (m_isDoing == false)
        //{
        //    return;
        //}
        //m_isDoing = false;

        if (m_glinting != null)
        {
            StopCoroutine(m_glinting);
            m_glinting = null;
        }

        for (int i = 0; i < m_listMat.Count; i++)
        {
            m_listMat[i].SetColor("_Color", m_dicColor[m_listMat[i]]);
        }

    }
    private IEnumerator IEGlinting()
    {
        Color newColor = Color.white;
        while (true)
        {
            for (int i = 0; i < m_listMat.Count; i++)
            {
                m_listMat[i].SetColor("_Color", m_dicColor[m_listMat[i]]);
            }
            
            //_renderer.UpdateGIMaterials();
            yield return new WaitForSeconds(m_rate) ;


            for (int i = 0; i < m_listMat.Count; i++)
            {
                m_listMat[i].SetColor("_Color", m_color);
            }

            //_renderer.UpdateGIMaterials();
            yield return new WaitForSeconds(m_rate);
        }
    }
}
