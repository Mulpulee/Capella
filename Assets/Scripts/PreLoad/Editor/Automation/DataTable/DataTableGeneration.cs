using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class DataTableGeneration
{
    public static void Generate(String nameSpace, String fileName, List<String>[] values)
    {
#if UNITY_EDITOR
        String autofolderPath = Application.dataPath + "/Scripts/Auto";
        String m_Path = Application.dataPath + $"/Scripts/Auto/{nameSpace}";

        DirectoryInfo di;
        di = new DirectoryInfo(autofolderPath);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(m_Path);
        if (di.Exists == false)
            di.Create();

        try
        {
            CreateGenericsCode("cs", $"{m_Path}/{fileName}.cs", $"{fileName}_CS.exe", values, nameSpace, fileName);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        AssetDatabase.Refresh();
#endif
    }

    static void CreateGenericsCode(String providerName, String sourceFileName, String assemblyName, List<String>[] values, String pNameSpace, String fileName)
    {
#if UNITY_EDITOR
        using (var stream = new StreamWriter(sourceFileName, append: false))
        {
            var tw = new IndentedTextWriter(stream);
            var codeProvider = CodeDomProvider.CreateProvider(providerName);
            var cu = CreateCode(values, pNameSpace, fileName);
            codeProvider.GenerateCodeFromCompileUnit(cu, stream, new CodeGeneratorOptions());
        }
#endif
    }

    static CodeCompileUnit CreateCode(List<String>[] datas, String pNameSpace, String pFilename)
    {
#if UNITY_EDITOR
        CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

        CodeNamespace nameSpace = new CodeNamespace($"Automation.{pNameSpace}");
        codeCompileUnit.Namespaces.Add(nameSpace);

        CodeTypeDeclaration generatedClass = new CodeTypeDeclaration();

        generatedClass.BaseTypes.Add(new CodeTypeReference(typeof(DataTableRow)));
        generatedClass.Name = $"{pFilename}";

        CodeAttributeDeclaration codeAttrDecl = new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.SerializableAttribute)));
        generatedClass.CustomAttributes.Add(codeAttrDecl);

        for (Int32 i=0,iCount = datas[0].Count;i<iCount;i++)
        {
            Type fieldType;
            
            fieldType = Type.GetType($"System.{datas[1][i]}");
            if(fieldType == null)
                fieldType = Type.GetType($"{datas[1][i]},Assembly-CSharp");

            CodeMemberField field = new CodeMemberField
            {
                Name = datas[0][i],
                Type = new CodeTypeReference(fieldType),
                Attributes = MemberAttributes.Public
            };

            generatedClass.Members.Add(field);
        }

        nameSpace.Types.Add(generatedClass);
        return codeCompileUnit;
#endif
    }
}
