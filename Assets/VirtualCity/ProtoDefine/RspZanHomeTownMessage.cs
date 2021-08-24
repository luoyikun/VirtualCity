using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspZanHomeTownMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;
[ProtoMember(3)]
    public long playerId;

    public RspZanHomeTownMessage() {
    }

    public RspZanHomeTownMessage(int code, string tips) {
        this.code = code;
        this.tips = tips;
    }

    public RspZanHomeTownMessage(int code, string tips, long playerId) {
        this.code = code;
        this.tips = tips;
        this.playerId = playerId;
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