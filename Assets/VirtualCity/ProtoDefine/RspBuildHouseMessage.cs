using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspBuildHouseMessage {
[ProtoMember(1)]
    public House house;
[ProtoMember(2)]
    public int gold;
[ProtoMember(3)]
    public int diament;
[ProtoMember(4)]
    public int code;
[ProtoMember(5)]
    public string tip;

    public RspBuildHouseMessage() {
    }

    public RspBuildHouseMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public RspBuildHouseMessage(House house, int gold, int diament, int code, string tip) {
        this.house = house;
        this.gold = gold;
        this.diament = diament;
        this.code = code;
        this.tip = tip;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
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