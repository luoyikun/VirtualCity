﻿using UnityEngine;
using System.Collections;
using AssetBundles;
using UnityEditor;

/// <summary>
/// added by wsh @ 2018.01.03
/// 功能：打包前的AB检测工作
/// </summary>

public static class CheckAssetBundles
{
    public static void SwitchChannel(string channelName)
    {
        var channelFolderPath = AssetBundleUtility.PackagePathToAssetsPath(AssetBundleConfig.ChannelFolderName);
        var guids = AssetDatabase.FindAssets("t:textAsset", new string[] { channelFolderPath });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameUtility.SafeWriteAllText(path, channelName);
        }
        AssetDatabase.Refresh();
    }

    public static void ClearAllAssetBundles()
    {
        var assebundleNames = AssetDatabase.GetAllAssetBundleNames();
        var length = assebundleNames.Length;
        var count = 0;
        foreach (var assetbundleName in assebundleNames)
        {
            count++;
            EditorUtility.DisplayProgressBar("Remove assetbundle name :", assetbundleName, (float)count / length);
            AssetDatabase.RemoveAssetBundleName(assetbundleName, true);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        assebundleNames = AssetDatabase.GetAllAssetBundleNames();
        if (assebundleNames.Length != 0)
        {
            Debug.LogError("Something wrong!!!");
        }
    }

    public static void RunAllCheckers(bool checkChannel)
    {
        var guids = AssetDatabase.FindAssets("t:AssetBundleDispatcherConfig", new string[] { AssetBundleInspectorUtils.DatabaseRoot });
        var length = guids.Length;
        var count = 0;
        foreach (var guid in guids)
        {
            count++;
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath) == false)
            {
                var config = AssetDatabase.LoadAssetAtPath<AssetBundleDispatcherConfig>(assetPath);
                if (config != null)
                {
                    config.Load();
                    if (string.IsNullOrEmpty(config.PackagePath) == false)
                    {
                        EditorUtility.DisplayProgressBar("Run checker :", config.PackagePath, (float)count / length);
                        AssetBundleDispatcher.Run(config, checkChannel);
                    }
                }
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    public static void Run(bool checkChannel)
    {
        ClearAllAssetBundles();
        RunAllCheckers(checkChannel);
    }
}
