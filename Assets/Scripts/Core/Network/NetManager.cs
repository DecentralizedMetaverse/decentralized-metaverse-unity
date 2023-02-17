using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using YamlDotNet.RepresentationModel;
using System.IO;
using System;
using TC;

[RequireComponent(typeof(CustomNetworkManager))]
public class NetManager : MonoBehaviour
{    
    CustomNetworkManager network;
    string path = "";
    List<string> ips = new List<string>();
    int port = 7777;
    int maxPlayer = 100;
    bool serverMode = false;
    bool vrMode = false;
    
    void Awake()
    {
        network = GetComponent<CustomNetworkManager>();

        path = Application.dataPath + "/../config.yaml";
        ReadConfig();

        foreach(string ip in ips)
        {
            print($"ip: {ip}");
        }
        network.networkAddress = ips[0];
    }       

    void Start()
    {
        if (serverMode) network.StartHost();
        else network.StartClient();
    }    

    void ReadConfig()
    {
        if (!File.Exists(path)) { WriteConfig(); return; }
        using (var reader = new StreamReader(path))
        {
            var yaml = new YamlStream();
            yaml.Load(reader);
            var rootNode = yaml.Documents[0].RootNode;
            try
            {
                var ips = (YamlSequenceNode)rootNode["ip"];

                foreach (var ip in ips.Children)
                {
                    this.ips.Add((string)ip);
                }
                port = int.Parse(rootNode["port"].ToString());
                maxPlayer = int.Parse(rootNode["max-users"].ToString());
                serverMode = bool.Parse(rootNode["server-mode"].ToString());
                vrMode = bool.Parse(rootNode["vr-mode"].ToString());
            }
            catch(Exception e)
            {
                this.ips.Add("localhost");
                GM.Msg("message", e.Message);
                serverMode = true;
            }
        }
    }

    void WriteConfig()
    {
        string text = "ip:\n  - 133.37.59.90\nport: 7777\nmax-users: 100\nserver-mode: false\nvr-mode: false";
        File.WriteAllText(path, text);
    }
}

[System.Serializable]
public struct SendData : NetworkMessage
{
    public string data;
}
