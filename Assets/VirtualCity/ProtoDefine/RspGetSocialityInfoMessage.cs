using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetSocialityInfoMessage {
[ProtoMember(1)]
    public List<ChatUser> friendList;
[ProtoMember(2)]
    public List<ChatGroup> inChatGroup;
[ProtoMember(3)]
    public List<SystemNotify> systemNotifies;
[ProtoMember(4)]
    public List<ProxyUser> proxyUsers;

    public RspGetSocialityInfoMessage() {
    }

    public RspGetSocialityInfoMessage(List<ChatUser> friendList, List<ChatGroup> inChatGroup, List<SystemNotify> systemNotifies, List<ProxyUser> proxyUsers) {
        this.friendList = friendList;
        this.inChatGroup = inChatGroup;
        this.systemNotifies = systemNotifies;
        this.proxyUsers = proxyUsers;
    }

    public List<ProxyUser> getProxyUsers() {
        return proxyUsers;
    }

    public void setProxyUsers(List<ProxyUser> proxyUsers) {
        this.proxyUsers = proxyUsers;
    }

    public List<ChatUser> getFriendList() {
        return friendList;
    }

    public void setFriendList(List<ChatUser> friendList) {
        this.friendList = friendList;
    }

    public List<ChatGroup> getInChatGroup() {
        return inChatGroup;
    }

    public void setInChatGroup(List<ChatGroup> inChatGroup) {
        this.inChatGroup = inChatGroup;
    }

    public List<SystemNotify> getSystemNotifies() {
        return systemNotifies;
    }

    public void setSystemNotifies(List<SystemNotify> systemNotifies) {
        this.systemNotifies = systemNotifies;
    }
}
}