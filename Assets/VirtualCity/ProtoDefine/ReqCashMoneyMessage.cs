using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqCashMoneyMessage {
    //金额
[ProtoMember(1)]
    public float cashMoney;
    //微信：1
    //支付宝：0
[ProtoMember(2)]
    public int payType;
    //账号信息
[ProtoMember(3)]
    public string payAccount;
    //提现密码
[ProtoMember(4)]
    public string password;
[ProtoMember(5)]
    public byte[] content;

    public byte[] getContent() {
        return content;
    }

    public void setContent(byte[] content) {
        this.content = content;
    }

    public float getCashMoney() {
        return cashMoney;
    }

    public void setCashMoney(float cashMoney) {
        this.cashMoney = cashMoney;
    }

    public int getPayType() {
        return payType;
    }

    public void setPayType(int payType) {
        this.payType = payType;
    }

    public string getPayAccount() {
        return payAccount;
    }

    public void setPayAccount(string payAccount) {
        this.payAccount = payAccount;
    }

    public string getPassword() {
        return password;
    }

    public void setPassword(string password) {
        this.password = password;
    }
}
}