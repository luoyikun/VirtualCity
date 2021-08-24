using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DeleteAnimator : Editor {

    [MenuItem("SceneTools/CreatePrefab")]
    public static void CreatePrefab()
    {
        GameObject obj = Selection.activeGameObject;

        Object tempPrefab;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/AssetsPackage/HomeUnitModels/" + obj.transform.GetChild(i).name + ".prefab");
            PrefabUtility.ReplacePrefab(obj.transform.GetChild(i).gameObject, tempPrefab);
        }

    }

    [MenuItem("SceneTools/删除登陆信息")]
    public static void DeleteLogin()
    {
        if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
        {
            File.Delete(AppConst.LocalPath + "/Rsa.txt");
        }

        if (File.Exists(AppConst.LocalPath + "/login.mi"))
        {
            File.Delete(AppConst.LocalPath + "/login.mi");
        }
    }

    [MenuItem("SceneTools/DeleteAnimator")]
    static void tranPoint()
    {
        int AnimatorCount = 0;
        //建立一个List数组
        List<GameObject> SceneRoot = new List<GameObject>();
        //得到场景所有的根节点，并返回给上面的字典
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects(SceneRoot);

        //遍历字典
        for (int i = 0; i < SceneRoot.Count; i++)
        {
            //将SceneRoot[i]当作每个父物体 进行遍历 得到每个根节点的所有子物体transform组件
            Transform[] m_Transform = SceneRoot[i].transform.GetComponentsInChildren<Transform>();
            //GetComponentsInChildren:得到自身 与 子物体（包含子物体中的子物体）

            //List<Transform> m_Transform = new List<Transform>();
            //foreach (Transform t in SceneRoot[i].transform)
            //{
            //    m_Transform.Add(t);
            //}
            //这种方法得到的数组，就是只有子物体（不包含父物体，也不包含子物体中的子物体）


            Debug.Log(m_Transform.Length);
            //遍历每个子物体中的Animator状况
            for (int j = 0; j < m_Transform.Length; j++)
            {

                Animator s = m_Transform[j].GetComponent<Animator>();
                if (m_Transform[j].GetComponent<Animator>())
                {
                    if (s.runtimeAnimatorController == null)
                    {
                        //编辑器只能用DestroyImmediate，不然无效或报错
                        DestroyImmediate(s);
                        AnimatorCount++;
                    }
                }
            }
        }
        //将场景设置为dirty，为了删除后保存场景，这个在做编辑器的时候经常要用到
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        Debug.Log("已经删除了" + "" + AnimatorCount + "" + "个空Anirmator组件");
    }  

}
