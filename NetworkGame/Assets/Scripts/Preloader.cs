using System;
using System.Collections;
using System.Collections.Generic;
using NetworkModule;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Preloader : MonoBehaviour
{
    Multicast multicast;

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
        multicast = GetComponent<Multicast>();
        multicast.initDefaultNetwork();
        multicast.enabled = true;
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
