using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspGetRewardMessage {
[ProtoMember(1)]
    public Devlopments devlopment;
[ProtoMember(2)]
    public int gold;
[ProtoMember(3)]
    public int code;
[ProtoMember(4)]
    public string tips;

    public RspGetRewardMessage() {
    }

    public RspGetRewardMessage(Devlopments devlopment, int gold, int code, string tips) {
        this.devlopment = devlopment;
        this.gold = gold;
        this.code = code;
        this.tips = tips;
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

    public Devlopments getDevlopment() {
        return devlopment;
    }

    public void setDevlopment(Devlopments devlopment) {
        this.devlopment = devlopment;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }
}
}