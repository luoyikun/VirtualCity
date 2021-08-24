using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspBuyHousePartMessage {
[ProtoMember(1)]
    public int gold;
[ProtoMember(2)]
    public int diament;
[ProtoMember(3)]
    public int code;
[ProtoMember(4)]
    public string tip;

    public RspBuyHousePartMessage() {
    }

    public RspBuyHousePartMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspBuyHousePartMessage(int gold, int diament, int code, string tip) {
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
    }

    public int getGold() {
        return gold;
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public int getDiament() {
        return diament;
    }

    public void setDiament(int diament) {
        this.diament = diament;
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