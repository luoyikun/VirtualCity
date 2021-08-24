using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqSearchUserMessage {
    //如果群内搜索没有输入用户名传null即显示所有成员
[ProtoMember(1)]
    public string userName;
    //如果不在群内搜索传null
[ProtoMember(2)]
    public long? groupId;
    /**页码,搜索用户不填*/
[ProtoMember(3)]
    public int pageIndex;

    public ReqSearchUserMessage() {
    }

    public ReqSearchUserMessage(string userName, long? groupId) {
        this.userName = userName;
        this.groupId = groupId;
    }

    public string getUserName() {
        return userName;
    }

    public void setUserName(string userName) {
        this.userName = userName;
    }

    public long? getGroupId() {
        return groupId;
    }

    public void setGroupId(long? groupId) {
        this.groupId = groupId;
    }

    public int getPageIndex() {
        return pageIndex;
    }

    public void setPageIndex(int pageIndex) {
        this.pageIndex = pageIndex;
    }
}
}