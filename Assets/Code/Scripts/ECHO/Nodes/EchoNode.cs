using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EchoNode : MonoBehaviour
{
    public Dictionary<ConnectionPoint, GameObject> node_dict = new();
    public GameObject connection;

    [SerializeField] private GameObject hack_effect_prefab;

    public void DoHack() {
        var hackable = GetComponentInParent<IHackable>();

        if (hackable != null) {
            if (hackable.OnHack()) {
                GameObject effect = Instantiate(hack_effect_prefab);
                effect.transform.position = gameObject.transform.position;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}