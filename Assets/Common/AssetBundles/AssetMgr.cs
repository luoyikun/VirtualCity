//#define EditorLoadAb
using AssetBundles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Framework.UI;
using Framework.Event;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetMgr : MonoBehaviour {

    static AssetMgr m_instance = null;
    static bool m_isLoad = false;
    bool m_isBigVersionUpdate = false;
    static public AssetMgr Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("AssetMgr");
                DontDestroyOnLoad(obj);
                m_instance = obj.AddComponent<AssetMgr>();
                //InitResources();
            }
            return m_instance;
        }
    }

    /// <summary>
    /// 生成物體
    /// </summary>
    /// <param name="assetName">预制体名字</param>
    /// <param name="abName"></param>
    /// <param name="par">要加入的父物体</param>
    /// <param name="pos">生成的位置</param>
    /// <param name="angles">旋转</param>
    /// <param name="scale">缩放</param>
    /// <param name="onFinish"></param>
    /// <param name="path"></param>
    public void CreateObj(string assetName,string abName,Transform par,Vector3 pos,Vector3 angles,Vector3 scale, UnityAction<GameObject> onFinish = null,string path = "")
    {
        if (PoolMgr.Instance.IsExistKey(assetName) == false)
        {
            PoolMgr.Instance.CreateKey(assetName);
        }
        if (PoolMgr.Instance.m_dicPool[assetName].m_queChild.Count == 0)
        {
            onFinish += (param) =>
            {
                if (param.GetComponent<PoolKeyName>() == null)
                {
                    param.AddComponent<PoolKeyName>();
                }
                param.GetComponent<PoolKeyName>().m_keyName = assetName;
                //PoolMgr.Instance.m_dicPool[assetName].m_queChild.Enqueue(param);
            };
            //par = PoolMgr.Instance.m_dicPool[assetName].m_poolPar;
            Instance.StartCoroutine(Instance.YieldCreateObj(assetName, abName, par, pos, angles, scale, onFinish, path));
        }
        else {
            GameObject go = PoolMgr.Instance.m_dicPool[assetName].m_queChild.Dequeue();

            go.transform.SetParent(par, false);
            go.transform.localPosition = pos;
            go.transform.localEulerAngles = angles;
            go.name = assetName;


            if (go.GetComponent<PoolKeyName>() == null)
            {
                go.AddComponent<PoolKeyName>();
            }
            go.GetComponent<PoolKeyName>().m_keyName = assetName;


            if (scale.x != -10000)
            {
                go.transform.localScale = scale;
            }
            if (onFinish != null)
            {
                onFinish(go);
            }
        }
    }

    public void CreateMat(string assetName, string abName,  UnityAction<Material> onFinish = null, string path = "")
    {
        Instance.StartCoroutine(Instance.YieldCreateMat(assetName, abName, onFinish, path));
    }


    public void CreateObjOne(string assetName, string abName, Transform par, Vector3 pos, Vector3 angles, Vector3 scale, UnityAction<GameObject> onFinish = null, string path = "")
    {
        Instance.StartCoroutine(Instance.YieldCreateObj(assetName, abName, par, pos, angles, scale, onFinish, path));
    }


    //public void CreateSpr(string assetName, string abName, OnFinishSpr onFinish = null, string path = "")
    //{
    //    Instance.StartCoroutine(Instance.YieldCreateSpr(assetName, abName, onFinish, path));
    //}

    public void CreateSpr(string assetName, string abName, UnityAction<Sprite> act = null, string path = "")
    {
        Instance.StartCoroutine(Instance.YieldCreateSpr(assetName, abName, act,  path));
    }

    public void CreateText(string assetName,string abName, UnityAction<string> onfinish = null, string path = "")
    {
        Instance.StartCoroutine(Instance.YieldCreateText(assetName,abName,onfinish,  path));
    }

    public IEnumerator YieldCreateText(string assetName,string abName, UnityAction<string> onFinish = null,string path = "")
    {
        //yield return StartCoroutine(Instance.Initialize());
        //assetName = assetName.ToLower();
        if (abName == "")
        {
            abName = assetName.ToLower();
        }

        if (abName.EndsWith(".assetbundle") == false)
        {
            abName += ".assetbundle";
        }

        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(abName, assetName, typeof(TextAsset),path);
        if (request == null) yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        TextAsset prefab = request.GetAsset<TextAsset>();
        if (prefab == null)
        {
            Debug.LogError("ab文字错误:" + abName + "+" + assetName);
        }
        if (onFinish != null)
        {
            onFinish(prefab.text);
        }
    }

    public IEnumerator YieldCreateSpr(string assetName, string abName, UnityAction<Sprite> onFinish = null,  string path = "")
    {
        //yield return StartCoroutine(Instance.Initialize());
        //assetName = assetName.ToLower();
        if (abName == "")
        {
            abName = assetName.ToLower();
        }
        if (abName.EndsWith(".assetbundle") == false)
        {
            abName += ".assetbundle";
        }
#if UNITY_EDITOR && EditorLoadAb
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(abName, assetName, typeof(Texture2D));
        if (request == null) yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        Texture2D prefab = request.GetAsset<Texture2D>();
        if (prefab == null)
        {
            Debug.LogError("ab找不到图:" + abName + "中的" + assetName);
        }
        Sprite sprite = Sprite.Create(prefab, new Rect(0, 0, prefab.width, prefab.height), Vector2.zero);
        if (onFinish != null)
        {
            onFinish(sprite);
        }
#else
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(abName, assetName, typeof(Sprite),path);

        if (request == null)
        {
            Debug.LogError("加载asset失败:" + assetName);
            yield break;
        }

        yield return StartCoroutine(request);

        // Get the asset.
        Sprite prefab = request.GetAsset<Sprite>();
        if (prefab == null)
        {
            Debug.LogError("ab找不到图:" + abName + "中的" + assetName);
        }
        if (onFinish != null)
        {
            onFinish(prefab);
        }
#endif
    }

    public IEnumerator YieldCreateMat(string assetName, string abName, UnityAction<Material> onFinish = null, string path = "")
    {
        if (abName == "")
        {
            abName = assetName.ToLower();
        }

        if (abName.EndsWith(".assetbundle") == false)
        {
            abName += ".assetbundle";
        }
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(abName, assetName, typeof(Material), path);
        if (request == null) yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        Material prefab = request.GetAsset<Material>();
        if (prefab == null)
        {
            Debug.LogError("ab obj错误:" + abName + "+" + assetName);
        }
        if (onFinish != null)
        {
            onFinish(prefab);
        }
        AssetBundleManager.UnloadAssetBundle(assetName);

    }

    public IEnumerator YieldCreateObj(string assetName, string abName,Transform par, Vector3 pos, Vector3 angles, Vector3 scale, UnityAction<GameObject> onFinish = null, string path = "")
    {
        //if (m_isAbMgrInit == false)
        //{
        //    m_isAbMgrInit = true;
        //}

        //yield return StartCoroutine(Instance.Initialize());

        //assetName = assetName.ToLower();
        if (abName == "")
        {
            abName = assetName.ToLower();
        }

        if (abName.EndsWith(".assetbundle") == false)
        {
            abName += ".assetbundle";
        }
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(abName, assetName, typeof(GameObject),path);
        if (request == null)
        {
            Debug.LogError("加载asset失败:" + assetName);
            yield break;
        }
        yield return StartCoroutine(request);

        // Get the asset.
        GameObject prefab = request.GetAsset<GameObject>();
        if (prefab == null)
        {
            Debug.LogError("ab obj错误:" + abName + "+" + assetName);
        }


        GameObject go = Instantiate(prefab) as GameObject;
        go.transform.SetParent(par, false);
        go.transform.localPosition = pos;
        go.transform.localEulerAngles = angles;
        go.name = assetName;
        //go.SetActive(true);
        

        if (scale.x != -10000)
        {
            go.transform.localScale = scale;
        }
        if (onFinish != null)
        {
            onFinish(go);
        }
        AssetBundleManager.UnloadAssetBundle(assetName);
        //AssetBundleManager.UnloadAssetBundle(assetName, false, false);
    }

    public void Init(System.Action varFinish = null)
    {
        if (m_isLoad == true)
        {
            if (varFinish != null)
            {
                varFinish();
            }
        }
        Instance.StartCoroutine(Initialize(varFinish));
    }
    IEnumerator Initialize(Action act = null)
    {
        yield return StartCoroutine(InitResources());

        if (m_isBigVersionUpdate == true)
        {
           yield break; 
        }
        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project. 
        // 	Another approach would be to make this configurable in the standalone player.)
#if UNITY_EDITOR && EditorLoadAb
        AssetBundleManager.SetDevelopmentAssetBundleServer();
#else
        //同时要适应移动平台
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        AssetBundleManager.SetSourceAssetBundleURL(AppConst.AbDataPath + "/");
        // Or customize the URL based on your deployment or configuration
        //AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
#endif

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);

        yield return StartCoroutine(VcData.Instance.LoadDataSync());
        //VcData.Instance.LoadData();
        if (act != null)
        {
            act();
        }

        // 读取固定策划数据
        m_isLoad = true;
    }

    //资源初始化，解压或者更新资源
    IEnumerator InitResources()
     {

        //#if UNITY_EDITOR
        //        yield return 1;
        //#else
        if (AppConst.m_isUpdateRes == true)
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                yield return m_instance.StartCoroutine(doBigVersionUpdate());
                if (m_isBigVersionUpdate == true)
                {
                    yield break;
                }
            }
            yield return m_instance.StartCoroutine(doUpdateResources());
            

            //if (Directory.Exists(AppConst.AbDataPath) && File.Exists(AppConst.AbDataPath + "/files.txt"))
            //{
            //    //LoadUpdateResourcePanel();

            //    //从网上下载更新资源
            //    Debug.Log("更新资源");
            //    yield return m_instance.StartCoroutine(doUpdateResources());
            //}
            //else
            //{
            //    Debug.Log("释放资源");
            //    //释放资源至沙盘路径
            //    yield return m_instance.StartCoroutine(doExtractResources());
            //}
        }
        //else
        //{
        //    yield return m_instance.StartCoroutine(doExtractResources());
        //}
