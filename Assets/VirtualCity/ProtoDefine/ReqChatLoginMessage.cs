using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqChatLoginMessage {
[ProtoMember(1)]
    public ChatUser chatUser;
[ProtoMember(2)]
    public List<long?> friendList;
[ProtoMember(3)]
    public List<long?> groupList;

    public ChatUser getChatUser() {
        return chatUser;
    }

    public ReqChatLoginMessage() {
    }

    public void setChatUser(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public ReqChatLoginMessage(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public List<long?> getFriendList() {
        return friendList;
    }

    public void setFriendList(List<long?> friendList) {
        this.friendList = friendList;
    }

    public List<long?> getGroupList() {
        return groupList;
    }

    public void setGroupList(List<long?> groupList) {
        this.groupList = groupList;
    }
}
}