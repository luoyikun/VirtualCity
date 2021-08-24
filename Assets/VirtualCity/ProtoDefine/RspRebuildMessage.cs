using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspRebuildMessage {
[ProtoMember(1)]
    public Devlopments devlopments;
[ProtoMember(2)]
    public int gold;
[ProtoMember(3)]
    public int diamend;
[ProtoMember(4)]
    public int code;
[ProtoMember(5)]
    public string tip;

    public RspRebuildMessage() {
    }

    public RspRebuildMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspRebuildMessage(Devlopments devlopments, int gold, int diamend, int code, string tip) {
        this.devlopments = devlopments;
        this.gold = gold;
        this.diamend = diamend;
        this.code = code;
        this.tip = tip;
    }

    public Devlopments getDevlopments() {
        return devlopments;
    }

    public void setDevlopments(Devlopments devlopments) {
        this.devlopments = devlopments;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiamend() {
        return diamend;
    }

    public void setDiamend(int diamend) {
        this.diamend = diamend;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }
}
}