using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqBuyHousePartMessage {
[ProtoMember(1)]
    public long housePartsId;
[ProtoMember(2)]
    public int number;
    /**
     * 0:金币
     * 1:钻石
     */
[ProtoMember(3)]
    public int costDiamond;

    public long getHousePartsId() {
        return housePartsId;
    }

    public void setHousePartsId(long housePartsId) {
        this.housePartsId = housePartsId;
    }

    public int getNumber() {
        return number;
    }

    public void setNumber(int number) {
        this.number = number;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
}