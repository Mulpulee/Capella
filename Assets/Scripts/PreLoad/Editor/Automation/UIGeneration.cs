using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class UIGeneration
{
    public static void Generate(string Filename, List<UIHelper.UIData> dataList)
    {
#if UNITY_EDITOR
        string autofolderPath = Application.dataPath + "/Scripts/UI";
        string autofolderPath2 = Application.dataPath + $"/Scripts/UI/{Filename}";
        string m_Path = Application.dataPath + $"/Scripts/UI/{Filename}/View";
        DirectoryInfo di;
        di = new DirectoryInfo(autofolderPath);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(autofolderPath2);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(m_Path);
        if (di.Exists == false)
            di.Create();

        try
        {
            CreateGenericsCode("cs", $"{m_Path}/{Filename}View.cs", dataList, Filename);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        AssetDatabase.Refresh();
#endif
    }

    static void CreateGenericsCode(string providerName, string sourceFileName, List<UIHelper.UIData> dataList, string Filename)
    {
#if UNITY_EDITOR
        using (var stream = new StreamWriter(sourceFileName, append: false))
        {
            var tw = new IndentedTextWriter(stream);
            var codeProvider = CodeDomProvider.CreateProvider(providerName);
            var cu = CreateCode(dataList, Filename);
            codeProvider.GenerateCodeFromCompileUnit(cu, stream, new CodeGeneratorOptions());
        }
#endif
    }

    static CodeCompileUnit CreateCode(List<UIHelper.UIData> dataList, string Filename)
    {
#if UNITY_EDITOR
        var cu = new CodeCompileUnit();

        CodeNamespace ns = new CodeNamespace($"");

        cu.Namespaces.Add(ns);

        CodeTypeDeclaration GeneratedClass = new CodeTypeDeclaration();

        GeneratedClass.Name = $"{Filename}View";
        GeneratedClass.BaseTypes.Add(new CodeTypeReference(typeof(ViewBase)));
        GeneratedClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(ViewAttribute))));
        GeneratedClass.IsPartial = true;

        CodeAttributeDeclaration codeAttrDecl = new CodeAttributeDeclaration(new CodeTypeReference(typeof(UIElementAttribute)));


        CodeAttributeArgument codeAttr = new CodeAttributeArgument(new CodePrimitiveExpression("UIElements"));
        var headerAttribute = new CodeAttributeDeclaration(new CodeTypeReference(typeof(HeaderAttribute)), codeAttr);


        foreach (var item in dataList)
        {
            var prop = new CodeMemberField();

            prop.CustomAttributes.Add(codeAttrDecl);
            prop.Name = item.from.transform.name;

            if (item.isList)
            {
                prop.Type = new CodeTypeReference(item.actualType.MakeArrayType());
            }
            else
            {
                prop.Type = new CodeTypeReference(item.actualType);
            }


            prop.Attributes = MemberAttributes.Public;
            GeneratedClass.Members.Add(prop);
        }

        GeneratedClass.Members[0].CustomAttributes.Add(headerAttribute);

        ns.Types.Add(GeneratedClass);
        return cu;
#endif
    }
}