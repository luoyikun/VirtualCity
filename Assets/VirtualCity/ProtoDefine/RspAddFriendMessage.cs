using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspAddFriendMessage {
[ProtoMember(1)]
    public ChatUser chatUser;
[ProtoMember(2)]
    public long fromAccountId;


    public RspAddFriendMessage() {
    }

    public RspAddFriendMessage(ChatUser chatUser, long fromAccountId) {
        this.chatUser = chatUser;
        this.fromAccountId = fromAccountId;
    }

    public ChatUser getChatUser() {
        return chatUser;
    }

    public void setChatUser(ChatUser chatUser) {
        this.chatUser = chatUser;
    }

    public long getFromAccountId() {
        return fromAccountId;
    }

    public void setFromAccountId(long fromAccountId) {
        this.fromAccountId = fromAccountId;
    }
}
}