using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]


public class Recode {
    /**用户名*/
[ProtoMember(1)]
    public string name;
    /**时间*/
[ProtoMember(2)]
    public string time;
    /**操作类型：0,帮忙,1,收取*/
[ProtoMember(3)]
    public int handleType;
[ProtoMember(4)]
    /**帮忙：获得的钻石,收取：收取金币利润*/
    public long number;
[ProtoMember(5)]
    /**减少时间*/
    public long reduceTime;

    public Recode() {
    }

    public Recode(string name, string time, int handleType, long number, long reduceTime) {
        this.name = name;
        this.time = time;
        this.handleType = handleType;
        this.number = number;
        this.reduceTime = reduceTime;
    }

    public long getReduceTime() {
        return reduceTime;
    }

    public void setReduceTime(long reduceTime) {
        this.reduceTime = reduceTime;
    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
    }

    public string getTime() {
        return time;
    }

    public void setTime(string time) {
        this.time = time;
    }

    public int getHandleType() {
        return handleType;
    }

    public void setHandleType(int handleType) {
        this.handleType = handleType;
    }

    public long getNumber() {
        return number;
    }

    public void setNumber(long number) {
        this.number = number;
    }
}
}