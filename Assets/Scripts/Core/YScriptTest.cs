using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class YScriptTest : MonoBehaviour
{
    [SerializeField] string data = "";
    static List<string> data4 = new List<string>();
    static HashSet<string> split2 = new HashSet<string>()
    {
        "==",
        "<=",
        ">=",
        "++",
        "--",
        "+=",
        "-=",
        "&&",
        "||",
        "//",
        "/*",
        "*/",
    };
    static HashSet<string> split1 = new HashSet<string>()
    {
        "\"",
        "=",
        "!",
        "?",
        ":",
        ")",
        "(",
        "#",
        "<",
        ">",
        "+",
        //"-",
        "*",
        "/",
        "%",
        "&",
        "|",
    };   

    [ContextMenu("Exe script")]
    void Test()
    {
        GetToken(data);
    }
    public static string[][] GetToken(string data)
    {
        data = data.Replace("\r", string.Empty);
        data = data.Replace("\t", string.Empty);
        var data2 = data.Split('\n');
        var str = "";
        int st1 = 0;
        int st2 = 0;
        string[][] data3 = new string[data2.Length][];
        
        for (int j = 0; j < data2.Length; j++)
        {
            //行ごとにループ
            int i = 0;
            st1 = 0;

            while(i < data2[j].Length)
            {
                //1文字ずつループ

                //文字列を抽出
                if(data2[j][i] == '"')
                {
                    do i++;
                    while (data2[j][i] != '"' && i < data2[j].Length);

                    st1++;
                    st2 = i;
                    AddText(data2[j].Substring(st1, st2 - st1));
                    i++;
                    st1 = i;
                    continue;
                }

                if (i + 1 < data2[j].Length)
                {
                    //2文字での検索
                    str = data2[j].Substring(i, 2);
                    if (split2.Contains(str))
                    {
                        st2 = i;
                        AddText(data2[j].Substring(st1, st2 - st1));
                        AddText(str);
                        i += 2;
                        st1 = i;
                        continue;
                    }
                }

                str = data2[j][i].ToString();
                if (str == " " || split1.Contains(str))
                {
                    //1文字での検索
                    st2 = i;
                    AddText(data2[j].Substring(st1, st2 - st1));
                    AddText(str);
                    i++;
                    st1 = i;
                    continue;
                }

                if (i == data2[j].Length-1)
                {
                    //最後の文字列を抽出
                    st2 = i+1;
                    AddText(data2[j].Substring(st1, st2 - st1));
                    break;
                }
                i++;
            } 
            data3[j] = data4.ToArray();
            data4.Clear();
        }

#if UNITY_EDITOR
        //ShowAll(data3);
#endif
        return data3;
    }

    static void AddText(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        data4.Add(text);
    }

#if UNITY_EDITOR
    /// <summary>
    /// 配列の中身を表示
    /// </summary>
    /// <param name="data3"></param>
    static void ShowAll(string[][] data3)
    {
        var str = "";
        foreach (var dt in data3)
        {
            str = "";
            foreach (var dt2 in dt)
                str += dt2 + ",";
            print(str);
        }
    }
#endif
}
