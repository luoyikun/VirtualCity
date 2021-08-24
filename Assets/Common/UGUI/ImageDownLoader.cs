using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(RawImage))]
public class ImageDownLoader : MonoBehaviour
{

    private static Dictionary<string,Texture2D> mImageCacheDict = new Dictionary<string, Texture2D>();
    private RawImage mImage;

    void Awake()
    {
        mImage = GetComponent<RawImage>();

        //Test
        //SetOnlineTexture("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1506247880185&di=df9429c3802ea6cb5fae77643d926047&imgtype=0&src=http%3A%2F%2F58pic.ooopic.com%2F58pic%2F22%2F90%2F18%2F18r58PICEP7.png");
    }


    public void SetOnlineTexture(string mUrl)
    {
        Action<bool,Texture2D> handle = (bool mIsSuccess, Texture2D mSetupTexture) =>
        {
            if (mIsSuccess)
            {
                mImage.texture = mSetupTexture;
                //mImage.SetNativeSize();
            }
            else
            {
                //TODO 加载失败处理
            }
        };

        Texture2D mTexture;
        if (TryGetImageInCache(mUrl, out mTexture))
        {
            handle(true, mTexture);
        }
        else
        {
            StartCoroutine(DownLoadImages(mUrl, handle));
        }
    }

    IEnumerator DownLoadImages(string mUrl, Action<bool,Texture2D> mCallBack)
    {
        WWW www = new WWW(mUrl);
        while (!www.isDone)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error))
        {
            UpdateImageInCache(mUrl, www.texture);
            mCallBack(true, www.texture);
        }
        else
        {
            mCallBack(false, null);
        }

        www.Dispose();
        yield return null;
    }

    public static bool TryGetImageInCache(string mPath, out Texture2D mTexture)
    {
        return mImageCacheDict.TryGetValue(mPath, out mTexture);
    }

    public static void UpdateImageInCache(string mPath, Texture2D mTexture)
    {
        if (mImageCacheDict.ContainsKey(mPath))
        {
            mImageCacheDict[mPath] = mTexture;
        }
        else
        {
            mImageCacheDict.Add(mPath, mTexture);
        }
    }
}

