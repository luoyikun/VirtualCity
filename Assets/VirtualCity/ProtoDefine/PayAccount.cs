using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class PayAccount {
[ProtoMember(1)]
    public string account;
[ProtoMember(2)]
    public string realName;
[ProtoMember(3)]
    public int payType;
[ProtoMember(4)]
    public int isDefualt;

    public PayAccount() {
    }

    public PayAccount(string account, string realName, int payType, int isDefualt) {
        this.account = account;
        this.realName = realName;
        this.payType = payType;
        this.isDefualt = isDefualt;
    }

    public string getAccount() {
        return account;
    }

    public void setAccount(string account) {
        this.account = account;
    }

    public string getRealName() {
        return realName;
    }

    public void setRealName(string realName) {
        this.realName = realName;
    }

    public int getPayType() {
        return payType;
    }

    public void setPayType(int payType) {
        this.payType = payType;
    }
}
}