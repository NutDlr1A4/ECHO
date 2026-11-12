using System;
using System.Collections.Generic;
using UnityEngine;

public class EchoNode : MonoBehaviour
{
    public Dictionary<ConnectionPoint, GameObject> node_dict = new();
    public GameObject connection;

    public void DoHack() {
        var hackable = GetComponentInParent<IHackable>();
        hackable?.OnHack();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}