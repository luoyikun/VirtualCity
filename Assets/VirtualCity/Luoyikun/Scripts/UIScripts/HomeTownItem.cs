using Newtonsoft.Json;
using ProtoDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ModelData
{
    public string name;
}


public class HomeTownItem : MonoBehaviour {

    public Image m_icon;
    public Text m_name;
    public GameObject m_onlyGold;
    public GameObject m_onlyDiamon;
    public GameObject m_goldADiamon;

    public void SetCommonBuildInfo(int idx, EnHtSelectType type)
    {
        switch (type)
        {
            case EnHtSelectType.Home:
                {
                    HomeProperties info = DataMgr.homeProperties[idx];
                    gameObject.name = info.name;
                    AssetMgr.Instance.CreateSpr(info.icon, "homeicon", (spr) => { m_icon.sprite = spr; });

                    //名字
                    m_name.text = info.name;
                    if (info.gold > 0 && info.diamond == 0)
                    {
                        m_onlyGold.SetActive(true);
                        m_onlyDiamon.SetActive(false);
                        m_goldADiamon.SetActive(false);

                        m_onlyGold.transform.Find("Text").GetComponent<Text>().text = info.gold.ToString();
                    }
                    else if (info.gold == 0 && info.diamond > 0)
                    {
                        m_onlyGold.SetActive(false);
                        m_onlyDiamon.SetActive(true);
                        m_goldADiamon.SetActive(false);

                        m_onlyDiamon.transform.Find("Text").GetComponent<Text>().text = info.diamond.ToString();
                    }
                    else if (info.gold > 0 && info.diamond > 0)
                    {
                        m_onlyGold.SetActive(false);
                        m_onlyDiamon.SetActive(false);
                        m_goldADiamon.SetActive(true);

                        m_goldADiamon.transform.Find("TextGold").GetComponent<Text>().text = info.gold.ToString();
                        m_goldADiamon.transform.Find("TextDiamo").GetComponent<Text>().text = info.diamond.ToString();
                    }
                }
                break;
            case EnHtSelectType.Building:
                {
                    DevlopmentProperties info = DataMgr.devlopmentProperties[idx];
                    gameObject.name = info.cnName;
                    ModelData modelData = JsonConvert.DeserializeObject<ModelData>(info.modleData);

                    //加载图标
                    AssetMgr.Instance.CreateSpr(modelData.name, "commonbuild", (spr) => { m_icon.sprite = spr; });

                    //名字
                    m_name.text = info.cnName;
                    if (info.gold > 0 && info.diamond == 0)
                    {
                        m_onlyGold.SetActive(true);
                        m_onlyDiamon.SetActive(false);
                        m_goldADiamon.SetActive(false);

                        m_onlyGold.transform.Find("Text").GetComponent<Text>().text = info.gold.ToString();
                    }
                    else if (info.gold == 0 && info.diamond > 0)
                    {
                        m_onlyGold.SetActive(false);
                        m_onlyDiamon.SetActive(true);
                        m_goldADiamon.SetActive(false);

                        m_onlyDiamon.transform.Find("Text").GetComponent<Text>().text = info.diamond.ToString();
                    }
                    else if (info.gold > 0 && info.diamond > 0)
                    {
                        m_onlyGold.SetActive(false);
                        m_onlyDiamon.SetActive(false);
                        m_goldADiamon.SetActive(true);

                        m_goldADiamon.transform.Find("TextGold").GetComponent<Text>().text = info.gold.ToString();
                        m_goldADiamon.transform.Find("TextDiamo").GetComponent<Text>().text = info.diamond.ToString();
                    }
                }
                break;
            default:
                break;
        }
       
    }

}
