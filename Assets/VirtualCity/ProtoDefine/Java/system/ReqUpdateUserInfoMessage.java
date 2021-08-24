package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.server.game.system.entity.UserInfoMap;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.REQ_UPDATEUSERINFO)
public class ReqUpdateUserInfoMessage extends Message {
    /**修改信息名称*/
    @Protobuf(order = 1,fieldType = FieldType.OBJECT)
    private List<UserInfoMap> info;
    /**玩家ID*/
    @Protobuf(order = 2)
    private Long accountId;

    public ReqUpdateUserInfoMessage() {
    }

    public ReqUpdateUserInfoMessage(List<UserInfoMap> info,Long accountId) {
        this.info = info;
        this.accountId = accountId;
    }

    public void setInfo(List<UserInfoMap> info) {
        this.info = info;
    }

    public List<UserInfoMap> getInfo() {
        return info;
    }

    public Long getAccountId() {
        return accountId;
    }

    public void setAccountId(Long accountId) {
        this.accountId = accountId;
    }

}
