using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using PixelGradient.Environment;

namespace PixelGradient.Sample
{
    [RequireComponent(typeof(GradientSync))]
    public class SingleController : MonoBehaviour
    {
        [SerializeField]
        private GradientSync controller;

        private void Start()
        {
            controller.ChangePixelColor(1);
        }
    }
}
