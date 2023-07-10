using System;
using System.Collections.Generic;


public class FunctionData
{
    public String FunctionName { get; }
    public String[] FunctionArgs { get; }

    public FunctionData(string pFunctionName, String[] pFunctionArgs)
    {
        FunctionName = pFunctionName;
        FunctionArgs = pFunctionArgs;
    }
}


public class FunctionParser
{
    public static FunctionData Parse(String pInput)
    {
        // 함수 이름과 인자를 저장할 변수 선언
        string functionName;
        List<int> arguments = new List<int>();

        // 함수 이름을 추출
        int index = pInput.IndexOf('(');
        functionName = pInput.Substring(0, index);

        // 인자를 추출
        int startIndex = index + 1;
        int endIndex = pInput.IndexOf(')', startIndex);
        string[] argStrings = pInput.Substring(startIndex, endIndex - startIndex).Split(',');

        // 결과 출력
        FunctionData data = new FunctionData(functionName, argStrings);
        return data;
    }
}