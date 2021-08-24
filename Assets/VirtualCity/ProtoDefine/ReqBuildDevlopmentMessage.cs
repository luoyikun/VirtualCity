using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqBuildDevlopmentMessage {
[ProtoMember(1)]
    public long? devModelId;
[ProtoMember(2)]
    public string LandCode;
    /**
     * 0:金币
     * 1:钻石
     */
[ProtoMember(3)]
    public int costDiamond;

    public ReqBuildDevlopmentMessage() {
    }

    public ReqBuildDevlopmentMessage(long? devModelId, string landCode) {
        this.devModelId = devModelId;
        LandCode = landCode;
    }

    public long? getDevModelId() {
        return devModelId;
    }

    public void setDevModelId(long? devModelId) {
        this.devModelId = devModelId;
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