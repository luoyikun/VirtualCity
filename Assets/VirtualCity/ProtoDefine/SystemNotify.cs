using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class SystemNotify
{
 [ProtoMember(1)]
private  long? id;

/**
 用户Id
*/
 [ProtoMember(2)]
private  long? accountId;

/**
 通知类型:'0'系统消息，'1'，加好友消息，'2',加群消息
*/
 [ProtoMember(3)]
private  int? type;

/**
 通知来源：0指系统消息
*/
 [ProtoMember(4)]
private  long? notifyFrom;

/**
 是否已经处理
*/
 [ProtoMember(5)]
private  int? hasHandle;

/**
 处理结果 0“不同意”，“1”同意
*/
 [ProtoMember(6)]
private  string handleRes;

/**
 通知标题
*/
 [ProtoMember(7)]
private  string title;

/**
 通知内容
*/
 [ProtoMember(8)]
private  string content;

 [ProtoMember(9)]
private  string createtime;

 [ProtoMember(10)]
private  string updatetime;

public long? Id
{

get {	 return id ;	}

 set {	id = value;	}

}

public long? AccountId
{

get {	 return accountId ;	}

 set {	accountId = value;	}

}

public int? Type
{

get {	 return type ;	}

 set {	type = value;	}

}

public long? NotifyFrom
{

get {	 return notifyFrom ;	}

 set {	notifyFrom = value;	}

}

public int? HasHandle
{

get {	 return hasHandle ;	}

 set {	hasHandle = value;	}

}

public string HandleRes
{

get {	 return handleRes ;	}

 set {	handleRes = value;	}

}

public string Title
{

get {	 return title ;	}

 set {	title = value;	}

}

public string Content
{

get {	 return content ;	}

 set {	content = value;	}

}

public string Createtime
{

get {	 return createtime ;	}

 set {	createtime = value;	}

}

public string Updatetime
{

get {	 return updatetime ;	}

 set {	updatetime = value;	}

}

}

}

