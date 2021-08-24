using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspBuildDevlopmentMessage {
[ProtoMember(1)]
    public Devlopments devlopment;
[ProtoMember(2)]
    public int gold;
[ProtoMember(3)]
    public int diament;
[ProtoMember(4)]
    public int code;
[ProtoMember(5)]
    public string tip;


    public RspBuildDevlopmentMessage() {
    }

    public RspBuildDevlopmentMessage(Devlopments devlopment, int gold, int diament, int code, string tip) {
        this.devlopment = devlopment;
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
    }

    public RspBuildDevlopmentMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
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