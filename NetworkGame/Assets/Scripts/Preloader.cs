using System;
using System.Collections;
using System.Collections.Generic;
using NetworkModule;
using UnityEngine;
using UnityEngine.UIElements;

public class Preloader : MonoBehaviour
{
    Multicast multicast;
    // Awake is called on object initialization
    void Awake()
    {
        multicast= new Multicast();
        multicast.initDefaultNetwork();
    }

    void Update() {
       if (!Application.isPlaying) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true) {
            multicast.Send();
        }
    }
}
