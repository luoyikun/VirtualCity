using Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour {
	public GameObject startImg;
	public GameObject endImg;
	public GameObject prefab;
	public GameObject gameObjectPool;
	public ResEffect resEffect;

    public Canvas m_canvas;
    public Camera m_cam;
	// Use this for initialization
	void Start () {
		// public void Init(GameObject _gameObjectPool, List<GameObject> prefabList)
		List<GameObject> list = new List<GameObject>();
		list.Add (prefab);
		gameObjectPool.SetActive(false);
		prefab.SetActive(false);
		resEffect.Init (gameObjectPool,  list);
	    EventManager.Instance.AddEventListener(Common.EventStr.PlayGetGold, OnEvPlayGetGold);
    }

    void OnEvPlayGetGold(EventData data)
    {
        Debug.Log("GetPlayGold");
    }


    public void Play () {
        ExPlayGold play = new ExPlayGold();
        play.type = 0;
        play.source = PublicFunc.PosWorld2Overlay(m_cam,startImg.transform.position);
        play.target = endImg.transform.position;
        play.count = 20;
        EventManager.Instance.DispatchEvent(Common.EventStr.PlayGetGoldEffect, new EventDataEx<ExPlayGold>(play));

        //PlayOkOne();
    }


    void PlayOkOne()
    {
        Vector3 pos1 = PublicFunc.PosOverlay2World(m_cam, startImg.transform.position);
        Vector3 worldPos = PublicFunc.PosOverlay2World(m_cam, endImg.transform.position);

        resEffect.Play(0, pos1, worldPos, 20, (int num) =>
        {
            if (num == 0)
            {
                Debug.Log("======》第一个资源icon飞到资源栏");
            }
            else if (num == 1)
            {
                Debug.Log("======》最后一个资源icon飞到资源栏");
            }
        });
    }
}
