using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class ConstGeneration 
{
    public static void Generate(string ns, string Filename, List<string> ConstList)
    {
#if UNITY_EDITOR
        string autofolderPath = Application.dataPath + "/Scripts/Auto";
        string m_Path = Application.dataPath + $"/Scripts/Auto/{ns}";
        DirectoryInfo di;
        di = new DirectoryInfo(autofolderPath);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(m_Path);
        if (di.Exists == false)
            di.Create();
        
        try
        {
            CreateGenericsCode("cs", $"{m_Path}/{Filename}.cs", $"{Filename}_CS.exe", ConstList,ns, Filename);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        AssetDatabase.Refresh();
#endif
    }

    static void CreateGenericsCode(string providerName, string sourceFileName, string assemblyName, List<string> ConstList,string ns,string Filename)
    {
#if UNITY_EDITOR
        using (var stream = new StreamWriter(sourceFileName, append: false))
        {
            var tw = new IndentedTextWriter(stream);
            var codeProvider = CodeDomProvider.CreateProvider(providerName);
            var cu = CreateCode(ConstList,ns, Filename);
            codeProvider.GenerateCodeFromCompileUnit(cu, stream, new CodeGeneratorOptions());
        }
#endif
    }

    static CodeCompileUnit CreateCode(List<string> ConstList,string nsString,string Filename)
    {
#if UNITY_EDITOR
        var cu = new CodeCompileUnit();

        CodeNamespace ns = new CodeNamespace($"Automation.{nsString}");
        cu.Namespaces.Add(ns);

        CodeTypeDeclaration GeneratedClass = new CodeTypeDeclaration();

        GeneratedClass.Name = $"{Filename}";

        foreach (var item in ConstList)
        {
            var prop = new CodeMemberField();
            prop.Name = item.Replace(" ","_");
            prop.Type = new CodeTypeReference(typeof(string));
            prop.Attributes = MemberAttributes.Public | MemberAttributes.Const;
            prop.InitExpression =
            new CodePrimitiveExpression(item);
            GeneratedClass.Members.Add(prop);
        }

        ns.Types.Add(GeneratedClass);
        return cu;
#endif
    }
}
