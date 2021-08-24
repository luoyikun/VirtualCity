using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqSyncShopMessage {
[ProtoMember(1)]
    public PlayerStatus playerStatus;
[ProtoMember(2)]
    public string streetScenceId;
[ProtoMember(3)]
    public long shopId;


    public PlayerStatus getPlayerStatus() {
        return playerStatus;
    }

    public void setPlayerStatus(PlayerStatus playerStatus) {
        this.playerStatus = playerStatus;
    }

    public string getStreetScenceId() {
        return streetScenceId;
    }

    public void setStreetScenceId(string streetScenceId) {
        this.streetScenceId = streetScenceId;
    }

    public long getShopId() {
        return shopId;
    }

    public void setShopId(long shopId) {
        this.shopId = shopId;
    }


}
}