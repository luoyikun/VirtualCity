using BE;
using DG.Tweening;
using Framework.Event;
using Framework.Tools;
using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExPlayGold
{
    public int type;
    public Vector3 source;
    public Vector3 target;
    public int count;
}

/// <summary>
/// PlayAnim拷贝自
/// </summary>
public class reseffectpanel : UGUIPanel {
    public GameObject m_prefab;
    public GameObject m_gameObjectPool;
    public ResEffect m_resEffect;

    private  BufferPool mTipsPool;
    int m_max = 20;
    // Use this for initialization
    void Start () {
        PoolInit();

        //List<GameObject> list = new List<GameObject>();
        //list.Add(m_prefab);
        //m_gameObjectPool.SetActive(false);
        //m_prefab.SetActive(false);
        //m_resEffect.Init(m_gameObjectPool, list);


        //EventManager.Instance.AddEventListener(Common.EventStr.PlayGetGoldEffect, OnEvPlayGetGoldEffect);
    }


    void PoolInit()
    { 
        if (mTipsPool == null)
        {
            mTipsPool = new BufferPool(m_prefab, m_gameObjectPool.transform, m_max);
        }
    }
    public override void OnOpen()
    {
        EventManager.Instance.AddEventListener(Common.EventStr.PlayGetGoldEffect, OnEvPlayGetGoldEffect);
    }

    void OnEvPlayGetGoldEffect(EventData data)
    {
        var exdata = data as EventDataEx<ExPlayGold>;
        ExPlayGold play = exdata.GetData();
        OnPlayAddNum(play.source, play.target,play.count);
        //AddCoinEffect(play.source, play.target);
        //PlayEffect(play.type, play.source, play.target, play.count);
    }

    public void OnPlayAddNum(Vector3 oriPos, Vector3 toPos,int gold)
    {
        GameObject tempObj = mTipsPool.GetObject();
        tempObj.transform.position = oriPos;
        //tempObj.GetComponent<TextMeshProUGUI>().text = "+" + gold.ToString();
        //tempObj.GetComponent<TextMeshProUGUI>().fontSize = 50;
        tempObj.GetComponent<Text>().text = "+" + gold.ToString();
        Tweener tweener = tempObj.transform.DOMove(toPos, 1.0f);
        //设置这个Tween不受Time.scale影响
        tweener.SetUpdate(true);
        //设置移动类型
        tweener.SetEase(Ease.InQuad);
        BETween.alpha(tempObj.gameObject, 0.3f, 1.0f, 0.0f);
        tweener.onComplete = delegate ()
        {
            mTipsPool.Recycle(tempObj);
        };
    }


    public void AddCoinEffect(Vector3 oriPos, Vector3 toPos)
    {
        for (int i = 0; i < m_max; i++)
        {
            GameObject tempObj = mTipsPool.GetObject();
            Vector3 oriPosNew = new Vector3(Random.Range(oriPos.x - 100, oriPos.x + 100), Random.Range(oriPos.y - 100, oriPos.y + 100), 0f);
            //StartCoroutine(YieldDoAddCoinEffect(tempObj, oriPosNew,toPos));
            tempObj.transform.position = oriPosNew;

            Tweener tweener = tempObj.transform.DOMove(toPos, 1.0f);

            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(Ease.InQuad);


            //Tween t1 = DOTween.To(() => rote, x => rote = x, -360f, 1f).SetEase(Ease.OutQuad).OnUpdate(() =>
            //{
            //    starImg.GetComponent<RectTransform>().GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, rote);
            //});

            tweener.onComplete = delegate ()
            {
                mTipsPool.Recycle(tempObj);
            };
            

        }
    }

    IEnumerator YieldDoAddCoinEffect(GameObject obj,Vector3 oriPos,Vector3 toPos)
    {
        Vector3 rotateSpeed = new Vector3(
            Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
            Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
            Mathf.Lerp(0, 360, Random.Range(0, 1.0f))) * 3;

        obj.transform.position = oriPos;
        while ((obj.transform.position - toPos).sqrMagnitude >= 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, toPos, 1.0f);
            

            obj.transform.Rotate(rotateSpeed.x * Time.deltaTime,rotateSpeed.y * Time.deltaTime, rotateSpeed.z * Time.deltaTime);

            //obj.transform.position = Vector3.Lerp(Vector3.Lerp(sourceIn, sourceIn + ctrlPoints[i], coins[i].mMoveTime * 5), targetIn, coins[i].mMoveTime);
            yield return null;
        }
        mTipsPool.Recycle(obj);
    }

    public override void OnClose()
    {
        EventManager.Instance.RemoveEventListener(Common.EventStr.PlayGetGoldEffect, OnEvPlayGetGoldEffect);
    }

    void PlayEffect(int type, Vector3 source, Vector3 target, int count, System.Action<int> onFinish = null)
    {
        m_resEffect.Play(type, source, target, count, onFinish);
    }


}
