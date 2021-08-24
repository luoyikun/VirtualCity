using Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;

public class FileDownloder : MonoBehaviour {

    public System.Diagnostics.Stopwatch sw;

    public string DownloadingFileName;
    public float DownloadingSpeed;
    public bool DownloadComplete;
    long m_lastDown = 0;
    public FileDownloder()
    {
        sw = new System.Diagnostics.Stopwatch();
        DownloadComplete = false;
        DownloadingFileName = "";
    }

    public bool DownloadFile(string url, string file)
    {
        if (DownloadingFileName == "")
        {
            DownloadComplete = false;
            DownloadingFileName = file;

            //using (WebClient client = new WebClient())
            //{
            //    sw.Start();
            //    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
            //    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
            //    //Debug.Log(String.Format("Download URL:{0} FILE:{1}", url, file));
            //    client.DownloadFileAsync(new Uri(url), file);
            //    Debug.Log("uri:" + new Uri(url));
            //}

            Loom.RunAsync(() =>
            {
                using (WebClient client = new WebClient())
                {
                    sw.Start();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
                    //Debug.Log(String.Format("Download URL:{0} FILE:{1}", url, file));
                    client.DownloadFileAsync(new Uri(url), file);
                }
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        sw.Reset();
        m_lastDown = 0;
        DownloadComplete = true;
        DownloadingFileName = "";
    }

    private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        DataMgr.m_downCur += (e.BytesReceived - m_lastDown);
        m_lastDown = e.BytesReceived;
        DownloadingSpeed = float.Parse((e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

        Loom.QueueOnMainThread((param) =>
        {
            EventManager.Instance.DispatchEvent(Common.EventStr.UpdateProgress, new EventDataEx<float>(DownloadingSpeed));
        }, null);
        if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        {
            sw.Reset();
            m_lastDown = 0;
            DownloadComplete = true;
            DownloadingFileName = "";
        }
    }
}
