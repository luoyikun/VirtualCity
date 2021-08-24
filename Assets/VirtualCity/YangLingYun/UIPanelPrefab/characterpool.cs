using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class characterpool : MonoBehaviour {
    //public ScrollCallback scb;
    public Image BianKuang;
    public Image HeadImage;
    int id=0;
    public GameObject HeadImagePar;
    public GameObject ModelShowPar;
    //public Image 
    // Use this for initialization
    private void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
        //scb.callback = headimagecallback;
        //ClickListener.Get(gameObject).onClick = clickheadimage;
        //clickheadimage(HeadImagePar.transform.GetChild(0).gameObject);
    }
    void headimagecallback(int idx)
    {
        Tween toscale = DOTween.To(() => gameObject.transform.localScale, r => gameObject.transform.localScale = r, Vector3.one, 0.5f);
        gameObject.name = idx.ToString();
        if (createcharacterpanel.ccp.characterl[idx].Sex == 0)
        {
            gameObject.tag = "Man";
        }
        else if (createcharacterpanel.ccp.characterl[idx].Sex == 1)
        {
            gameObject.tag = "Woman";
        }
        AssetMgr.Instance.CreateSpr(createcharacterpanel.ccp.characterl[idx].Name, "charactericon", (sprite) => { HeadImage.sprite = sprite; });
    }
    void Start () {
		
	}
    void clickheadimage(GameObject obj)
    {
        PublicFunc.RemoveFromChild(ModelShowPar.transform);
        id = int.Parse(obj.name);
        obj.transform.GetChild(1).gameObject.SetActive(true);
        AssetMgr.Instance.CreateObj(createcharacterpanel.ccp.characterl[int.Parse(obj.name)].Name, "charactermodel" + createcharacterpanel.ccp.characterl[int.Parse(obj.name)].Name, ModelShowPar.transform, Vector3.zero, Vector3.zero, new Vector3(100, 100, 100), (charactermodel) => { charactermodel.layer = 9; });
    }
	// Update is called once per frame
	void Update () {
    }
}

