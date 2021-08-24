using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;

using System.Diagnostics;
using System;
using LitJson;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System.Text;
using ProtoDefine;
using System.Linq;

public class PublicFunc : MonoBehaviour
{

    static PublicFunc m_instance = null;
    static GameObject m_publicObj = null;
    static Coroutine m_corUiPlaneMove = null;
    static Coroutine m_corCreateFromRes = null;//异步创建obj
    public static PublicFunc Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_publicObj = new GameObject();
                m_publicObj.name = "PublicFunc";
                m_instance = m_publicObj.AddComponent<PublicFunc>();
            }
            return m_instance;
        }
    }

    public static void AddMeshCollider(GameObject obj)
    {
        foreach (var render in obj.transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (render.gameObject.GetComponent<MeshCollider>() == null)
            render.gameObject.AddComponent<MeshCollider>();
        }

        foreach (var skinRender in obj.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (skinRender.gameObject.GetComponent<MeshCollider>() == null)
                skinRender.gameObject.AddComponent<MeshCollider>();
        }
    }


    public static void RemoveMeshCollider(GameObject obj)
    {
        foreach (var render in obj.transform.GetComponentsInChildren<MeshCollider>())
        {
            Destroy(render);
        }
    }
    public static void ChangeTag(GameObject obj, string sTag)
    {
        //给未赋值的放tag
        if (obj.gameObject.tag == DataMgr.m_tagUntagged)
            obj.tag = sTag;
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            if (trans.gameObject.tag == DataMgr.m_tagUntagged)
                trans.gameObject.tag = sTag;
        }
    }

    public static void ChangeTagUntagged(GameObject obj)
    {
        obj.tag = DataMgr.m_tagUntagged;
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
                trans.gameObject.tag = DataMgr.m_tagUntagged;
        }
    }

    public static void ChangeTagForce(GameObject obj, string sTag)
    {
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            trans.gameObject.tag = sTag;
        }
    }

    public static void ChangeLayerForce(GameObject obj, int sTag)
    {
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            trans.gameObject.layer = sTag;
        }
    }

    public static void ObjSetDrag(GameObject obj)
    {
        ChangeTag(obj, DataMgr.m_tagDrag);
        AddMeshCollider(obj);
    }
    public static void SaveOriPos(GameObject obj)
    {
        DataMgr.m_listHideObj.Clear();
        DataMgr.m_dicOriPos.Clear();
        DataMgr.m_dicOriQua.Clear();
        DataMgr.m_dicOriLocalPos.Clear();
        DataMgr.m_dicOriLocalQua.Clear();
        DataMgr.m_dicOriLocalScale.Clear();

        DataMgr.m_dicOriPos[obj] = obj.transform.position;
        DataMgr.m_dicOriQua[obj] = obj.transform.rotation;
        DataMgr.m_dicOriLocalScale[obj] = obj.transform.localScale;
        DataMgr.m_dicOriLocalPos[obj] = obj.transform.localPosition;
        DataMgr.m_dicOriLocalQua[obj] = obj.transform.localRotation;

        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {

            DataMgr.m_dicOriPos[trans.gameObject] = trans.position;
            DataMgr.m_dicOriQua[trans.gameObject] = trans.rotation;

            DataMgr.m_dicOriLocalPos[trans.gameObject] = trans.localPosition;
            DataMgr.m_dicOriLocalQua[trans.gameObject] = trans.localRotation;
            DataMgr.m_dicOriLocalScale[trans.gameObject] = trans.localScale;
        }
    }
    public static double GetDicimals(double oriDouble)
    {
        if (oriDouble == 0)
        {
            return oriDouble;
        }

        //获取小数点后的长度
        string oriDoubleString = (oriDouble.ToString()).Substring(oriDouble.ToString().IndexOf(".")).Replace(".", "");
        if (oriDoubleString.Length == 0)
        {
            return oriDouble;
        }
        //去掉所有小数点，为转成整型做准备
        double targetDouble = oriDouble;
        for (int i = 0; i < oriDoubleString.Length; i++)
        {
            targetDouble = targetDouble * 10;
        }
        //保留两位小数点，其他全部除掉
        long targetLong = (long)targetDouble;
        for (int i = 0; i < oriDoubleString.Length - 2; i++)
        {
            targetLong = targetLong / 10;
        }

        //将保留下来的数转成浮点，并且除两次，获得小数点后两位的数据
        targetDouble = targetLong;
        for (int i = 0; i < 2; i++)
        {
            targetDouble = targetDouble / 10;
        }
        return targetDouble;
    }
    public static double GetDicimals(double oriDouble,int dicimalsNumber)
    {
        if (oriDouble == 0)
        {
            return oriDouble;
        }

        //获取小数点后的长度
        string oriDoubleString = (oriDouble.ToString()).Substring(oriDouble.ToString().IndexOf(".")).Replace(".","");
        if (oriDoubleString.Length == 0)
        {
            return oriDouble;
        }

        //去掉所有小数点，为转成整型做准备
        double targetDouble = oriDouble;
        for (int i = 0; i < oriDoubleString.Length; i++)
        {
            targetDouble = targetDouble * 10;
        }

        //保留小数点，其他全部除掉
        long targetLong = (long)targetDouble;
        for (int i = 0; i < oriDoubleString.Length-dicimalsNumber; i++)
        {
            targetLong = targetLong / 10;
        }

        //将保留下来的数转成浮点，获得小数点后的数据
        targetDouble = targetLong;
        for (int i = 0; i < dicimalsNumber; i++)
        {
            targetDouble = targetDouble / 10;
        }
        return targetDouble;
    }

    public static void SaveOriPosDotween(GameObject obj)
    {
        DataMgr.m_listHideObj.Clear();

        DataMgr.m_dicOriLocalPos.Clear();
        DataMgr.m_dicOriLocalEuler.Clear();
        DataMgr.m_dicOriLocalScale.Clear();
        DataMgr.m_dicOriLocalQua.Clear();

        DataMgr.m_dicOriPos.Clear();
        DataMgr.m_dicOriEuler.Clear();
        DataMgr.m_dicOriScale.Clear();
        DataMgr.m_dicOriQua.Clear();
        //DataMgr.m_dicOriPos[obj] = obj.transform.position;
        //DataMgr.m_dicOriQua[obj] = obj.transform.rotation;
        //DataMgr.m_dicOriLocalScale[obj] = obj.transform.localScale;
        //DataMgr.m_dicOriLocalPos[obj] = obj.transform.localPosition;
        //DataMgr.m_dicOriLocalQua[obj] = obj.transform.localRotation;

        DataMgr.m_dicOriLocalPos[obj] = obj.transform.localPosition;
        DataMgr.m_dicOriLocalEuler[obj] = obj.transform.localEulerAngles;
        DataMgr.m_dicOriLocalScale[obj] = obj.transform.localScale;
        DataMgr.m_dicOriLocalQua[obj] = obj.transform.localRotation;

        DataMgr.m_dicOriPos[obj] = obj.transform.position;
        DataMgr.m_dicOriEuler[obj] = obj.transform.eulerAngles;
        DataMgr.m_dicOriScale[obj] = obj.transform.localScale;
        DataMgr.m_dicOriQua[obj] = obj.transform.rotation;

        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {

            //DataMgr.m_dicOriPos[trans.gameObject] = trans.position;
            //DataMgr.m_dicOriQua[trans.gameObject] = trans.rotation;

            DataMgr.m_dicOriLocalPos[trans.gameObject] = trans.localPosition;
            DataMgr.m_dicOriLocalEuler[trans.gameObject] = trans.localEulerAngles;
            DataMgr.m_dicOriLocalScale[trans.gameObject] = trans.localScale;
            DataMgr.m_dicOriLocalQua[trans.gameObject] = trans.localRotation;

            DataMgr.m_dicOriPos[trans.gameObject] = trans.position;
            DataMgr.m_dicOriEuler[trans.gameObject] = trans.eulerAngles;
            DataMgr.m_dicOriScale[trans.gameObject] = trans.localScale;
            DataMgr.m_dicOriQua[trans.gameObject] = trans.rotation;
        }
    }

    //全体差值还原，包括一开始的位置，方向，缩放
    public static void RestoreOriPos(GameObject obj, float fTime = 0.01f)
    {
        //AllChildSetActive(obj);
        //Instance.StopCoroutine()
        for (int i = 0; i < DataMgr.m_listHideObj.Count; i++)
        {
            DataMgr.m_listHideObj[i].SetActive(true);
        }
        Instance.StopCoroutine("YieldRestoreOriPos");
        Instance.StartCoroutine("YieldRestoreOriPos", obj);//
        //Instance.StartCoroutine (Instance.YieldRestoreOriPos (obj));
        //StartEnableToPpt(fTime*101);
        DataMgr.m_isEnableDrag = false;
        DataMgr.m_isEnableToPpt = false;
        DataMgr.m_listHideObj.Clear();
    }

    public static void RestoreOriPosDotween(GameObject obj, float time = 1.0f)
    {
        for (int i = 0; i < DataMgr.m_listHideObj.Count; i++)
        {
            DataMgr.m_listHideObj[i].SetActive(true);
        }
        obj.transform.DOScale(DataMgr.m_oriScale, time);
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            trans.DOMove(DataMgr.m_dicOriPos[obj], time);
            trans.DORotate(DataMgr.m_dicOriEuler[obj], time);
        }
    }

    //所有的子物体返回位置
    public static void RestoreAllChildOriLocalPos(GameObject obj)
    {
        Instance.StopCoroutine("YieldAllChildRestoreOriLocalPos");
        Instance.StartCoroutine("YieldAllChildRestoreOriLocalPos", obj);
    }


    public static void RestoreOriLocalPos(GameObject obj)
    {
        Instance.StopCoroutine("YieldRestoreOriLocalPos");
        Instance.StartCoroutine("YieldRestoreOriLocalPos", obj);
        //DataMgr.m_isMoveOri = true;
        //if (DataMgr.m_coMoveOri!= null)
        //{
        //    Instance.StopCoroutine(DataMgr.m_coMoveOri);
        //}
        //DataMgr.m_coMoveOri = Instance.StartCoroutine(Instance.YieldRestoreOriLocalPos(obj));
    }

    public static void RestoreOriPosImmediate(GameObject obj)
    {
        obj.transform.position = DataMgr.m_dicOriPos[obj];
        obj.transform.rotation = DataMgr.m_dicOriQua[obj];
        //obj.transform.localScale = DataMgr.m_dicOriLocalScale
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            trans.position = DataMgr.m_dicOriPos[trans.gameObject];
            trans.rotation = DataMgr.m_dicOriQua[trans.gameObject];
        }
    }

    public static void RestoreAllChildOriPosImmediate(GameObject obj)
    {
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            trans.position = DataMgr.m_dicOriPos[trans.gameObject];
            trans.rotation = DataMgr.m_dicOriQua[trans.gameObject];
        }
    }


    public static void RestoreAllChildOriLocalPosImmediate(GameObject obj)
    {
        obj.transform.position = DataMgr.m_dicOriPos[obj];
        obj.transform.rotation = DataMgr.m_dicOriQua[obj];
        obj.transform.localScale = DataMgr.m_oriScale;
        foreach (var it in DataMgr.m_dicOriLocalPos)
        {
            it.Key.transform.localPosition = it.Value;
            if (DataMgr.m_dicOriLocalQua.ContainsKey(it.Key))
            {
                it.Key.transform.localRotation = DataMgr.m_dicOriLocalQua[it.Key];
            }
        }
        //foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        //{
        //    trans.localPosition = DataMgr.m_dicOriLocalPos[trans.gameObject];
        //    trans.localRotation = DataMgr.m_dicOriLocalQua[trans.gameObject];
        //}
    }

    IEnumerator YieldRestoreOriPos(GameObject obj)
    {
        int i = 0;
        float t = Time.time;
        while (i <= 100)
        {
            //obj.transform.position = Vector3.Lerp (obj.transform.position, DataMgr.m_dicOriPos [obj], (float)i / 100.0f);
            //obj.transform.rotation = Quaternion.Lerp (obj.transform.rotation, DataMgr.m_dicOriQua [obj], (float)i / 100.0f);
            //obj.transform.localScale = Vector3.Lerp (obj.transform.localScale, DataMgr.m_oriScale, (float)i / 100.0f);
            //foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
            //{
            //    if (DataMgr.m_dicOriLocalPos.ContainsKey(trans.gameObject) && trans.localPosition != DataMgr.m_dicOriLocalPos[trans.gameObject])
            //    {
            //        trans.localPosition = Vector3.Lerp(trans.localPosition, DataMgr.m_dicOriLocalPos[trans.gameObject], (float)i / 100.0f);
            //    }
            //    if (DataMgr.m_dicOriLocalQua.ContainsKey(trans.gameObject) && trans.localRotation != DataMgr.m_dicOriLocalQua[trans.gameObject])
            //    {
            //        trans.localRotation = Quaternion.Lerp(trans.localRotation, DataMgr.m_dicOriLocalQua[trans.gameObject], (float)i / 100.0f);
            //    }
            //}
            foreach (var it in DataMgr.m_dicOriLocalPos)
            {
                it.Key.transform.localPosition = Vector3.Lerp(it.Key.transform.localPosition, it.Value, (float)i / 100.0f);
                it.Key.transform.localRotation = Quaternion.Lerp(it.Key.transform.localRotation, DataMgr.m_dicOriLocalQua[it.Key], (float)i / 100.0f);
                it.Key.transform.localScale = Vector3.Lerp(it.Key.transform.localScale, DataMgr.m_dicOriLocalScale[it.Key], (float)i / 100.0f);
            }
            i += 2;
            //yield return new WaitForSeconds (fTime);
            yield return null;
        }
        float timeEnd = Time.time;
        float timeTotal = timeEnd - t;
        DataMgr.m_isEnableDrag = true;
        DataMgr.m_isEnableToPpt = true;
    }

    IEnumerator YieldAllChildRestoreOriLocalPos(GameObject obj)
    {
        int i = 0;
        while (i <= 100/* && DataMgr.m_isMoveOri == true*/)
        {
            foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
            {
                trans.position = Vector3.Lerp(trans.position, DataMgr.m_dicOriPos[trans.gameObject], (float)i / 100.0f);
                trans.rotation = Quaternion.Lerp(trans.rotation, DataMgr.m_dicOriQua[trans.gameObject], (float)i / 100.0f);
            }
            i += 2;
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator YieldRestoreOriLocalPos(GameObject obj)
    {
        int i = 0;
        while (i <= 100 && obj != null)
        {
            if (DataMgr.m_dicOriLocalPos.ContainsKey(obj))
                obj.transform.localPosition = Vector3.Lerp(obj.transform.localPosition, DataMgr.m_dicOriLocalPos[obj], (float)i / 100.0f);
            if (DataMgr.m_dicOriLocalQua.ContainsKey(obj))
                obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, DataMgr.m_dicOriLocalQua[obj], (float)i / 100.0f);
            if (DataMgr.m_dicOriLocalScale.ContainsKey(obj))
                obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, DataMgr.m_dicOriLocalScale[obj], (float)i / 100.0f);
            i +=2;
            yield return new WaitForSeconds(0.001f);
        }
    }

    static public void StopYieldRestoreOriLocalPos()
    {
        Instance.StopCoroutine("YieldRestoreOriLocalPos");
    }

    public static void CreateClone(GameObject obj, bool isHaveOri = true)
    {
        DeleteClone();
        GameObject clone = Instantiate(obj);
        clone.transform.parent = obj.transform.parent;
        if (isHaveOri == true)
        {
            clone.transform.localScale = DataMgr.m_dicOriLocalScale[obj.gameObject];
            clone.transform.localPosition = DataMgr.m_dicOriLocalPos[obj.gameObject];
            clone.transform.localRotation = DataMgr.m_dicOriLocalQua[obj.gameObject];
        }
        else
        {
            clone.transform.localScale = obj.transform.localScale;
            clone.transform.localPosition = obj.transform.localPosition;
            clone.transform.localRotation = obj.transform.localRotation;
        }
        if (clone.GetComponent<MeshCollider>() != null)
        {
            Destroy(clone.GetComponent<MeshCollider>());
        }
        clone.transform.tag = DataMgr.m_tagUnuse;

        List<GameObject> lineTip = new List<GameObject>();//获取标签页
        foreach(var it in clone.transform.GetComponentsInChildren<Transform>())
        {
            if (it.transform.name == "LineTip")
            {
                lineTip.Add(it.gameObject);
            }

            if (it.GetComponent<Canvas>() != null)
            {
                lineTip.Add(it.gameObject);
            }
        }

        for (int i = 0; i < lineTip.Count; i++)
        {
            //lineTip.RemoveAt(i);
            Destroy(lineTip[i]);
        }

        foreach (var trans in clone.transform.GetComponentsInChildren<Renderer>())
        {
            Material mat = new Material(Shader.Find("Custom/TwoSideAlpha"));
            Material[] newBufMat = new Material[trans.transform.GetComponent<Renderer>().materials.Length];
            for (int i = 0; i < trans.transform.GetComponent<Renderer>().materials.Length; i++)
            {
                newBufMat[i] = mat;
            }
            //for (int i = 0; i < trans.transform.GetComponent<Renderer>().materials.Length; i++)
            //{
            //    //trans.transform.GetComponent<Renderer>().materials[i].shader = Shader.Find("Custom/TwoSideAlpha");
            //    trans.transform.GetComponent<Renderer>().materials[i] = PenMgr.mInstance.m_alphaMat;
            //}

            trans.transform.GetComponent<Renderer>().materials = newBufMat;
        }

        //if (clone.GetComponent<SkinnedMeshRenderer>() != null)
        //{
        //    for (int i = 0; i < clone.GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
        //    {
        //        clone.GetComponent<SkinnedMeshRenderer>().materials[i].shader = Shader.Find("Custom/TwoSideAlpha");
        //    }
        //}
        //else
        //{
        //    foreach (var trans in clone.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
        //    {
        //        for (int i = 0; i < trans.transform.GetComponent<SkinnedMeshRenderer>().materials.Length; i++)
        //        {
        //            trans.transform.GetComponent<SkinnedMeshRenderer>().materials[i].shader = Shader.Find("Custom/TwoSideAlpha");
        //        }
        //    }
        //}
        DataMgr.m_curAdsorbObj = clone;
    }

    static public void TransChangeAlphaShader(GameObject obj)
    {
        Material mat = new Material(Shader.Find("Custom/TwoSideAlpha"));
        mat.SetColor("_ColorWithAlpha_Front", new Color(1,1,1,0.04f));
        mat.SetColor("_ColorWithAlpha_Back", new Color(1, 1, 1, 0.04f));
        foreach (var trans in obj.transform.GetComponentsInChildren<Renderer>())
        {
            Material[] newBufMat = new Material[trans.transform.GetComponent<Renderer>().materials.Length];
            for (int i = 0; i < trans.transform.GetComponent<Renderer>().materials.Length; i++)
            {
                newBufMat[i] = mat;
            }
            trans.transform.GetComponent<Renderer>().materials = newBufMat;
        }
    }

    static public void TransOneChangeAlphaShader(GameObject obj)
    {
        Material mat = new Material(Shader.Find("Custom/TwoSideAlpha"));
        mat.SetColor("_ColorWithAlpha_Front", new Color(1, 1, 1, 0.04f));
        mat.SetColor("_ColorWithAlpha_Back", new Color(1, 1, 1, 0.04f));
        
        Material[] newBufMat = new Material[obj.transform.GetComponent<Renderer>().materials.Length];
        for (int i = 0; i < obj.transform.GetComponent<Renderer>().materials.Length; i++)
        {
            newBufMat[i] = mat;
        }

        obj.transform.GetComponent<Renderer>().materials = newBufMat;
    }

    public static void DeleteClone()
    {
        if (DataMgr.m_curAdsorbObj != null)
        {
            Destroy(DataMgr.m_curAdsorbObj);
            DataMgr.m_curAdsorbObj = null;
        }
    }

    public static void CloneSetFalse()
    {
        if (DataMgr.m_curAdsorbObj != null)
        {
            DataMgr.m_curAdsorbObj.SetActive(false);
        }
    }


    static public bool IsAdsorb(GameObject objCur, GameObject objClone)
    {
        bool isAdsorb = false;

        {
            float dis = Vector3.Distance(objCur.transform.position, objClone.transform.position);
            float angle = GetAngle(objCur.transform.forward, objClone.transform.forward);
            //if (dis <= 0.02f && angle <= 30.0f)
            if (dis <= 0.03f)
            {
                isAdsorb = true;
            }
        }
        return isAdsorb;
    }

    static public bool IsAdsorb(Transform objCur, Transform objClone,float minDis)
    {
        bool isAdsorb = false;
        if (objCur != null && objClone != null)
        {
            float dis = Vector3.Distance(objCur.position, objClone.position);
            float angle = GetAngle(objCur.forward, objClone.forward);
            //if (dis <= 0.02f && angle <= 30.0f)
            if (dis <= minDis)
            {
                isAdsorb = true;
            }
        }
        return isAdsorb;
    }

    public static float GetAngle(Vector3 from_, Vector3 to_)
    {
        Vector3 v3 = Vector3.Cross(from_, to_);
        return Vector3.Angle(from_, to_);
    }

    static public void AdsorbDeal()
    {
        if (DataMgr.m_isPenMidPress == false && DataMgr.m_curSelectObj != null && DataMgr.m_curAdsorbObj != null)
        {
            bool isAdsorb = IsAdsorb(DataMgr.m_curSelectObj, DataMgr.m_curAdsorbObj);
            if (isAdsorb == true)
            {
                //DataMgr.m_curSelectObj.transform.localPosition = DataMgr.m_dicOriLocalPos[DataMgr.m_curSelectObj];
                //DataMgr.m_curSelectObj.transform.localRotation = DataMgr.m_dicOriLocalQua[DataMgr.m_curSelectObj];
                RestoreOriLocalPos(DataMgr.m_curSelectObj);
                DeleteClone();
            }
        }
    }

    //中键松开手后还原到原来的位置
    static public void MidUpReturn(GameObject obj)
    {
        obj.transform.DOMove(DataMgr.m_curAdsorbObj.transform.position, 0.5f);
        obj.transform.DORotate(DataMgr.m_curAdsorbObj.transform.eulerAngles, 0.5f);
    }

    static public void MovePos()
    {
        bool isAdsorb = IsAdsorb(DataMgr.m_moveObj, DataMgr.m_moveToObj);
        if (isAdsorb == true)
        {
            Instance.StartCoroutine(Instance.YieldMoveToPos(0.01f, DataMgr.m_moveObj, DataMgr.m_moveToObj));
        }
    }

    static public void MovePosClose()
    {
        bool isAdsorb = IsAdsorb(DataMgr.m_moveObj, DataMgr.m_moveToObj);
        if (isAdsorb == true)
        {
            Instance.StartCoroutine(Instance.YieldMoveToPos(0.01f, DataMgr.m_moveObj, DataMgr.m_moveToObj, isclose: true));
        }
    }

    IEnumerator YieldMoveToPos(float fTime, GameObject obj, GameObject abObj, bool isclose = false)
    {
        int i = 0;
        obj.transform.parent = abObj.transform.parent;
        while (i <= 100 && obj != null)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, abObj.transform.position, (float)i / 100.0f);
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, abObj.transform.rotation, (float)i / 100.0f);
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, abObj.transform.localScale, (float)i / 100.0f);
            i++;
            if (i == 50)
            {
                //abObj.SetActive(false);
                //吸附完成了,发出通知
                if (isclose)
                {
                    obj.GetComponent<BoxCollider>().enabled = false;
                }
                NotificationCenter.Get().ObjDispatchEvent(KEventKey.m_evAbEnd);
            }
            yield return new WaitForSeconds(fTime);
        }
    }

    static public void AllChildSetActive(GameObject obj)
    {
        obj.SetActive(true);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            AllChildSetActive(obj.transform.GetChild(i).gameObject);
        }
    }

    public static void RemoveFromChild(Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            DestroyImmediate(trans.GetChild(i).gameObject);
        }
    }

    public static void ShaderChangeTrans(GameObject obj)
    {
        foreach (var mesh in obj.transform.GetComponentsInChildren<MeshRenderer>())
        {
            for (int i = 0; i < mesh.materials.Length; i++)
            {
                mesh.materials[i].shader = Shader.Find("Custom/TwoSideAlpha");
            }
        }
    }

    public static void RemoveBoxCollider(GameObject obj)
    {
        foreach (var trans in obj.transform.GetComponentsInChildren<Transform>())
        {
            if (trans.GetComponent<BoxCollider>() != null)
            {
                Destroy(trans.GetComponent<BoxCollider>());
            }
        }
    }

    public static void DelayChageTransShader(float fTime, GameObject obj, float r, float g, float b, float a)
    {
        Instance.StartCoroutine(Instance.YieldChageTransShader(fTime, obj, r, g, b, a));
    }

    IEnumerator YieldChageTransShader(float fTime, GameObject obj, float r, float g, float b, float a)
    {
        yield return new WaitForSeconds(fTime);
        ShaderChangeTransColor(obj, r, g, b, a);
    }

    //shader为标准模式
    public static void ShaderReturn(GameObject obj)
    {
        if (obj != null)
        {
            foreach (var mesh in obj.transform.GetComponentsInChildren<MeshRenderer>())
            {
                if (mesh.gameObject.tag == "JianXiuBall")
                {
                    continue;
                }
                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].shader = Shader.Find("Standard");
                }
            }
        }
    }

    public static void ShaderChangeTransColor(GameObject obj, float r, float g, float b, float a)
    {
        if (obj != null)
        {
            foreach (var mesh in obj.transform.GetComponentsInChildren<MeshRenderer>())
            {
                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].shader = Shader.Find("Custom/TwoSideAlpha");
                    mesh.materials[i].SetVector("_ColorWithAlpha_Front", new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f));
                    mesh.materials[i].SetVector("_ColorWithAlpha_Back", new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f));
                }
            }
        }
    }

    public static void RightUiUpdate(int leftIdx, int rightIdx)
    {
        //GameObject par = GameObject.Find("TmpUi");
        //GameObject m_rightPar = par.transform.Find("RightPar").gameObject;
        //GameObject btnLast = m_rightPar.transform.Find("BtnLast").gameObject;
        //GameObject btnNext = m_rightPar.transform.Find("BtnNext").gameObject;
        //btnNext.SetActive(true);
        //btnLast.SetActive(true);
        //string sceneName = SceneManager.GetActiveScene().name;
        //string sChapter = sceneName + leftIdx;
        //Text textTitle = m_rightPar.transform.Find("TextTitle").GetComponent<Text>();
        //textTitle.text = dataManage.Instance(sChapter).get(rightIdx, "Title");

        //string sType = dataManage.Instance(sChapter).get(rightIdx, "Type");
        //GameObject objText = m_rightPar.transform.Find("TextContent").gameObject;
        //GameObject objSpr = m_rightPar.transform.Find("ImgPar").gameObject;

        //Text textIdx = m_rightPar.transform.Find("TextIdx").GetComponent<Text>();
        //textIdx.text = (rightIdx + 1).ToString() + "/" + dataManage.Instance(sChapter).num().ToString();
        //if (sType == "0")
        //{
        //    objText.SetActive(true);
        //    objSpr.SetActive(false);
        //    objText.GetComponent<Text>().text = dataManage.Instance(sChapter).get(rightIdx, "Text");
        //    TypeWriter writer = objText.GetComponent<TypeWriter>();
        //    writer.SetContent();
        //}
        //else if (sType == "1")
        //{
        //    objText.SetActive(false);
        //    objSpr.SetActive(true);
        //    objSpr.transform.Find("Image");

        //    Image sprView = objSpr.transform.Find("Image").GetComponent<Image>();
        //    Sprite tem = new Sprite();
        //    sprView.sprite = Resources.Load(dataManage.Instance(sChapter).get(rightIdx, "Sprite"), tem.GetType()) as Sprite;

        //    objSpr.transform.Find("Text").GetComponent<Text>().text = dataManage.Instance(sChapter).get(rightIdx, "Text");
        //    TypeWriter writer = objSpr.transform.Find("Text").GetComponent<TypeWriter>();
        //    writer.SetContent();
        //}
        //int max = dataManage.Instance(sChapter).num();
        //if (max == 1)
        //{
        //    btnLast.SetActive(false);
        //    btnNext.SetActive(false);
        //}

        //if (rightIdx == 0)
        //{
        //    btnLast.SetActive(false);
        //}

        //if (rightIdx == max - 1)
        //{
        //    btnNext.SetActive(false);
        //}
    }

    public static void GoToScene(string name)
    {
        PlayerPrefs.SetString(DataMgr.m_gotoScene, name);
        PlayerPrefs.SetString(DataMgr.m_reScene, SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LoadScene");
    }


    public static void GoToSceneDirect(string name)
    {
        PlayerPrefs.SetString(DataMgr.m_gotoScene, name);
        PlayerPrefs.SetString(DataMgr.m_reScene, SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(name);
    }

    public static void GotoReScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString(DataMgr.m_reScene));
    }

    //从资源文件夹中加载
    public static Sprite GetSprite(string name)
    {
        return Resources.Load<Sprite>(name);
    }

    public static string GetCurSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    static public void ExplodeObj(GameObject obj)
    {
        float j = 0;
        Instance.StopCoroutine("YieldRestoreOriPos");
        StartEnableDrag(1);
        StartEnableToPpt(1);
        foreach (var it in DataMgr.m_dicOriLocalPos)
        {
            float x = DataMgr.m_radius * Mathf.Cos(j);
            float y = DataMgr.m_radius * Mathf.Sin(j);
            //trans.DOMove(new Vector3(x + m_dicTransLocalInfo[trans.gameObject].m_pos.x, y + m_dicTransLocalInfo[trans.gameObject].m_pos.y, trans.localPosition.z), 2);
            it.Key.transform.DOMove(new Vector3(x + DataMgr.m_dicOriLocalPos[it.Key.transform.gameObject].x, y + DataMgr.m_dicOriLocalPos[it.Key.transform.gameObject].y, it.Key.transform.localPosition.z), 1);
            j += DataMgr.m_changeRate;
        }
        //foreach (Transform trans in obj.transform.GetComponentsInChildren<Transform>())
        //{
        //    float x = DataMgr.m_radius * Mathf.Cos(j);
        //    float y = DataMgr.m_radius * Mathf.Sin(j);
        //    //trans.DOMove(new Vector3(x + m_dicTransLocalInfo[trans.gameObject].m_pos.x, y + m_dicTransLocalInfo[trans.gameObject].m_pos.y, trans.localPosition.z), 2);
        //    trans.DOMove(new Vector3(x + DataMgr.m_dicOriLocalPos[trans.gameObject].x, y + DataMgr.m_dicOriLocalPos[trans.gameObject].y, trans.localPosition.z), 1);
        //    j += DataMgr.m_changeRate;
        //}
    }

    static public void FlashObj(GameObject obj, float time, float delayClose)
    {
        if (obj.GetComponent<HighlightableObject>() == null)
        {
            obj.AddComponent<HighlightableObject>();
        }
        obj.GetComponent<HighlightableObject>().FlashingOn(Color.red, Color.yellow, time);
        Instance.StartCoroutine(Instance.YieldFlashClose(delayClose, obj));
    }

    static public void FlashObjAllTime(GameObject obj, float freq)
    {
        if (obj.GetComponent<HighlightableObject>() == null)
        {
            obj.AddComponent<HighlightableObject>();
        }
        obj.GetComponent<HighlightableObject>().FlashingOn(Color.red, Color.yellow, freq);
    }

    static public void FlashOff(GameObject obj)
    {
        if (obj.GetComponent<HighlightableObject>() != null)
        {
            obj.GetComponent<HighlightableObject>().FlashingOff();
        }
    }
    IEnumerator YieldFlashClose(float delayClose, GameObject obj)
    {
        yield return new WaitForSeconds(delayClose);
        obj.GetComponent<HighlightableObject>().FlashingOff();
    }


    static public void MoveAToB(GameObject a, GameObject b, float time = 0.01f)
    {
        Instance.StartCoroutine(Instance.YieldMoveToPos(time, a, b));
    }


    static public void MoveOnlyPosAToB(GameObject a, GameObject b, float time = 0.01f)
    {
        Instance.StartCoroutine(Instance.YieldOnlyPosMoveToPos(time, a, b));
    }

    IEnumerator YieldOnlyPosMoveToPos(float fTime, GameObject obj, GameObject abObj, bool isclose = false)
    {
        int i = 0;
        while (i <= 100)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, abObj.transform.position, (float)i / 100.0f);
            i++;
            yield return new WaitForSeconds(fTime);
        }
    }

    static public void ForbidDrag(GameObject obj)
    {
        ChangeTag(obj, DataMgr.m_tagUnuse);
        RemoveMeshCollider(obj);
    }

    public static void SaveLabel(GameObject obj)
    {
        DataMgr.m_listLabelCanvas.Clear();
        foreach (Canvas trans in obj.transform.GetComponentsInChildren<Canvas>())
        {
            DataMgr.m_listLabelCanvas.Add(trans);
        }
    }
    public static void LabelSet(bool isActive)
    {
        for (int i = 0; i < DataMgr.m_listLabelCanvas.Count; i++)
        {
            DataMgr.m_listLabelCanvas[i].gameObject.SetActive(isActive);
        }
    }
 
    static public void StartEnableToPpt(float delay)
    {
        DataMgr.m_isEnableToPpt = false;
        Instance.StopCoroutine("YieldStartEnableToPpt");
        Instance.StartCoroutine("YieldStartEnableToPpt", delay);
    }

    static public void StopCor()
    {
        Instance.StopAllCoroutines();
    }
    IEnumerator YieldStartEnableToPpt(float delay)
    {
        yield return new WaitForSeconds(delay);
        DataMgr.m_isEnableToPpt = true;
    }

    static public void StartEnableDrag(float delay)
    {
        DataMgr.m_isEnableDrag = false;
        Instance.StopCoroutine("YieldStartEnableDrag");
        Instance.StartCoroutine("YieldStartEnableDrag", delay);
    }

    IEnumerator YieldStartEnableDrag(float delay)
    {
        yield return new WaitForSeconds(delay);
        DataMgr.m_isEnableDrag = true;
    }

    static public void StartEnableRightBigMenu(float delay)
    {
        DataMgr.m_isEnableRightBigMenu = false;
        Instance.StopCoroutine("YieldStartEnableRightBigMenu");
        Instance.StartCoroutine("YieldStartEnableRightBigMenu", delay);
    }

    IEnumerator YieldStartEnableRightBigMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        DataMgr.m_isEnableRightBigMenu = true;
    }

    static public void ShaderFlash(float freq, float times, GameObject obj, float r, float g, float b, float a)
    {
        Instance.StartCoroutine(Instance.YieldShaderFlash(freq, times, obj, r, g, b, a));
    }

    static public void ShaderFlashZaku(float freq, float times, GameObject obj, float r, float g, float b, float a)
    {
        Instance.StartCoroutine(Instance.YieldShaderFlashZaku(freq, times, obj, r, g, b, a));
    }

    IEnumerator YieldShaderFlashZaku(float freq, float times, GameObject obj, float r, float g, float b, float a)
    {
        int i = 0;
        float curTime = 0.0f;
        while (curTime <= times)
        {
            if (i % 2 == 0)
            {
                ShaderChangeTransColorZaku(obj, r, g, b, a);
            }
            else
            {
                ShaderReturn(obj);
            }
            i++;
            curTime += freq;
            yield return new WaitForSeconds(freq);
        }
        ShaderReturn(obj);
    }

    public static void ShaderChangeTransColorZaku(GameObject obj, float r, float g, float b, float a)
    {
        if (obj != null)
        {
            foreach (var mesh in obj.transform.GetComponentsInChildren<MeshRenderer>())
            {
                if (mesh.gameObject.tag == "JianXiuBall")
                {
                    continue;
                }
                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    Texture main = mesh.materials[i].mainTexture;
                    Texture nomal = mesh.materials[i].GetTexture("_BumpMap");
                    Texture metal = mesh.materials[i].GetTexture("_MetallicGlossMap");
                    mesh.materials[i].shader = Shader.Find("Custom/Flash_S");
                    mesh.materials[i].SetTexture("_Albedo", main);
                    mesh.materials[i].SetTexture("_Metallic", metal);
                    mesh.materials[i].SetTexture("_Normal", nomal);
                    //mesh.materials[i].mainTexture = main;fdfddsfds
                    //mesh.materials[i].
                    //mesh.materials[i].SetVector("_ColorWithAlpha_Front", new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f));
                    //mesh.materials[i].SetVector("_ColorWithAlpha_Back", new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f));
                }
            }
        }
    }

    IEnumerator YieldShaderFlash(float freq, float times, GameObject obj, float r, float g, float b, float a)
    {
        int i = 0;
        while (i <= times)
        {
            if (i % 2 == 0)
            {
                ShaderChangeTransColor(obj, r, g, b, a);
            }
            else
            {
                ShaderReturn(obj);
            }
            i++;
            yield return new WaitForSeconds(freq);
        }
        ShaderReturn(obj);
    }

    public static void DoMoveY(GameObject obj, float y, float time)
    {
        foreach (MeshRenderer trans in obj.transform.GetComponentsInChildren<MeshRenderer>())
        {
            trans.transform.DOLocalMoveY(y, time);
        }
    }

    static public void ShaderFlashFrame(int clip, int total, GameObject obj, float r, float g, float b, float a)
    {
        Instance.StartCoroutine(Instance.YieldShaderFlashFrame(clip, total, obj, r, g, b, a));
    }
    IEnumerator YieldShaderFlashFrame(int clip, int total, GameObject obj, float r, float g, float b, float a)
    {
        int i = 0;
        int cnt = 0;
        bool isFalsh = false;
        while (i < total && obj != null)
        {
            cnt++;
            if (cnt >= clip)
            {
                cnt = 0;
                isFalsh = !isFalsh;
            }

            if (isFalsh)
            {
                ShaderChangeTransColor(obj, r, g, b, a);
            }
            else
            {
                ShaderReturn(obj);
            }
            i++;
            yield return null;
        }
        ShaderReturn(obj);
    }

    /// <summary>
    /// 以IO方式进行加载
    /// </summary>
    static public Sprite LoadSprByIO(string sPath, Image img, int width, int height)
    {
        //double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;

        //startTime = (double)Time.time - startTime;
        //Debug.Log("IO加载用时:" + startTime);
        return sprite;
    }

    static public Sprite GetSprByByte(byte[] bufByte,int width,int height)
    {
        //创建Texture
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bufByte);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    static public byte[] GetSprByIoRetByte(string sPath, Image img, int width, int height)
    {
        //double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;

        //startTime = (double)Time.time - startTime;
        //Debug.Log("IO加载用时:" + startTime);
        return bytes;
    }

    static public Sprite GetSprByIO(string sPath, int width, int height)
    {
        //double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //startTime = (double)Time.time - startTime;
        //Debug.Log("IO加载用时:" + startTime);
        return sprite;
    }

    //一个btnpar下的统一按钮样式,
    static public void BtnChangeState(int idx, GameObject btnPar, Sprite sprN, Sprite sprP, Color colN, Color colP)
    {
        for (int i = 0; i < btnPar.transform.childCount; i++)
        {
            if (idx == i)
            {
                btnPar.transform.GetChild(i).GetComponent<Image>().sprite = sprP;
                btnPar.transform.GetChild(i).Find("Text").GetComponent<Text>().color = colP;
            }
            else
            {
                btnPar.transform.GetChild(i).GetComponent<Image>().sprite = sprN;
                btnPar.transform.GetChild(i).Find("Text").GetComponent<Text>().color = colN;
            }
        }
    }

    static public void BtnChangeStateList(int idx, GameObject btnPar, List<Sprite> listSpr)
    {
        for (int i = 0; i < btnPar.transform.childCount; i++)
        {
            if (idx == i)
            {
                btnPar.transform.GetChild(i).GetComponent<Image>().sprite = listSpr[i + btnPar.transform.childCount];
            }
            else
            {
                btnPar.transform.GetChild(i).GetComponent<Image>().sprite = listSpr[i];
            }
        }
    }

    static public GameObject CreateObjFromRes(string sName, Transform par = null)
    {
        GameObject tmp = Resources.Load<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            //obj.SetActive(true);
            if (par != null)
            {
                obj.transform.SetParent(par,false);
            }
            return obj;
        }
        else
        {
            UnityEngine.Debug.Log("CreateFromResFailure:" + sName);
            return null;
        }
    }

    static public GameObject CreateTmp(GameObject tmp)
    {
        GameObject obj = Instantiate(tmp);
        obj.SetActive(true);
        return obj;
    }
    static public GameObject CreateTmp(GameObject tmp ,Transform par,Vector3 pos,Vector3 angles,Vector3 scale)
    {
        GameObject obj = Instantiate(tmp);
        obj.SetActive(true);
        obj.transform.parent = par;
        obj.transform.localPosition = pos;
        obj.transform.localEulerAngles = angles;
        obj.transform.localScale = scale;
        return obj;

    }

    static public GameObject CreateTmp(GameObject tmp, Transform par)
    {
        GameObject obj = Instantiate(tmp);
        obj.SetActive(true);
        obj.transform.SetParent(par,false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        return obj;

    }

    public static bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }

    static public GameObject CreateObjFromRes(string sName,GameObject par,Vector3 pos,Vector3 angles, Vector3 scale)
    {
        GameObject tmp = Resources.Load<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            obj.SetActive(true);
            obj.transform.parent = par.transform;
            obj.transform.localPosition = pos;
            obj.transform.localEulerAngles = angles;
            obj.transform.localScale = scale;
            return obj;
        }
        else
        {
            UnityEngine.Debug.LogError("CreateFromResFailure:" + sName);
            return null;
        }
    }


    static public void CreateObjFromResAsync(string sName,GameObject par,Vector3 pos,Vector3 angle,Vector3 scale, bool isShowLoad = false,bool isAutoDestory = false,string name = "",bool isAtuoHide = false,bool isPreCnt = false)
    {
        //if (m_corCreateFromRes != null)
        //{
        //    Instance.StopCoroutine(m_corCreateFromRes);
        //}
        m_corCreateFromRes = Instance.StartCoroutine(Instance.YieldCreateFromRes(sName,par,pos,angle,scale,isShowLoad, isAutoDestory,name,isAtuoHide,isPreCnt));
    }

    IEnumerator YieldCreateFromRes(string sName, GameObject par, Vector3 pos, Vector3 angle, Vector3 scale,bool isShowLoad,bool isAutoDestory = false,string name = "",bool isAutoHide = false,bool isPreCnt = false)
    {
        //ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>("Characters/Textures/CostumePartyCharacters" + (i < 2 ? "" : "" + i));
        //while (!resourceRequest.isDone)
        //{
        //    yield return 0;
        //}
        //material.mainTexture = resourceRequest.asset as Texture2D;
        GameObject load = null;
        if (isShowLoad == true)
        {
            load = PublicFunc.CreateObjFromRes("Lockie/PreLoadMgr");
        }
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(sName);
        while (!resourceRequest.isDone)
        {
            yield return 0;
        }


        GameObject tmp = resourceRequest.asset as GameObject;
        //GameObject tmp = (GameObject)Resources.LoadAsync<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            obj.SetActive(true);
            if (obj != null && par != null)
            {
                obj.transform.parent = par.transform;
                obj.transform.localPosition = pos;
                obj.transform.localEulerAngles = angle;
                obj.transform.localScale = scale;
            }

            if (name != "")
            {
                obj.name = name;
            }

            if (isAutoHide == true)
            {
                obj.SetActive(false);
            }

            if (isPreCnt == true)
            {
                NotificationCenter.Get().ObjDispatchEvent(KEventKey.m_evPreLoadCnt);
            }
            if (isAutoDestory == true)
            {
                Destroy(obj);
            }
            //var rotation = Quaternion.identity;
            //rotation.eulerAngles = angle;
            //GameObject obj = ObjPoolMgr.SpawnPrefab(tmp, pos, rotation, scale, par.transform, transform);
        }
        else
        {
            UnityEngine.Debug.LogError("CreateFromResFailure:" + sName);
        }

        if (isShowLoad == true)
        {
            if (load != null)
            {
                Destroy(load);
            }
        }
        yield return null;
    }
    static public bool IsAnimtorFinish(Animator anim)
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        // 判断动画是否播放完成
        if (info.normalizedTime >= 1.0f)
        {
            return true;
        }
        return false;
    }

    public static float ClampAngle(float angle)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return angle;
    }

    public static float ClampAngleLimit(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    static public void CopyFileToFolder(string oriPath, string toPath)
    {
        string name = GetFileNameWithFormat(oriPath);

    }

    static public void CopyFileOldName(string oriPath, string toPath)
    {
        string name = GetFileNameWithFormat(oriPath);
        string toAllPath = toPath + "/" + name;

        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        if (File.Exists(toAllPath))
        {
            File.Copy(oriPath, toAllPath, true);
        }
        else
        {
            File.Copy(oriPath, toAllPath);
        }
    }

    static public void CopyFileNewName(string oriPath, string toPath,string name)
    {
        string format = GetFileFormat(oriPath);
        string toAllPath = toPath + "/" + name + "." + format;
        
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        if (File.Exists(toAllPath))
        {
            File.Copy(oriPath, toAllPath, true);
        }
        else
        {
            File.Copy(oriPath, toAllPath);
        }
    }

    static public string GetFileNameWithFormat(string path)
    {
        string[] bufName = path.Split('\\');
        string name = bufName[bufName.Length - 1];
        return name;
    }

    static public string GetFileFormat(string path)
    {
        string[] bufFormat = path.Split('.');
        return bufFormat[bufFormat.Length - 1];
    }

    static public void SavePptDataToOther(int id,string toPath)
    {

        string toPar = toPath + "/" + id.ToString();

        if (!Directory.Exists(toPar))
        {
            Directory.CreateDirectory(toPar);
        }

        string toJsonPath = toPar + "/PptEdit";
        string toImgPath = toPar + "/PptImg";

        if (!Directory.Exists(toJsonPath))
        {
            Directory.CreateDirectory(toJsonPath);
        }

        if (!Directory.Exists(toImgPath))
        {
            Directory.CreateDirectory(toImgPath);
        }

        string imgOriPath = UnityEngine.Application.streamingAssetsPath + "/PptImg/" + id.ToString();
        string jsonOriPathFile = UnityEngine.Application.streamingAssetsPath + "/PptEdit/" + id.ToString() + ".json";
        File.Copy(jsonOriPathFile, toJsonPath + "/" + id.ToString() + ".json", true);

        string[] listImg = Directory.GetFiles(imgOriPath);
        for (int i = 0; i < listImg.Length; i++)
        {
            string[] bufImg = listImg[i].Split('\\');
            string toImgPathFile = toImgPath + "/"  + bufImg[bufImg.Length - 1];
            File.Copy(listImg[i], toImgPathFile, true);
        }

        //寻找子图文件夹
        string[] listChildImgFolder = Directory.GetDirectories(imgOriPath);
        for ( int i = 0; i < listChildImgFolder.Length; i++)
        {
            string[] listChildImg = Directory.GetFiles(listChildImgFolder[i]);

            for ( int j = 0; j < listChildImg.Length; j++)
            {
                string[] bufImg = listChildImg[j].Split('\\');
                string toImgPathFile = toImgPath + "/" + bufImg[bufImg.Length - 2] + "/" + bufImg[bufImg.Length - 1];
                string toImgPathFloder = toImgPath + "/" + bufImg[bufImg.Length - 2];
                if (!Directory.Exists(toImgPathFloder))
                {
                    Directory.CreateDirectory(toImgPathFloder);
                }

                File.Copy(listChildImg[j], toImgPathFile, true);
            }
        }
    }

    static public void ExportPptData(Dictionary<int,int> dic,string path)
    {
        foreach(var it in dic)
        {
            string oriId = it.Key.ToString();
            string toId = it.Value.ToString();
            string oriJosnPathFile = path + "/" + oriId + "/PptEdit/" + oriId + ".json";
            string toJsonPathFile = UnityEngine.Application.streamingAssetsPath + "/PptEdit/" + toId + ".json";
            File.Copy(oriJosnPathFile, toJsonPathFile, true);

            string oriImgPath = path + "/" + oriId + "/PptImg";

            string toImgPath = UnityEngine.Application.streamingAssetsPath + "/PptImg/" + toId;
            if (!Directory.Exists(toImgPath))
            {
                Directory.CreateDirectory(toImgPath);
            }

            string[] listImg = Directory.GetFiles(oriImgPath);
            for (int i = 0; i < listImg.Length; i++)
            {
                string[] bufImg = listImg[i].Split('\\');
                string toImgPathFile = toImgPath + "/" + bufImg[bufImg.Length - 1];
                File.Copy(listImg[i], toImgPathFile, true);
            }

            //寻找子图文件夹
            string[] listChildImgFolder = Directory.GetDirectories(oriImgPath);
            for (int i = 0; i < listChildImgFolder.Length; i++)
            {
                string[] listChildImg = Directory.GetFiles(listChildImgFolder[i]);

                for (int j = 0; j < listChildImg.Length; j++)
                {
                    string[] bufImg = listChildImg[j].Split('\\');
                    string toImgPathFile = UnityEngine.Application.streamingAssetsPath + "/PptImg/" + toId + "/" + bufImg[bufImg.Length - 2] + "/" + bufImg[bufImg.Length - 1];
                    string toImgPathFloder = UnityEngine.Application.streamingAssetsPath + "/PptImg/" + toId + "/" + bufImg[bufImg.Length - 2];
                    if (!Directory.Exists(toImgPathFloder))
                    {
                        Directory.CreateDirectory(toImgPathFloder);
                    }

                    File.Copy(listChildImg[j], toImgPathFile, true);
                }
            }

        }
    }

    public static Process StartProcess(string fileName,string args)
    {
        try
        {
            Process myProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, args);
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo = startInfo;
            return myProcess;
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("出错原因：" + ex.Message);
        }
        return null;
    }

    public static bool IsChild(Transform child, Transform par)
    {
        bool isChild = false;
        foreach (var it in par.GetComponentsInChildren<Transform>())
        {
            if (it.GetInstanceID() == child.GetInstanceID())
            {
                isChild = true;
                break;
            }
        }
        return isChild;
    }

    public static void LightHigh(Transform trans)
    {
        if (trans.transform.gameObject.GetComponent<HighlightableObject>() == null)
        {
            trans.transform.gameObject.AddComponent<HighlightableObject>();
        }
        trans.transform.gameObject.GetComponent<HighlightableObject>().ConstantOn(Color.yellow);
    }


    public static void LightHigh(Transform trans,Color color)
    {
        if (trans.transform.gameObject.GetComponent<HighlightableObject>() == null)
        {
            trans.transform.gameObject.AddComponent<HighlightableObject>();
        }
        trans.transform.gameObject.GetComponent<HighlightableObject>().ConstantOn(color);
    }

    public static void LightHighOff(Transform trans)
    {
        if (trans.transform.gameObject.GetComponent<HighlightableObject>() == null)
        {
            trans.transform.gameObject.AddComponent<HighlightableObject>();
        }
        trans.transform.gameObject.GetComponent<HighlightableObject>().ConstantOff();
    }

    public static void Encypt(ref Byte[] targetData)
    {
        int dataLength = targetData.Length;
        for (int i = 0; i < dataLength; ++i)
        {
            targetData[i] = (byte)(targetData[i] ^ DataMgr.m_abKey);
        }
    }

    public static string GetFileNameByLine(string name)
    {
        string[] file = name.Split('/');
        if (file.Length == 0)
        {
            return name;
        }
        else
        {
            return file[file.Length - 1];
        }
    }

    public static string GetFileWithFormatByPath(string path)
    {
        string[] bufPath = path.Split('/');
        string name = bufPath[bufPath.Length - 1];
        //string abPath = info.m_prefabName.Replace("/" + abName, "");
        //string[] bufAbName = abName.Split('.');
        return name;
    }

    public static string GetFileNoFormatByPath(string path)
    {
        string[] bufPath = path.Split('/');
        string name = bufPath[bufPath.Length - 1];
        string[] bufName = name.Split('/');
        //string abPath = info.m_prefabName.Replace("/" + abName, "");
        //string[] bufAbName = abName.Split('.');
        return bufName[0];
    }

    public static string GetOnlyPath(string path)
    {
        string[] bufPath = path.Split('/');
        string name = bufPath[bufPath.Length - 1];
        string onlyPath = path.Replace(name,"");
        //string abPath = info.m_prefabName.Replace("/" + abName, "");
        //string[] bufAbName = abName.Split('.');
        return onlyPath;
    }

    public static T StringToEnum<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    public static T GetJsonData<T>(string path) where T:new()
    {
        T ret = new T();
        string str = JsonMgr.GetJsonString(GetStreamingAssetsPath() + "/" + path);
        ret = JsonMapper.ToObject<T>(str);
        return ret;

    }

    public static string GetStreamingAssetsPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "jar:file://" + Application.dataPath + "!/assets/";
            case RuntimePlatform.IPhonePlayer:
                return Application.dataPath + "/Raw/";
            default:
                return Application.streamingAssetsPath;
        }
    }

    public static List<int> GetRandoms(int sum, int min, int max)
    {
        List<int> arr = new List<int>();
        int j = 0;
        System.Random rm = new System.Random();
        while (arr.Count < sum)
        {
            //返回一个min到max之间的随机数
            int nValue = rm.Next(min, max);
            // 是否包含特定值
            if (!arr.Contains(nValue))
            {
                //把键和值添加到hashtable
                arr.Add(nValue);
                j++;
            }
        }

        return arr;
    }

    public static string GetFileMD5(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetFileMD5 Failed:" + ex.Message);
        }
    }

    public static bool IsPhoneOK(string num)
    {
        if (num.Length < 11)
        {
            return false;
        }
        //电信手机号码正则
        string dianxin = @"^1[3578][01379]\d{8}$";
        Regex regexDX = new Regex(dianxin);
        //联通手机号码正则
        string liantong = @"^1[34578][01256]\d{8}";
        Regex regexLT = new Regex(liantong);
        //移动手机号码正则
        string yidong = @"^(1[012345678]\d{8}|1[345678][012356789]\d{8})$";

        string phone = @"^1([38][0-9]|4[579]|5[0-3,5-9]|6[6]|7[0135678]|9[89])\d{8}$";
        Regex regexYD = new Regex(yidong);
        Regex phoneRe = new Regex(phone);
        if (regexDX.IsMatch(num) || regexLT.IsMatch(num) || regexYD.IsMatch(num) || phoneRe.IsMatch(num))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsFileExist(string path)
    {
        if (File.Exists(path))
        {
            return true;
        }
        return false; 
    }

    //颜色转换16进制转color
    public static Color StringToColor(string colorStr)
    {
        if (string.IsNullOrEmpty(colorStr))
        {
            return new Color();
        }
        int colorInt = int.Parse(colorStr, System.Globalization.NumberStyles.AllowHexSpecifier);
        return IntToColor(colorInt);
    }

    public static Color IntToColor(int colorInt)
    {
        float basenum = 255;

        int b = 0xFF & colorInt;
        int g = 0xFF00 & colorInt;
        g >>= 8;
        int r = 0xFF0000 & colorInt;
        r >>= 16;
        return new Color((float)r / basenum, (float)g / basenum, (float)b / basenum, 1);

    }


    public static bool IsDigitOrNumber(string str)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(str, @"(?i)^[0-9a-z]+$"))
            return true;
        else return false;
    }

    public static bool IsTiXianOk(string str)
    {
        
        Regex reg = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,10}$");
        return reg.IsMatch(str);
    }
    public static string GetAbNameFromPrefab(string assetPath)
    {
        string name = "";

        string[] buf = assetPath.Split('/');
        name = buf[buf.Length - 1];
        string[] bufName = name.Split('.');
        if (bufName.Length == 2)
        {
            name = name.Replace(bufName[1], "");
            name += "assetbundle";
        }
        else {
            name += ".assetbundle";
        }
        
        return name;
       
    }

    public static string GetUserHeadImg(Account account)
    {
        long modelId = (int)account.modleId;
        
        
        string headImgName = DataMgr.m_dicRoleProperties[modelId].Icon;
        
        return headImgName;
    }


    public static string GetUserHeadImg(long modelId)
    {
        if (DataMgr.m_dicRoleProperties.ContainsKey(modelId) == false)
        {
            UnityEngine.Debug.Log("字典中不包含头像");
        }
        string headImgName = DataMgr.m_dicRoleProperties[modelId].Icon;

        return headImgName;
    }

    public static string GetUserModelName(Account account)
    {
        long modelId = (int)account.modleId;
        string modelName = DataMgr.m_dicRoleProperties[modelId].ModelDate;
        return modelName;
    }


    public static string GetUsertModelName(long modelId)
    {
        string modelName = DataMgr.m_dicRoleProperties[modelId].ModelDate;
        return modelName;
    }

    public static void ToGameServer<T>(string key,T content)
    {
        switch (DataMgr.m_curServer)
        {
            case EnServer.MyServer:
                GameSocket.Instance.SendMsgProto(key, content);
                break;
            case EnServer.OtherServer:
                OtherSocket.Instance.SendMsgProto(key, content);
                break;
            default:
                break;
        }
    }

    public static string GetTimeBySec(int sec)
    {
        string ret = "";

        int hour = Convert.ToInt16((sec % 86400) / 3600);
        int minute = Convert.ToInt16((sec % 86400 % 3600) / 60);
        int second = Convert.ToInt16(sec % 86400 % 3600 % 60);

        if (hour != 0)
        {
            ret += hour.ToString() + "h";
        }

        if (minute != 0)
        {
            ret += minute.ToString() + "m";
        }

        if (second != 0)
        {
            ret += second.ToString() + "s";
        }
  
        return ret;
    }
    public static string GetAmountInt(int num)
    {
        string ret = "";
        if (num > 1000000)
        {
            num = num / 10000;
            ret = num + "万";
        }
        else
        {
            ret = num.ToString();
        }
        return ret;

    }

    public static string GetAmountDoub(double num)
    {
        string ret = "";
        if (num > 1000000)
        {
            num = num / 10000;
            ret = num + "万";
        }
        else
        {
            ret = num.ToString("0.00");
        }
        return ret;
    }

    public static string GetLegalString(string before)
    {

        string[] bufRemove = new string[]{ "\'","\"","\\", "\b", "\f", "\n", "\r", "\t", "-","_",",","!","|","~","`","#","$","%","^","&","*",":",";","?"};
        for (int i = 0; i < bufRemove.Length; i++)
        {
            before = before.Replace(bufRemove[i], "");
        }
        return before;

    }

    public static List<T> GetListDiff<T>(List<T> listA,List<T> listB)
    {
        List<T> ret = new List<T>();
        if (listA.Count > listB.Count)
            ret = listA.Except(listB).ToList();
        else {
            ret = listB.Except(listA).ToList();
        }
        return ret;
    }

    public static  Transform GetTransform(Transform check, string name)
    {

        foreach (Transform t in check.GetComponentsInChildren<Transform>())
        {
            if (t.name == name)
            {
                return t;
            }
        }
        return null;
    }

    public static bool IsMy(long id)
    {
        if (DataMgr.m_account.id == id)
        {
            return true;
        }
        return false;
    }

    public static void CreateHeadImg(Image head, long modelId)
    {
        string modelIcon = DataMgr.m_dicRoleProperties[modelId].Icon;
        //head.color=new Vector4 (1,1,1,1);
        AssetMgr.Instance.CreateSpr(modelIcon, "charactericon", (param) => { head.sprite = param; });
    }

    public static void DeleteFile(string path)
    {
        bool isExist = PublicFunc.IsFileExist(path);
        if (isExist == true)
        {
            File.Delete(path);
        }
    }

    public static void AddNavTag(GameObject obj)
    {
        foreach (var item in obj.transform.GetComponentsInChildren<Renderer>())
        {
            if (item.gameObject.layer != LayerMask.NameToLayer("NotWalk"))
            { if (item.GetComponent<NavMeshSourceTag>() == null)
                {
                    item.gameObject.AddComponent<NavMeshSourceTag>();
                }
            }
        }
    }

    public static IEnumerator YieldDynamicText(Text text, double ori, double to,float clipTotal = 0.8f)
    {
        int clip = 20;
        double i = (to - ori) / (double)clip; //每一份的差

        if (ori > to)
        {
            while (ori > to)
            {
                ori += i;
                if (ori < to)
                {
                    ori = to;
                    text.text = GetAmountDoub(ori);
                    break;
                }
                else
                {
                    text.text = GetAmountDoub(ori);
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
        else if (ori < to)
        {
            while (ori < to)
            {
                ori += i;
                if (ori > to)
                {
                    ori = to;
                    text.text = GetAmountDoub(ori);
                    break;
                }
                else
                {
                    text.text = GetAmountDoub(ori);
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
    }

    //ugui坐标转世界坐标
    public static Vector3 posUGUI2World(Camera camera,Vector3 uguipos)
    {
        Vector3 scr = RectTransformUtility.WorldToScreenPoint(camera, uguipos);
        scr.z = 0;
        scr.z = Mathf.Abs(camera.transform.position.z - uguipos.z);
        Vector3 ret = camera.ScreenToWorldPoint(uguipos);
        return ret;
    }


    public static Vector3 PosOverlay2World(Camera cam, Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos); // 目的获取z，在Start方法
        pos.z = screenPos.z; // 这个很关键
        Vector3 worldPos = cam.ScreenToWorldPoint(pos);
        return worldPos;
    }

    public static Vector3 PosWorld2Overlay(Camera cam, Vector3 pos)
    {
        Vector2 player2DPosition = cam.WorldToScreenPoint(pos);
        return player2DPosition;
    }

    public static IEnumerator YieldDynamicText(Text text, int ori, int to, float clipTotal = 0.8f)
    {
        int clip = 20;
        int i = (to - ori) / clip; //每一份的差

        if (ori > to)
        {
            while (ori > to)
            {
                ori += i;
                if (ori < to)
                {
                    ori = to;
                    text.text = GetAmountInt(ori);
                    break;
                }
                else {
                    text.text = GetAmountInt(ori);
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
        else if (ori < to)
        {
            while (ori < to)
            {
                ori += i;
                if (ori > to)
                {
                    ori = to;
                    text.text = GetAmountInt(ori);
                    break;
                }
                else
                {
                    text.text = GetAmountInt(ori);
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
    }
    public static IEnumerator YieldDynamicTextNoM(Text text, int ori, int to, float clipTotal = 0.8f)
    {
        int clip = 20;
        int i = (to - ori) / clip; //每一份的差

        if (ori > to)
        {
            while (ori > to)
            {
                ori += i;
                if (ori < to)
                {
                    ori = to;
                    text.text = ori.ToString("##,###");
                    break;
                }
                else
                {
                    text.text = ori.ToString("##,###");
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
        else if (ori < to)
        {
            while (ori < to)
            {
                ori += i;
                if (ori > to)
                {
                    ori = to;
                    text.text = ori.ToString("##,###");
                    break;
                }
                else
                {
                    text.text = ori.ToString("##,###");
                }
                yield return new WaitForSeconds(clipTotal / (float)clip);
            }

        }
    }
    //世界坐标转画布坐标
    Vector2 World2CanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out pos);
        return pos;
    }

    public static bool IsJsonNull(string str)
    {
        if (str == null || str == "[]" || str == "{}")
        {
            return true;
        }
        return false;
    }

    public static bool IsMyFriend(long accoundId)
    {
        for (int i = 0; i < DataMgr.m_RspGetSocialityInfoMessage.friendList.Count; i++)
        {
            if (DataMgr.m_RspGetSocialityInfoMessage.friendList[i].accountId == accoundId)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsHomeTownMyOrFriend()
    {
        if ((DataMgr.m_myOther == EnMyOhter.My) || (DataMgr.m_myOther == EnMyOhter.Other && PublicFunc.IsMyFriend(DataMgr.m_taInfo.accountId)))
        {
            return true;
        }
        return false;
    }

    public static bool FloatEqual(float a, float b,float clip)
    {
        if ((a - b > clip * -1) && (a - b) < clip)
            return true;
        else
            return false;
    }



    public static GameObject GetFirstRenderer(GameObject obj)
    {
        foreach (var item in obj.transform.GetComponentsInChildren<Renderer>())
        {
            return item.gameObject;
        }
        return null;
    }

    /// <summary>
    /// 得到寻路某个随机点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3 GetRandomPos(Vector3 pos)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 50;
        randomDirection += pos;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 50, 1);
        return  hit.position;
    }

    public static bool IsExistFile(string path)
    {
        if (File.Exists(AppConst.LocalPath + "/" + path))
        {
            return true;
        }
        return false;
    }

    public static void SendSkipNewGuide()
    {
        ReqUpdateOtherDataMessage req = new ReqUpdateOtherDataMessage();
        req.fieldName = "newStep";
        req.value = "999";
        HallSocket.Instance.SendMsgProto(MsgIdDefine.ReqUpdateOtherDataMessage, req);
        newguidepanel.Instance.Close();
        uiloadpanel.Instance.CloseByNewGuide();
        DataMgr.m_isNewGuide = false;
    }

    public static float GetHeightFactor()
    {
        return   Screen.height/ DataMgr.m_designHeight;
    }

    public static float GetWidthFactor()
    {
        return   Screen.width / DataMgr.m_designWidth;
    }
}

public class PathCompareTwoSprit : IComparer<string>
{
    public int Compare(string x, string y)
    {
        string[] aBuf = x.Split('\\');
        string[] bBuf = y.Split('\\');
        string[] aNameBuf = aBuf[aBuf.Length - 1].Split('.');
        string[] bNameBuf = bBuf[bBuf.Length - 1].Split('.');
        if (aNameBuf[0].Length > bNameBuf[0].Length)
        {
            return 1;
        }
        else if (aNameBuf[0].Length < bNameBuf[0].Length)
        {
            return -1;
        }
        else if (aNameBuf[0].Length == bNameBuf[0].Length)
        {
            return aNameBuf[0].CompareTo(bNameBuf[0]);
        }
        return 0;
    }
}

public class PathCompareOneSprit:IComparer<string>
{
    public int Compare(string x, string y)
    {
        string[] aBuf = x.Split('/');
        string[] bBuf = y.Split('/');
        string[] aNameBuf = aBuf[aBuf.Length - 1].Split('.');
        string[] bNameBuf = bBuf[bBuf.Length - 1].Split('.');
        if (aNameBuf[0].Length > bNameBuf[0].Length)
        {
            return 1;
        }
        else if (aNameBuf[0].Length < bNameBuf[0].Length)
        {
            return -1;
        }
        else if (aNameBuf[0].Length == bNameBuf[0].Length)
        {
            return aNameBuf[0].CompareTo(bNameBuf[0]);
        }
        return 0;
    }
}