using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

public class JavaProtoToCs : EditorWindow
{

    [MenuItem("DoJavaToCs/DoProtoToCs")]
    static void OpenToProtoExe()
    {
        //string fullPath =  Application.dataPath + "/VirtualCity/ProtoDefine/ProtoJavaToCs.exe";  //路径
        //fullPath = fullPath.Replace('/', '\\');

        ////PublicFunc.StartProcess(fullPath,"");

        //Process.Start(fullPath);

        ////System.Diagnostics.Process.Start("ProtoJavaToCs.exe", fullPath);


        DoProtoToCs();
    }

    static void DoProtoToCs()
    {
        string fullPath = "Assets/VirtualCity/ProtoDefine/Java";  //路径
        string toPath = "Assets/VirtualCity/ProtoDefine";
        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".java"))
                {
                    string fileOnlyName = files[i].Name.Replace(".java", "");

                    StringBuilder tempBuilder = new StringBuilder();
                    tempBuilder.Append("using System.Collections.Generic;");
                    tempBuilder.AppendLine();
                    tempBuilder.Append("using ProtoBuf;");
                    tempBuilder.AppendLine();
                    tempBuilder.Append("namespace ProtoDefine {");
                    tempBuilder.AppendLine();
                    tempBuilder.Append("[ProtoContract]");
                    tempBuilder.AppendLine();
                    var strs = File.ReadAllLines(files[i].FullName);
                    int protoIdx = 1;
                    for (int lineIdx = 0; lineIdx < strs.Length; lineIdx++)
                    {
                        if (strs[lineIdx].Contains("import") || strs[lineIdx].Contains("package") || strs[lineIdx].Contains("MessageMeta"))
                        {
                            continue;
                        }

                        if (strs[lineIdx].Contains("Protobuf"))
                        {
                            string sAdd = "[ProtoMember(" + protoIdx + ")]";
                            protoIdx++;

                            tempBuilder.Append(sAdd);
                            tempBuilder.AppendLine();
                            continue;

                        }

                        if (strs[lineIdx].Contains("class"))
                        {
                            //string[] bufStr = strs[lineIdx].Split(' ');
                            string[] bufStr = Regex.Split(strs[lineIdx], "extends", RegexOptions.IgnoreCase);
                            string sClass = "";
                            if (bufStr.Length == 2)
                            {
                                sClass = bufStr[0] + "{";
                            }
                            else {
                                sClass = strs[lineIdx];
                            }
                            //string sClass = bufStr[0] + " " +  bufStr[1] + " "+ bufStr[2] + "{";
                             
                            tempBuilder.Append(sClass);
                            tempBuilder.AppendLine();
                            continue;
                        }

                        if (strs[lineIdx].Contains("@") && strs[lineIdx].Contains("Protobuf") == false)
                        {
                            continue;
                        }
                        strs[lineIdx] = strs[lineIdx].Replace("Long", "long?");
                        strs[lineIdx] = strs[lineIdx].Replace("Double", "double?");
                        strs[lineIdx] = strs[lineIdx].Replace("String", "string");
                        strs[lineIdx] = strs[lineIdx].Replace("Integer", "int?");
                        strs[lineIdx] = strs[lineIdx].Replace("Short", "short?");
                        strs[lineIdx] = strs[lineIdx].Replace(" Map", " Dictionary");
                        strs[lineIdx] = strs[lineIdx].Replace("(Map", "(Dictionary");
                        strs[lineIdx] = strs[lineIdx].Replace("private", "public");
                        strs[lineIdx] = strs[lineIdx].Replace("private", "public");
                        strs[lineIdx] = strs[lineIdx].Replace("volatile", "");


                        tempBuilder.Append(strs[lineIdx]);
                      
                        tempBuilder.AppendLine();
                    }
                    tempBuilder.Append("}");

                    string tempFilePath = string.Format("{0}/{1}.cs", toPath, fileOnlyName);
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                    File.WriteAllText(tempFilePath, tempBuilder.ToString());
                }
            }


        }

        UnityEngine.Debug.Log("ProtoToCs转换完成");
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}
