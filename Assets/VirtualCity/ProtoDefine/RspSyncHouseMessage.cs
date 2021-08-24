using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class RspSyncHouseMessage {
[ProtoMember(1)]
    public House house;
[ProtoMember(2)]
    public int code;
[ProtoMember(3)]
    public string tip;

    public RspSyncHouseMessage(int code, string tip) {
        this.code = code;
        this.tip = tip;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
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

    public RspSyncHouseMessage() {
    }

    public RspSyncHouseMessage(House house, int code, string tip) {
        this.house = house;
        this.code = code;
        this.tip = tip;
    }
}
}