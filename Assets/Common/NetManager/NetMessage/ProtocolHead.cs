namespace SGF.Network.Core
{
    //包头信息
    public class ProtocolHead
    {
        public int packetLength  = 0; //整个包的长度：长度字节4个  + modelid字节2个 +cmd字节2个 +内容长度
        public short moduleId = 0; //和cmd组成一条协议的id
        public short cmd = 0;

        public ProtocolHead Deserialize(NetBuffer buffer)
        {
            ProtocolHead head = this;
            head.packetLength = buffer.ReadInt();
            head.moduleId = buffer.ReadShort();
            head.cmd = buffer.ReadShort();

            return head;
        }

        public NetBuffer Serialize(NetBuffer buffer)
        {
            buffer.WriteInt(packetLength);
            buffer.WriteShort(moduleId);
            
            buffer.WriteShort(cmd);
            return buffer;
        }

    }
}