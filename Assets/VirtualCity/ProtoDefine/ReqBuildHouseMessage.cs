using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqBuildHouseMessage {
[ProtoMember(1)]
    public long? houseModelId;
[ProtoMember(2)]
    public string LandCode;
    /**
     * 0:金币
     * 1:钻石
     */
[ProtoMember(3)]
    public int costDiamond;

    public ReqBuildHouseMessage() {
    }

    public ReqBuildHouseMessage(long? houseModelId, string landCode) {
        this.houseModelId = houseModelId;
        LandCode = landCode;
    }

    public long? getHouseModelId() {
        return houseModelId;
    }

    public void setHouseModelId(long? houseModelId) {
        this.houseModelId = houseModelId;
    }

    public string getLandCode() {
        return LandCode;
    }

    public void setLandCode(string landCode) {
        LandCode = landCode;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
}