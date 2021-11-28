using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGuideDemo : MonoBehaviour
{
    public GameObject m_newGuide;
    public GameObject m_load;
    public static bool m_isNewGuideDemo = true;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_newGuide.SetActive(false);
        m_load.SetActive(false);
        yield return StartCoroutine(VcData.Instance.LoadDataSync());

        if (NewGuideMgr.SetGuideIdx(1))
        {
            NewGuideMgr.Instance.Startup();
            NewGuideMgr.Instance.StartOneNewGuide();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
