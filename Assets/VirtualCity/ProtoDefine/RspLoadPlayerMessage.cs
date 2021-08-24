using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspLoadPlayerMessage {
[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
    /**
     * 账户对象
     */
[ProtoMember(3)]
    public Account account;


    /**游戏服务器时间*/
[ProtoMember(4)]
    public string time;

    /**
     * 用户其他信息
     */
[ProtoMember(5)]
    public UserOtherData userOtherData;

    /**
     * 用户其他信息
     */
[ProtoMember(6)]
    public string zanRecodeMap;



    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public string getTip() {
        return tip;
    }

    public void setTip(string tip) {
        this.tip = tip;
    }

    public Account getAccount() {
        return account;
    }

    public void setAccount(Account account) {
        this.account = account;
    }

    public string getTime() {
        return time;
    }

    public void setTime(string time) {
        this.time = time;
    }

    public UserOtherData getUserOtherData() {
        return userOtherData;
    }

    public void setUserOtherData(UserOtherData userOtherData) {
        this.userOtherData = userOtherData;
    }

}
}