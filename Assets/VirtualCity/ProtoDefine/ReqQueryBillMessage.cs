using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqQueryBillMessage {
    /**类型*/
[ProtoMember(1)]
    public int billType;
    /**最后一条记录的时间*/
[ProtoMember(2)]
    public string lastDate;

    public int getBillType() {
        return billType;
    }

    public void setBillType(int billType) {
        this.billType = billType;
    }

    public string getLastDate() {
        return lastDate;
    }

    public void setLastDate(string lastDate) {
        this.lastDate = lastDate;
    }

}
}