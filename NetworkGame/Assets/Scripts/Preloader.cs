using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using System.Reflection;
using System.IO;
using UnityEngine;

public class Preloader : MonoBehaviour
{

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
      
    }
}
