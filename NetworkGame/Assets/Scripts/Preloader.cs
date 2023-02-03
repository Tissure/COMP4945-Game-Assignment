using System;
using System.Collections;
using System.Collections.Generic;
using NetworkModule;
using System.Reflection;
using System.IO;
using UnityEngine;

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
    void Awake()
    {
        multicast = GetComponent<WebSocketNetwork>();
        multicast.Setup();
        multicast.enabled = true;
        multicast.execute();
        //GameManager.getInstance.initDefaultGameState(); 
        GameObject.Find("GameManager").GetComponent<GameManager>().initDefaultGameState();
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
