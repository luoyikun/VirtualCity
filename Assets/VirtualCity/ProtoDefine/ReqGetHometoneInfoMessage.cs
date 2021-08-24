using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetHometoneInfoMessage {
    /**玩家id*/
[ProtoMember(1)]
    public long? friendId;

    public ReqGetHometoneInfoMessage() {
    }

    public ReqGetHometoneInfoMessage(long? friendId) {
        this.friendId = friendId;
    }

    public long? getFriendId() {
        return friendId;
    }

    public void setFriendId(long? friendId) {
        this.friendId = friendId;
    }
}
}