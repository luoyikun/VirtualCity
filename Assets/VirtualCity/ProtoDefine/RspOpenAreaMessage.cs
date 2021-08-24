using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspOpenAreaMessage {
[ProtoMember(1)]
    public Land land;
[ProtoMember(2)]
    public int gold;
[ProtoMember(3)]
    public int diamend;
[ProtoMember(4)]
    public int code;
[ProtoMember(5)]
    public string tip;

    public RspOpenAreaMessage() {
    }

    public RspOpenAreaMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspOpenAreaMessage(Land land, int gold, int diamend, int code, string tip) {
        this.land = land;
        this.gold = gold;
        this.diamend = diamend;
        this.code = code;
        this.tip = tip;
    }

    public Land getLand() {
        return land;
    }

    public void setLand(Land land) {
        this.land = land;
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