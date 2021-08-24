using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspFriendLoginMessage {
[ProtoMember(1)]
    public long accountId;
    /**
     * 0：下线
     * 1：上线
     */
[ProtoMember(2)]
    public int online;

[ProtoMember(3)]

    public long friendId;

    public RspFriendLoginMessage() {
    }

    public RspFriendLoginMessage(long accountId, int online, long friendId) {
        this.accountId = accountId;
        this.online = online;
        this.friendId = friendId;
    }

    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public int getOnline() {
        return online;
    }

    public void setOnline(int online) {
        this.online = online;
    }

    public long getFriendId() {
        return friendId;
    }

    public void setFriendId(long friendId) {
        this.friendId = friendId;
    }
}
}