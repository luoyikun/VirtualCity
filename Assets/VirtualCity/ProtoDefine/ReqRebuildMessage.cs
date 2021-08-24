using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqRebuildMessage {
[ProtoMember(1)]
    public string buildId;
    /**
     * 0:金币
     * 1:钻石
     */
[ProtoMember(2)]
    public int costDiamond;

    public string getBuildId() {
        return buildId;
    }

    public void setBuildId(string buildId) {
        this.buildId = buildId;
    }

    public int getCostDiamond() {
        return costDiamond;
    }

    public void setCostDiamond(int costDiamond) {
        this.costDiamond = costDiamond;
    }
}
}