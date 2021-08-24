using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ReqUpdateUserInfoMessage {
    /**修改信息名称*/
[ProtoMember(1)]
    public List<UserInfoMap> info;
    /**玩家ID*/
[ProtoMember(2)]
    public long? accountId;

    public ReqUpdateUserInfoMessage() {
    }

    public ReqUpdateUserInfoMessage(List<UserInfoMap> info,long? accountId) {
        this.info = info;
        this.accountId = accountId;
    }

    public void setInfo(List<UserInfoMap> info) {
        this.info = info;
    }

    public List<UserInfoMap> getInfo() {
        return info;
    }

    public long? getAccountId() {
        return accountId;
    }

    public void setAccountId(long? accountId) {
        this.accountId = accountId;
    }

}
}