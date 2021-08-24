using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class ReqQueryOrderMessage {
    /**订单状态*/
[ProtoMember(1)]
    public int status;
    /**用户注册时间*/
[ProtoMember(2)]
    public string createTime;
    /**最后一条记录的时间*/
[ProtoMember(3)]
    public string lastDate;

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public string getCreateTime() {
        return createTime;
    }

    public void setCreateTime(string createTime) {
        this.createTime = createTime;
    }

    public string getLastDate() {
        return lastDate;
    }

    public void setLastDate(string lastDate) {
        this.lastDate = lastDate;
    }
}
}