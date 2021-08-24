using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHudForOne : MonoBehaviour {

    private bl_HUDText HUDRoot;
    [SerializeField] private GameObject TextPrefab;
    public GameObject m_Particle;
    //public ExampleType m_Type;

    private string[] Text = new string[] { "Floating Text", "Awasome", "But you say", "Nice", "Beatiful", "Surprising", "Impossible", "This is a big text for example purpose"
    ,"\n Add extra line","\n Add other line"};
    private string[] InfoText = new string[] { "Info Text", "Info Text Here", "Create a dialogue", };

    void Awake()
    {
        HUDRoot = bl_UHTUtils.GetHUDText;
    }

    private void Start()
    {
        CreateHud();
    }
    void CreateHud()
    {
        //Build the information
        HUDTextInfo info4 = new HUDTextInfo(transform, InfoText[Random.Range(0, InfoText.Length)]);
        info4.Color = Color.white;
        info4.Size = Random.Range(5, 12);
        info4.Speed = Random.Range(10, 20);
        info4.VerticalAceleration = 1;
        info4.VerticalPositionOffset = 5;
        info4.VerticalFactorScale = Random.Range(1.2f, 3);
        info4.Side = bl_Guidance.Right;
        info4.TextPrefab = TextPrefab;
        info4.FadeSpeed = 0;
        info4.ExtraDelayTime =99999;
        info4.AnimationType = bl_HUDText.TextAnimationType.None;
        //Send the information
        HUDRoot.NewText(info4);
    }
}
