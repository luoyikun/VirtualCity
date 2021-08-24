using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Audio_control : MonoBehaviour {

    public static Audio_control instance;

    public List<GameObject> Audio_obj;

   // AudioSource[] Audio_;

    public AudioClip[] audioClip;


    string []namearr={"music_login","music_btn","music_build","music_building","music_gold","music_highheelswalk","music_walk"};

    Dictionary<string, AudioClip> m_audioclipitem;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start ()
    {
        add_audio();
        initial();

        if (Application.isMobilePlatform)
        {    Audio_obj[0].GetComponent<AudioSource>().Play();
            Audio_obj[0].GetComponent<AudioSource>().loop = true;
        }


        if (File.Exists(AppConst.LocalPath + "/config"))
        {
            CloseallAudio(Convert.ToBoolean(JsonMgr.GetJsonString(AppConst.LocalPath + "/config")));
        }
        else
        {
            JsonMgr.SaveJsonString("true", AppConst.LocalPath + "/config");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="io">是否可重复播放</param>
    /// <param name="name">音乐的名字</param>
    public void playAudio(bool io,string name)
    {
        AudioSource au=Audio_obj[2].GetComponent<AudioSource>(); 
        au.clip= m_audioclipitem[name];
        au.loop = io;
        au.Play();
    }

    public void CloseAudio()
    {
        Audio_obj[2].GetComponent<AudioSource>().Stop();
    }

    void add_audio()
    {
        if (m_audioclipitem==null)
        {
            m_audioclipitem=new Dictionary<string, AudioClip>();
            for (int i = 0; i < audioClip.Length; i++)
            {
                m_audioclipitem.Add(namearr[i], audioClip[i]);
            }
        }
    }

    public void CloseallAudio(bool io)
    {
        for (int i = 0; i < Audio_obj.Count; i++)
        {
            Audio_obj[i].gameObject.SetActive(io);
        }
    }

    /// <summary>
    /// 生成音乐播放物体
    /// </summary>
    void initial()
    {
        Audio_obj = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            Audio_obj.Add(new GameObject());
            Audio_obj[i].name = "Audio_";
            Audio_obj[i].transform.parent = transform;
            Audio_obj[i].transform.localPosition= Vector3.zero;
            AudioSource au = Audio_obj[i].AddComponent<AudioSource>();
            au.clip = audioClip[i];
        }
    }


    public void loadAll()
    {
        int idx = 0;
        var bundle = AssetBundle.LoadFromFile("Assets/AssetsPackage/Music");
        foreach (AudioClip clip in bundle.LoadAllAssets())
        {
            audioClip[idx]=clip;
            idx++;
        }
        bundle.Unload(false);
    }

    


    bool io = true;

    /// <summary>
    /// 按钮特定音效
    /// </summary>
    public void palybut()
    {
        if (Application.isMobilePlatform)
        {   if (io)
            {
                StartCoroutine(star(Audio_obj[1].GetComponent<AudioSource>()));
            }
        }
    }

    IEnumerator star(AudioSource Au)
    {
        io = false;
        Au.Play();
        yield return new WaitForSeconds(Au.clip.length);
        io = true;
    }
}
