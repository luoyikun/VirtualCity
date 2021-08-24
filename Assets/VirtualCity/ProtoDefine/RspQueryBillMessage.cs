using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class RspQueryBillMessage {

[ProtoMember(1)]
    public int code;
[ProtoMember(2)]
    public string tip;
    /**账单对象*/
[ProtoMember(3)]
    public List<AccountBill> bills;

    public RspQueryBillMessage() {
    }

    public RspQueryBillMessage(int code, string tip, List<AccountBill> bills) {
        this.code = code;
        this.tip = tip;
        this.bills = bills;
    }

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

    public List<AccountBill> getBills() {
        return bills;
    }

    public void setBills(List<AccountBill> bills) {
        this.bills = bills;
    }
}

}