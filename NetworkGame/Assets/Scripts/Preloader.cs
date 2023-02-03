using System;
using System.Collections;
using System.Collections.Generic;
using NetworkModule;
using System.Reflection;
using System.IO;
using UnityEngine;
using static NetworkModule.PacketHandler;

public class Preloader : MonoBehaviour
{
    public WebSocketNetwork multicast;

    public CustomNetworkModule createNetworkModule(string module)
    {

        string workingDirectory = System.IO.Directory.GetCurrentDirectory().Split("Preloader.cs")[0];
        string modulePath = Path.Combine(workingDirectory, module + ".dll");
        string className = "NetworkModule." + module;

        Assembly assembly = AppDomain.CurrentDomain.Load(Assembly.LoadFrom(modulePath).GetName());
        Type assemblyType = assembly.GetType(className);

        return (CustomNetworkModule)Activator.CreateInstance(assemblyType);

    }

    // Awake is called on object initialization
    void Start()
    {
        multicast = GetComponent<WebSocketNetwork>();
        multicast.Setup();
        multicast.enabled = true;
        multicast.execute();
        //GameManager.getInstance.initDefaultGameState(); 

        string[] args = System.Environment.GetCommandLineArgs(); 

        foreach(string arg in args)
        {
            switch (arg)
            {
                case "-StartServer":
                    //start new server
                    GameObject.Find("GameManager").GetComponent<GameManager>().initDefaultGameState();
                    break;
                case "-JoinServer":
                    GameObject.Find("GameManager").GetComponent<GameManager>().sendJoinServer();
                    break;
                default:
                    break;
            }
        }

     
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            //multicast.Send();

        }
    }
}
