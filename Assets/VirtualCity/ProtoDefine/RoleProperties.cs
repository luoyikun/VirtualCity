using ProtoBuf;
namespace ProtoDefine
{
 [ProtoContract]
 public class RoleProperties
{
 [ProtoMember(1)]
private  long? id;

/**
 性别
*/
 [ProtoMember(2)]
private  int? sex;

/**
 头像
*/
 [ProtoMember(3)]
private  string icon;

/**
 模型数据
*/
 [ProtoMember(4)]
private  string modelDate;

 [ProtoMember(5)]
private  string createdate;

 [ProtoMember(6)]
private  string updatedate;

public long? Id
{

get {	 return id ;	}

 set {	id = value;	}

}

public int? Sex
{

get {	 return sex ;	}

 set {	sex = value;	}

}

public string Icon
{

get {	 return icon ;	}

 set {	icon = value;	}

}

public string ModelDate
{

get {	 return modelDate ;	}

 set {	modelDate = value;	}

}

public string Createdate
{

get {	 return createdate ;	}

 set {	createdate = value;	}

}

public string Updatedate
{

get {	 return updatedate ;	}

 set {	updatedate = value;	}

}

}

}

