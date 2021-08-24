using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRunInShopMessage {

[ProtoMember(1)]
    public PlayerStatus playerStatus;
[ProtoMember(2)]
    public string scenceId;
[ProtoMember(3)]
    public long shopId;

    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }

    public string getScenceId() {
        return scenceId;
    }

    public void setScenceId(string scenceId) {
        this.scenceId = scenceId;
    }

    public long getShopId() {
        return shopId;
    }

    public void setShopId(long shopId) {
        this.shopId = shopId;
    }
}
}