using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspUpdateUserInfoMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tips;
[ProtoMember(3)]
    public long? accountId;



[ProtoMember(4)]
    public List<UserInfoMap> userInfoMap;

    public RspUpdateUserInfoMessage() {
    }

    public RspUpdateUserInfoMessage(int code, string tips, long? accountId, List<UserInfoMap> userInfoMap) {
        this.code = code;
        this.tips = tips;
        this.accountId = accountId;
        this.userInfoMap = userInfoMap;
    }

    public List<UserInfoMap> getUserInfoMap() {
        return userInfoMap;
    }

    public void setUserInfoMap(List<UserInfoMap> userInfoMap) {
        this.userInfoMap = userInfoMap;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTips() {
        return tips;
    }

    public void setTips(string tips) {
        this.tips = tips;
    }

    public long? getAccountId() {
        return accountId;
    }

    public void setAccountId(long? accountId) {
        this.accountId = accountId;
    }
}
}