using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TC;
using Unity.VisualScripting.YamlDotNet.RepresentationModel;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;

/// <summary>
/// YamlÉtÉ@ÉCÉãì«Ç›èëÇ´
/// </summary>
public class YamlReader : MonoBehaviour
{
    void Awake()
    {
        GM.Add<string, Dictionary<string, object>>("ReadYaml", Read);
        GM.Add<string, Dictionary<string, object>>("WriteYaml", Write);
    }

    Dictionary<string, object> Read(string path)
    {
        if(!File.Exists(path)) return null;

        var input = File.ReadAllText(path);
        var deserializer = new DeserializerBuilder().Build();
        var result = deserializer.Deserialize<Dictionary<string, object>>(input);
        
        return result;
    }

    void Write(string path, Dictionary<string, object> data)
    {
        var serializer = new SerializerBuilder().Build();
        File.WriteAllText(path, serializer.Serialize(data));
    }
}
