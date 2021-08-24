using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqSyncHouseMessage {
[ProtoMember(1)]
    public long? friendId;
[ProtoMember(2)]
    public string houseId;
[ProtoMember(3)]
    public PlayerStatus playerStatus;

    public ReqSyncHouseMessage() {
    }

    public ReqSyncHouseMessage(long? friendId, string houseId, PlayerStatus playerStatus) {
        this.friendId = friendId;
        this.houseId = houseId;
        this.playerStatus = playerStatus;
    }

    public long? getFriendId() {
        return friendId;
    }

    public void setFriendId(long? friendId) {
        this.friendId = friendId;
    }

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }
}
}