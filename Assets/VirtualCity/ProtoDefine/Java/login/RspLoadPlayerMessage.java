package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.FieldType;
import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.entity.engineer.*;
import com.kingston.jforgame.server.game.entity.master.Account;
import com.kingston.jforgame.server.game.entity.user.SystemNotify;
import com.kingston.jforgame.server.game.entity.user.UserOtherData;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.server.game.sociality.entity.ProxyUser;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

import java.util.List;
import java.util.Map;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.RSP_LOAD_PLAYER)
public class RspLoadPlayerMessage extends Message {
    @Protobuf(order = 1)
    private int code;
    @Protobuf(order = 2)
    private String tip;
    /**
     * 账户对象
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 3)
    private Account account;


    /**游戏服务器时间*/
    @Protobuf(order = 4)
    private String time;

    /**
     * 用户其他信息
     */
    @Protobuf(fieldType = FieldType.OBJECT,order = 5)
    private UserOtherData userOtherData;

    /**
     * 用户其他信息
     */
    @Protobuf(order = 6)
    private String zanRecodeMap;



    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getTip() {
        return tip;
    }

    public void setTip(String tip) {
        this.tip = tip;
    }

    public Account getAccount() {
        return account;
    }

    public void setAccount(Account account) {
        this.account = account;
    }

    public String getTime() {
        return time;
    }

    public void setTime(String time) {
        this.time = time;
    }

    public UserOtherData getUserOtherData() {
        return userOtherData;
    }

    public void setUserOtherData(UserOtherData userOtherData) {
        this.userOtherData = userOtherData;
    }

}
