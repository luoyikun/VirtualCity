using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetOneDevInfoMessage {
    /**玩家id*/
[ProtoMember(1)]
    public long? friendId;
    /**建筑id*/
[ProtoMember(2)]
    public string devId;

    public ReqGetOneDevInfoMessage() {
    }

    public ReqGetOneDevInfoMessage(long? friendId, string devId) {
        this.friendId = friendId;
        this.devId = devId;
    }

    public long? getFriendId() {
        return friendId;
    }

    public void setFriendId(long? friendId) {
        this.friendId = friendId;
    }

    public string getDevId() {
        return devId;
    }

    public void setDevId(string devId) {
        this.devId = devId;
    }
}
}