using Framework.Event;
using Framework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rete_mx : UGUIPanel
{

	// Use this for initialization
	
    public static GameObject moxing;
    bool io;
    int speed;
    public Transform m_tarPar;
    public ModelCtrl m_modelCtrl;

    //public string []shader_Route;

    Dictionary<int, string[]> shader_Route = new Dictionary<int, string[]>();
    Dictionary<int, List<Material>> shader_Renar = new Dictionary<int, List<Material>>();

    void Start()
    {
        // Pos = moxing.transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        if (io&& moxing != null)
        {
            moxing.transform.Rotate(new Vector3(0, speed, 0), 1.5f);
        }
    }

    public void kaiguan(bool io_)
    {
        io= io_;
    }
    public void AniS(int num)
    {
        speed = num;
    }
    public void kaiguan()
    {
        if (moxing!=null) {
            for (int i = 0; i < shader_Renar.Count; i++)
            {
                Debug.Log(shader_Route[i]);
                //moxing.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find(shader_Route[i]);
                string[] arr = shader_Route[i];
                List<Material> arr_ = shader_Renar[i];
                for (int j = 0; j < arr.Length; j++)
                {
                    arr_[j].shader= Shader.Find(arr[j]);
                }
            }
            shader_Route.Clear();
            shader_Renar.Clear();
            Destroy(moxing);
        }
        //Debug.LogError("Shader被改回来了");
        UIManager.Instance.PopSelf(false);
        jiajushangchengpanel.kaiguai = false;
        Debug.Log(jiajushangchengpanel.kaiguai);
    }

    public void CreateModel(string modelName,float maxscale,float minscale)
    {

        Debug.Log(maxscale);
        Debug.Log(minscale);
        m_tarPar.localRotation = new Quaternion(0, 0, 0, 0);
        AssetMgr.Instance.CreateObj(modelName, modelName, m_tarPar, Vector3.zero, Vector3.zero, new Vector3(-10000, 0, 0), (obj) =>
        {
            float x;
            x = obj.GetComponent<BoxCollider>().size.y;
            Debug.Log(x);
            m_tarPar.localScale = new Vector3(minscale, minscale, minscale);
            ModelCtrl modelCtrl = transform.GetComponent<ModelCtrl>();
            modelCtrl.m_scaleMax = maxscale;
            modelCtrl.m_scaleMin = minscale;
            obj.transform.localPosition = new Vector3(0f, -(x / 2)*obj.transform.localScale.y, 0f);
            m_modelCtrl.m_tar = m_tarPar;
            try
            {
               // shader_Route= new string[obj.transform.childCount];
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    //  shader_Route[i]= obj.transform.GetChild(i).GetComponent<Renderer>().material.shader.name;
                    Debug.Log(obj.transform.GetChild(i).name);
                    Renderer[] Renar = obj.transform.GetChild(i).GetComponentsInChildren<Renderer>();
                   
                    List<Material> m_listMat = new List<Material>();
                    for (int k = 0; k < Renar.Length; k++)
                    {
                        for (int l = 0; l < Renar[k].materials.Length; l++)
                        {
                            m_listMat.Add(Renar[k].materials[l]);
                        }
                    }
                    string[] shader_ = new string[m_listMat.Count];
                    for (int j = 0; j < m_listMat.Count; j++)
                    {
                        if (m_listMat[j].shader.name == "Legacy Shaders/Transparent/Diffuse"||
                            m_listMat[j].shader.name == "Legacy Shaders/Transparent/Cutout/Diffuse")
                        {
                            shader_[j] = m_listMat[j].shader.name;
                            m_listMat[j].shader = Shader.Find("Unlit/Transparent Cutout");
                          //  Debug.LogError("Shader修改成功了");
                        }
                    }
                    shader_Route.Add(i,shader_);
                    shader_Renar.Add(i, m_listMat);
                  // obj.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Transparent Cutout");
               
                    if (obj.transform.GetChild(i).name== "shadow")
                    {
                        obj.transform.GetChild(i).gameObject.SetActive(false);
                    }
                  //  Debug.Log(obj.transform.GetChild(i).GetComponent<Renderer>().material.shader.name);
                }
            }
            catch
            {
                Debug.Log("");
            }
            rete_mx.moxing = obj;
        }
        );
    }


}
