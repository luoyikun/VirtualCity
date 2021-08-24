using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProtoBuf;
namespace ProtoDefine
{
    [ProtoContract]
    public class ProxyUser
    {
        [ProtoMember(1)]
        public ChatUser user;
        //代理人数
        [ProtoMember(2)]
        public int proxyNum;
        //总代理代理人数
        [ProtoMember(3)]
        public int proxyTotleNum;
        //是否在线
        [ProtoMember(4)]
        public int online;

    }
}