using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspGetRankMessage {
[ProtoMember(1)]
    public List<RankInfo> list;

    public RspGetRankMessage() {
    }

    public RspGetRankMessage(List<RankInfo> list) {
        this.list = list;
    }

    public List<RankInfo> getList() {
        return list;
    }

    public void setList(List<RankInfo> list) {
        this.list = list;
    }
}
}