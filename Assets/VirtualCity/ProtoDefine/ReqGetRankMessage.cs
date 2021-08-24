using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqGetRankMessage {
    //排行榜类型
[ProtoMember(1)]
    public int rankType;
    //排行对象（全服或好友）
[ProtoMember(2)]
    public int relationType;
    //好友ID列表 如果是查询全服数据传null
[ProtoMember(3)]
    public List<long?> friendIds;

    public List<long?> getFriendIds() {
        return friendIds;
    }

    public void setFriendIds(List<long?> friendIds) {
        this.friendIds = friendIds;
    }

    public int getRelationType() {
        return relationType;
    }

    public void setRelationType(int relationType) {
        this.relationType = relationType;
    }

    public int getRankType() {
        return rankType;
    }

    public void setRankType(int rankType) {
        this.rankType = rankType;
    }
}
}