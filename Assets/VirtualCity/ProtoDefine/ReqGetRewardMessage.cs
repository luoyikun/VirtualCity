using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetRewardMessage {

    /**
     * 建筑Id
     */
[ProtoMember(1)]
    public string devId;
    /**
     * 玩家Id
     */
[ProtoMember(2)]
    public long? playerId;
    /**
     * 玩家昵称
     */
[ProtoMember(3)]
    public string playerName;

    public ReqGetRewardMessage() {
    }

    public ReqGetRewardMessage(string devId, long? playerId, string playerName) {
        this.devId = devId;
        this.playerId = playerId;
        this.playerName = playerName;
    }

    public string getDevId() {
        return devId;
    }

    public void setDevId(string devId) {
        this.devId = devId;
    }

    public long? getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long? playerId) {
        this.playerId = playerId;
    }

    public string getPlayerName() {
        return playerName;
    }

    public void setPlayerName(string playerName) {
        this.playerName = playerName;
    }
}
}