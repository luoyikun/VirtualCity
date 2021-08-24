using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqGetConnectionMessage {
    /**
     * 用户对象
     */
[ProtoMember(1)]
    public long playerId;

    /**
     * 去哪：
     * game 家园
     * scence 商业街
     */
[ProtoMember(2)]
    public string runInfo;

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }

    public string getRunInfo() {
        return runInfo;
    }

    public void setRunInfo(string runInfo) {
        this.runInfo = runInfo;
    }
}
}