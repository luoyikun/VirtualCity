using System.Collections.Generic;
using ProtoBuf;
namespace ProtoDefine {
[ProtoContract]



public class ChatUser {
[ProtoMember(1)]
    public long accountId;
[ProtoMember(2)]
    public string userName;
[ProtoMember(3)]
    public long modelId;
[ProtoMember(4)]
    public double income;
    /**坐标*/
[ProtoMember(5)]
    public string serverIp;
[ProtoMember(6)]
    public int sex;
[ProtoMember(7)]
    public int online;

    public ChatUser() {
    }


    public long getAccountId() {
        return accountId;
    }

    public void setAccountId(long accountId) {
        this.accountId = accountId;
    }

    public string getUserName() {
        return userName;
    }

    public void setUserName(string userName) {
        this.userName = userName;
    }

    public long getModleId() {
        return modelId;
    }

    public void setModleId(long modleId) {
        this.modelId = modleId;
    }

    public double getIncome() {
        return income;
    }

    public void setIncome(double income) {
        this.income = income;
    }

    public string getServerIp() {
        return serverIp;
    }

    public void setServerIp(string serverIp) {
        this.serverIp = serverIp;
    }

    public int getSex() {
        return sex;
    }

    public void setSex(int sex) {
        this.sex = sex;
    }

    public int getOnline() {
        return online;
    }

    public void setOnline(int online) {
        this.online = online;
    }

}
}