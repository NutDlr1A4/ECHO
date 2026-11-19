using System;
using UnityEngine;

public class LightInteraction : MonoBehaviour, IHackable
{
    public void OnHack() {
        GameObject my_light = gameObject.transform.Find("Light_2D").gameObject;
        my_light.SetActive(!my_light.activeSelf);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
