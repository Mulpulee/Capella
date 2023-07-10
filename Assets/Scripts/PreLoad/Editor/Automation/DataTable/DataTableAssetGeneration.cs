using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class DataTableAssetGeneration
{
    public static void Generate(String nameSpace, String fileName, Type value)
    {
#if UNITY_EDITOR
        String autofolderPath = Application.dataPath + "/Scripts/Auto";
        String m_Path = Application.dataPath + $"/Scripts/Auto/DatTableAsset";
        String m2_Path = Application.dataPath + $"/Scripts/Auto/DatTableAsset/{nameSpace}";

        DirectoryInfo di;
        di = new DirectoryInfo(autofolderPath);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(m_Path);
        if (di.Exists == false)
            di.Create();
        di = new DirectoryInfo(m2_Path);
        if (di.Exists == false)
            di.Create();

        try
        {
            CreateGenericsCode("cs", $"{m_Path}/{fileName}.cs", $"{fileName}_CS.exe", value, nameSpace, fileName);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
        AssetDatabase.Refresh();
#endif
    }

    static void CreateGenericsCode(String providerName, String sourceFileName, String assemblyName, Type typeValue, String pNameSpace, String fileName)
    {
#if UNITY_EDITOR
        using (var stream = new StreamWriter(sourceFileName, append: false))
        {
            var tw = new IndentedTextWriter(stream);
            var codeProvider = CodeDomProvider.CreateProvider(providerName);
            var cu = CreateCode(typeValue, pNameSpace, fileName);
            codeProvider.GenerateCodeFromCompileUnit(cu, stream, new CodeGeneratorOptions());
        }
#endif
    }

    static CodeCompileUnit CreateCode(Type typeData, String pNameSpace, String pFilename)
    {
#if UNITY_EDITOR
        CodeCompileUnit codeCompileUnit = new CodeCompileUnit();

        CodeNamespace nameSpace = new CodeNamespace("");
        codeCompileUnit.Namespaces.Add(nameSpace);

        CodeTypeDeclaration generatedClass = new CodeTypeDeclaration();

        var baseType = new CodeTypeReference(typeof(DataTableAsset<>));
       // var baseType = new CodeTypeReference(typeof(ScriptableObject));
        baseType.TypeArguments.Add(typeData);

        generatedClass.BaseTypes.Add(baseType);
        generatedClass.Name = $"{pFilename}";

        nameSpace.Types.Add(generatedClass);
        return codeCompileUnit;
#endif
    }
}
