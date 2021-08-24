using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspZanHouseMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;
[ProtoMember(3)]
    public long playerId;
[ProtoMember(4)]
    public string houseId;

    public RspZanHouseMessage(int code, string tips, long playerId, string houseId) {
        this.code = code;
        this.tips = tips;
        this.playerId = playerId;
        this.houseId = houseId;
    }

    public RspZanHouseMessage(int code, string tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspZanHouseMessage() {
    }

    public string getHouseId() {
        return houseId;
    }

    public void setHouseId(string houseId) {
        this.houseId = houseId;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
        this.tips = tips;
    }

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }
}
}