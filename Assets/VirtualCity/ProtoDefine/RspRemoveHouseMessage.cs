using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspRemoveHouseMessage {

[ProtoMember(1)]
    public House house;

    public RspRemoveHouseMessage() {
    }

    public RspRemoveHouseMessage(House house) {
        this.house = house;
    }

    public House getHouse() {
        return house;
    }

    public void setHouse(House house) {
        this.house = house;
    }

}
}