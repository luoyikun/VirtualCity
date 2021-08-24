using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSShow : MonoBehaviour {

    private void OnGUI()
    {
        if (AppConst.m_enTestServer == EnTestServer.Out && Application.isMobilePlatform)
        {
            return;
        }
        string text = string.Format(" FPS:{0}", 1.0f / Time.smoothDeltaTime);
        GUIStyle font = new GUIStyle();
        font.fontSize = 40;
        GUI.Label(new Rect(0, 0, 200, 200), text, font);

        if (GUI.Button(new Rect(0, 100, 50,50 ),"删除登陆信息"))
        {
            if (File.Exists(AppConst.LocalPath + "/Rsa.txt"))
            {
                File.Delete(AppConst.LocalPath + "/Rsa.txt");
            }

            if (File.Exists(AppConst.LocalPath + "/login.mi"))
            {
                File.Delete(AppConst.LocalPath + "/login.mi");
            }
            //GC.Collec();
            //SceneManager.LoadScene("Test");
        }
    }
}
