using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetHousePartMessage {
[ProtoMember(1)]
    public List<HouseParts> housePartsList;

    public RspGetHousePartMessage() {
    }

    public RspGetHousePartMessage(List<HouseParts> housePartsList) {
        this.housePartsList = housePartsList;
    }

    public List<HouseParts> getHousePartsList() {
        return housePartsList;
    }

    public void setHousePartsList(List<HouseParts> housePartsList) {
        this.housePartsList = housePartsList;
    }
}
}