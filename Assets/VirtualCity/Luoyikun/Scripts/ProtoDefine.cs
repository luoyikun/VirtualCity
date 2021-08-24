using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProtoDefine
{
    

    //[ProtoContract]
    //public class RspLoginMessage
    //{
    //    [ProtoMember(1)]
    //    public int? code;
    //    //code = 0,账号在线不能
    //    [ProtoMember(2)]
    //    public string tips;
    //    [ProtoMember(3)]
    //    public string serverIp;
    //    [ProtoMember(4)]
    //    public long accountId;
    //}

    [ProtoContract]
    public class HeartReq
    {
        [ProtoMember(1)]
        public long? accountId;
    }


    [ProtoContract]
    public class UpdateUserInfoMessage
    {
        [ProtoMember(1)]
        public List<UserInfoMap> info = new List<UserInfoMap>();
        [ProtoMember(2)]
        public long accountId;
    }

    
    [ProtoContract]
    public class RspCodeTip
    {
        [ProtoMember(1)]
        public int code;
        [ProtoMember(2)]
        public string tip;
    }

    public class JsonTest
    {
        public int? code;
    }

   
    [ProtoContract]
    public class ReqChatLogoutMessage
    {
        [ProtoMember(1)]
        public long? accountID;
    }
    [ProtoContract]
    public class ReqCreateGroupMessage
    {
        [ProtoMember(1)]
        public string groupName;
    }
    [ProtoContract]
    public class ReqDeleteFrindMessage
    {
        [ProtoMember(1)]
        public long? friendId;
    }
    [ProtoContract]
    public class ReqDeleteGroupMessage
    {
        [ProtoMember(1)]
        public long? groupId;
    }
    [ProtoContract]
    public class ReqDeleteParaMateMessage
    {
        [ProtoMember(1)]
        public List<long> paramateIds;
        [ProtoMember(2)]
        public long? groupId;
    }
    [ProtoContract]
    public class ReqGetSocialityInfoMessage
    {
        [ProtoMember(1)]
        public List<long?> friendIdList = new List<long?>();
        [ProtoMember(2)]
        public long? accountId;
    }
    [ProtoContract]
    public class ReqUpdateGroupMessage
    {
        [ProtoMember(1)]
        public long? gourpId;
        [ProtoMember(2)]
        public String groupName;
    }
    [ProtoContract]
    public class RspChatLogoutMessage
    {
        [ProtoMember(1)]
    public int code;
        [ProtoMember(2)]
    public String tip;
        [ProtoMember(3)]
    public long accountId;
    }

    [ProtoContract]
    public class RspSearchUserMessage
    {
        [ProtoMember(1)]
    public List<ChatUser> userName;
    }
    [ProtoContract]
    public class SystemNotifyMessage
    {

        [ProtoMember(1)]
        public SystemNotify systemNotify;

    }


    [ProtoContract]
    public class ReqGetPlayerByIdMessage
    {
        [ProtoMember(1)]
        public List<long?> accountIds;
    }
    [ProtoContract]
    public class RspGetPlayerByIdMessage
    {
        [ProtoMember(1)]
        public List<ChatUser> users;
    }
}