//#endif
    }

    //IEnumerator doExtractResources()
    //{

    //    //TODO 资源释放面板
    //    UIManager.Instance.PushPanelFromRes(UIPanelName.firstpanel, UIManager.CanvasType.Screen);

    //    string assetsPath = AppConst.StreamingAssetsPath;
    //    string dataPath = AppConst.AbDataPath;

    //    //Debug.Log("Extracting from " + assetsPath + " to " + dataPath);

    //    string infile = assetsPath + "/files.txt";
    //    string outfile = dataPath + "/files.txt";
    //    yield return m_instance.StartCoroutine(ExtractFile(infile, outfile));
    //    yield return null;

    //    int preload = 0;
    //    string[] files = File.ReadAllLines(outfile);
    //    DataMgr.m_downTotal = files.Length;
    //    DataMgr.m_downCur = 0;
    //    for (int i = 0; i < files.Length; i++)
    //    {
    //        string file = files[i];
    //        string[] fs = file.Split('|');
    //        infile = assetsPath + "/" + fs[0];
    //        outfile = dataPath + "/" + fs[0];

    //        //Debug.Log("Extracting System File:>" + infile);
    //        yield return m_instance.StartCoroutine(ExtractFile(infile, outfile));

    //        //释放资源面板
    //        //if (fs[0] == UIPanelName.firstpanel)
    //        //{
    //        //    UIManager.Instance.PushPanel(UIPanelName.firstpanel, UIManager.CanvasType.Screen);
    //        //}
    //        yield return null;
            
    //        preload++;
    //        DataMgr.m_downCur = preload;
    //        EventManager.Instance.DispatchEvent(Common.EventStr.UpdateProgress, new EventDataEx<float>(-100.0f));
    //    }

    //    //释放资源面板
    //    //infile = assetsPath + "updateresource.assetbundle";
    //    //outfile = dataPath + "updateresource.assetbundle";
    //    //Debug.Log("Extracting UpdatePanel File:>" + infile);
    //    //yield return StartCoroutine(ExtractFile(infile, outfile));
    //    yield return null;
    //    UIManager.Instance.PopSelf();
    //    //LoadUpdateResourcePanel();

    //    //todo 释放完再执行更新资源
    //    yield return StartCoroutine(doUpdateResources());
    //}

     IEnumerator ExtractFile(string infile, string outfile)
     {
        Debug.Log("from:" + infile +"--------------to:" + outfile);
        var dir = Path.GetDirectoryName(outfile);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
        }
        else
        {
            File.Copy(infile, outfile, true);
            
        }
     }

    IEnumerator doBigVersionUpdate()
    {
        
        string dataPath = AppConst.BigVerionSavePath + "/"; //数据目录
        string url = AppConst.BigVersionDownUrl + "/";
        string apkName = "vc.apk";
        if (Directory.Exists(AppConst.BigVerionSavePath) == false)
        {
            Directory.CreateDirectory(AppConst.BigVerionSavePath);
        }
        WWW www = new WWW(url + "BigVersion.txt");
        yield return www;
        if (www.error != null)
        {
            Debug.LogError("大版本检测失败:" + www.error.ToString());
            //StartGameManager();
            Debug.Log("不能更新，开始游戏");
            
            yield break;
        }

        string[] remoteVersion = www.text.Split('|');
        if (remoteVersion[0] == VirtualCityMgr.m_instance.m_bigVersion)
        {
            Debug.Log("大版本一致");
        }
        else
        {
            m_isBigVersionUpdate = true;
            DataMgr.m_downTotal = long.Parse(remoteVersion[1]) * 1024 * 1024;
            List<string> updateUrls = new List<string>();
            List<string> updateFiles = new List<string>();
            updateUrls.Add(url + apkName);
            updateFiles.Add(dataPath + apkName);
            UIManager.Instance.PushPanelFromRes(UIPanelName.firstpanel, UIManager.CanvasType.Screen);
            if (DataMgr.m_downTotal > 0)
            {
                ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen, false, false);
                ispanel.SetContent("提示", "本次大版本更新需要下载" + (DataMgr.m_downTotal / 1024d/1024d).ToString("0.00") + "MB资源");
                ispanel.m_cancel = () =>
                {
                    Debug.Log("取消");
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif


                };

                while (ispanel.m_isClickOk == false)
                {
                    yield return null;
                }

                yield return StartCoroutine(doDownResources(updateFiles, updateUrls));
            }

            Debug.Log("apk下载完成:"+ dataPath + apkName);
            UIManager.Instance.PopSelf();

            string path = dataPath + apkName;
            path = path.Replace(":", "");
            AndroidFunc.InstallApk(path);

        }
        yield return null;
    }
    private IEnumerator doUpdateResources()
    {
        Debug.Log("更新资源");
        if (AppConst.m_isUpdateRes == false)
        {
            yield break;
        }
        UIManager.Instance.PushPanelFromRes(UIPanelName.firstpanel, UIManager.CanvasType.Screen);
        //LuaCall("UpdateResourceAPI", "ShowUpdateInfo", UpdateResourcePanelApi, 1, 0);
        Debug.Log("打开了第一个面板");
        string dataPath = AppConst.AbDataPath  + "/"; //数据目录
        string url = AppConst.DownUri + "/";

        if (Directory.Exists(AppConst.AbDataPath) == false)
        {
            Directory.CreateDirectory(AppConst.AbDataPath);
        }
        WWW www = new WWW(url + "files.txt");
        Debug.Log("下载的files的url:"+www.url);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError("Update Resources Failed! :" + www.error.ToString());
            //StartGameManager();
            Debug.Log("不能更新，开始游戏");
            UIManager.Instance.PopSelf();
            yield break;
        }

        List<string> updateUrls = new List<string>();
        List<string> updateFiles = new List<string>();

        File.WriteAllBytes(dataPath + "files.txt", www.bytes);
        string[] files = www.text.Split('\n');
        int needUpdateSize = 0;
        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            if (string.IsNullOrEmpty(file))
                continue;

            string[] keyValue = file.Split('|');
            string filename = keyValue[0];
            string localfile = (dataPath + filename).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + filename;
            bool needUpdate = !File.Exists(localfile);
            if (!needUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = PublicFunc.GetFileMD5(localfile);
                needUpdate = !remoteMd5.Equals(localMd5);
            }

            if (needUpdate)
            {
                updateUrls.Add(fileUrl);
                updateFiles.Add(localfile);

                needUpdateSize += int.Parse(keyValue[2]);
            }
        }

        //获取总得更新量
        //long countLength = 0;
        //int cnt = 0;
        //for (int i = 0; i < updateUrls.Count; i++)
        //{
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(updateUrls[i]);
        //    request.Method = "HEAD";
        //    countLength += request.GetResponse().ContentLength;
        //    cnt++;
        //}

        //float dTotalLength = (float)countLength * 1.0f / (1024 * 1024);

        //DataMgr.m_downTotal = countLength;
        //Debug.Log("总下载量:" + countLength);
        //Debug.Log("下载个数:" + cnt);

        DataMgr.m_downTotal = needUpdateSize * 1024;

        if (DataMgr.m_downTotal > 0)
        {
            Debug.Log("打开了更新确认下载面板");
            ispanel ispanel = (ispanel)UIManager.Instance.PushPanelFromRes(UIPanelName.ispanel, UIManager.CanvasType.Screen,false,false);
            ispanel.SetContent("提示", "本次更新需要下载" + (DataMgr.m_downTotal / 1024d / 1024d).ToString("0.00") + "MB资源");
            ispanel.m_cancel = () => { Debug.Log("取消");
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif


            };

            while (ispanel.m_isClickOk == false)
            {
                yield return null;
            }

            yield return StartCoroutine(doDownResources(updateFiles, updateUrls));
        }
        UIManager.Instance.PopSelf();
        //StartGameManager();
    }


    private IEnumerator doDownResources(List<string> updateFiles, List<string> updateUrls)
    {
        for (int i = 0; i < updateFiles.Count; i++)
        {
            File.Delete((string)updateFiles[i]);

            var downloader = new FileDownloder();

            Debug.Log("Updating File:>" + updateUrls[i]);
            ////LuaCall("UpdateResourceAPI", "DebugText", UpdateResourcePanelApi, "下载:" + keyValue[0]);

            downloader.DownloadFile((string)updateUrls[i], (string)updateFiles[i]);
            while (!downloader.DownloadComplete)
            {
                yield return null;
            }

            yield return null;
        }
        
    }
    public GameObject CreateObjSync(string name, string abName,Vector3 pos, Vector3 angle, Vector3 scale, Transform par = null,string path = "")
    {
        if (abName == "")
        {
            abName = name;
        }
        GameObject go = null;
        GameObject prefab = null;
#if UNITY_EDITOR && EditorLoadAb

        string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, name);
        if (assetPaths.Length == 0)
        {
            Debug.LogError("There is no asset with name \"" + name + "\" in " + abName);
            return null;
        }

        // @TODO: Now we only get the main object from the first asset. Should consider type also.
        prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
#else
        prefab = AssetBundleManager.LoadAssetSync(abName, name, path);
#endif
        go = GameObject.Instantiate(prefab);
        go.name = name;
        go.transform.parent = par;
        go.transform.localPosition = pos;
        go.transform.localEulerAngles = angle;
        go.transform.localScale = scale;

        //AssetBundleManager.UnloadAssetBundle(name, false, false);


        return go;
    }

    public GameObject CreateObjSync(string name, string abName, Transform par = null, string path = "")
    {
        if (abName == "")
        {
            abName = name;
        }
        GameObject go = null;
        GameObject prefab = null;
#if UNITY_EDITOR && EditorLoadAb

        string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(abName, name);
        if (assetPaths.Length == 0)
        {
            Debug.LogError("There is no asset with name \"" + name + "\" in " + abName);
            return null;
        }

        // @TODO: Now we only get the main object from the first asset. Should consider type also.
        prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
#else
        prefab = AssetBundleManager.LoadAssetSync(abName, name, path);
#endif
        go = GameObject.Instantiate(prefab);
        go.name = name;
        go.transform.SetParent(par,false);

        AssetBundleManager.UnloadAssetBundle(name, false, true);


        return go;
    }

}
