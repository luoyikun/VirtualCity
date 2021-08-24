using UnityEngine;
using System.Collections.Generic;
using System;

namespace Framework.UI
{
    [Serializable]
    public class UIPanelInfo : ISerializationCallbackReceiver
    {
        //和json文件信息对应的一个类
        //不可序列化的，因为这里unity解析json文件时没办法解析枚举类型，所以下面定义了一个string类型的字段panelTypeString,用于两者的转化
        [NonSerialized]
        public UIPanelType panelType;//面板类型
        public string panelTypeString;
        public string path;//面板所在路径

        //实现ISerializationCallbackReceiver的接口， 反序列化方法，从文本信息到对象
        public void OnAfterDeserialize()
        {
            UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);//把一个字符串转化为一个枚举
            panelType = type;
        }

        //实现接口， 序列化方法，从对象到文本信息
        public void OnBeforeSerialize() { }
    }

    [Serializable]
    public class UIPanelTypeJson //内部类里面就一个链表容器,用来配合解析
    {
        public List<UIPanelInfo> infoList;
    }
}