package com.kingston.jforgame.server.game.system.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.system.SystemDataPool;
import com.kingston.jforgame.server.game.system.entity.UserInfoMap;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;

@MessageMeta(module = Modules.SYSTEM,cmd = SystemDataPool.RSP_UPDATEUSERINFO)
public class RspUpdateUserInfoMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tips;
    @Protobuf(order = 3)
    private Long accountId;



    @Protobuf(order = 4,fieldType = FieldType.OBJECT)
    private List<UserInfoMap> userInfoMap;

    public RspUpdateUserInfoMessage() {
    }

    public RspUpdateUserInfoMessage(int code, String tips, Long accountId, List<UserInfoMap> userInfoMap) {
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

    public String getTips() {
        return tips;
    }

    public void setTips(String tips) {
        this.tips = tips;
    }

    public Long getAccountId() {
        return accountId;
    }

    public void setAccountId(Long accountId) {
        this.accountId = accountId;
    }
}
