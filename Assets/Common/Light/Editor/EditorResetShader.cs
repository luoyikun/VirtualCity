using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorResetShader : Editor {

    [MenuItem("LightTool/DoAddMat")]
    public static void DoAddMat()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj.GetComponent<ResetShader>() == null)
        {
            obj.AddComponent<ResetShader>();
        }
        ResetShader retS = obj.GetComponent<ResetShader>();
        retS.m_listRend.Clear();

        foreach (var it in obj.transform.GetComponentsInChildren<Renderer>())
        {
            //if (it.gameObject.GetComponent<Part>)
            retS.m_listRend.Add(it);
        }

        ChageMobileDiffuse();
    }

    [MenuItem("LightTool/ChageMobileDiffuse")]
    public static void ChageMobileDiffuse()
    {
        GameObject obj = Selection.activeGameObject;
       

        foreach (var it in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (it.sharedMaterials != null)
            {
                try
                {
                    if (it.gameObject.GetComponent<ParticleSystem>() == null)
                    {
                        for (int i = 0; i < it.sharedMaterials.Length; i++)
                        {
                            if (it.sharedMaterials[i].shader != null && it.sharedMaterials[i].shader.name == "Standard")
                            {
                                Debug.Log(it.name + ":shader.name:" + it.sharedMaterials[i].shader.name);
                                it.sharedMaterials[i].shader = Shader.Find("Mobile/Diffuse");
                            }
                        }
                    }
                }
                catch
                {
                    Debug.LogError(it.name + " error");
                }
            }
        }
    }
}
