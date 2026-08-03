using System.Collections.Generic;
using UnityEngine;

public class DataLoader
{
    public List<T> LoadJsonData<T>(string resourcePath)
    {
        TextAsset asset = Resources.Load<TextAsset>(resourcePath);

        if (asset == null)
        {
            return null;
        }

        List<T> result = new List<T>();

        foreach (string element in SplitJsonArray(asset.text))
        {
            result.Add(JsonUtility.FromJson<T>(element));
        }

        return result;
    }

    // 이게 맞는걸까 코드가..

    private List<string> SplitJsonArray(string json)
    {
        List<string> elements = new List<string>();

        // 최상위 객체를 못읽지만 List<IngredientAmount> 를 Json 으로 쓰니까 [] 로 감싸져서
        // 수동으로 일단 잘라서 사용하는 방향으로 진행     
        // 제일 외부 [] 만 제거
        int start = json.IndexOf('[') + 1;
        int end = json.LastIndexOf(']');
        string inner = json.Substring(start, end - start);

        // 위 이유로 현재 지금 몇번째 괄호인지 여기서 체크하는걸로
        int depth = 0;
        bool inString = false;
        int elementStart = 0;


        // 여기서 부터 안의 문자를 검사함
        // 목적은 Ingredients 같이 특정 데이터 내부에 [] 로 묶인 데이터들을
        // JsonUtility 로 처리 가능한 형태로 변경하기 위함 + 혹시나 " 나 / 같은 문자를 사용하기 위해
        for (int i = 0; i < inner.Length; i++)
        {
            char c = inner[i];

            // 혹시 우리가 Json 내부에 " 나 / 쓸 일 있을까..? 싶지만 혹시나 해서 추가
            if (c == '"' && (i == 0 || inner[i - 1] != '\\'))
            {
                inString = !inString;
            }
            else if (!inString)
            {
                if (c == '{' || c == '[') depth++;
                else if (c == '}' || c == ']') depth--;
                else if (c == ',' && depth == 0)
                {
                    elements.Add(inner.Substring(elementStart, i - elementStart).Trim());
                    elementStart = i + 1;
                }
            }
        }

        // 루프 돌고나서 여기까지 오면 , 가 없는 즉 마지막 데이터니까 추가

        string last = inner.Substring(elementStart).Trim();
        if (last.Length > 0)
        {
            elements.Add(last);
        }

        return elements;
    }
}
