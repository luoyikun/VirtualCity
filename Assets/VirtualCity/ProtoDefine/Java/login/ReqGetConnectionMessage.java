package com.kingston.jforgame.server.game.login.message;

import com.baidu.bjf.remoting.protobuf.annotation.Protobuf;
import com.kingston.jforgame.server.game.Modules;
import com.kingston.jforgame.server.game.login.LoginDataPool;
import com.kingston.jforgame.socket.annotation.MessageMeta;
import com.kingston.jforgame.socket.message.Message;

@MessageMeta(module = Modules.LOGIN,cmd = LoginDataPool.REQ_CONNECTION)
public class ReqGetConnectionMessage extends Message {
    /**
     * 用户对象
     */
    @Protobuf(order = 1)
    private long playerId;

    /**
     * 去哪：
     * game 家园
     * scence 商业街
     */
    @Protobuf(order = 2)
    private String runInfo;

    public long getPlayerId() {
        return playerId;
    }

    public void setPlayerId(long playerId) {
        this.playerId = playerId;
    }

    public String getRunInfo() {
        return runInfo;
    }

    public void setRunInfo(String runInfo) {
        this.runInfo = runInfo;
    }
}
