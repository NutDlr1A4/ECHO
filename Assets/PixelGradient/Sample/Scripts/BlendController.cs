using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using PixelGradient.Environment;

namespace PixelGradient.Sample
{
    [RequireComponent(typeof(GradientSync))]
    public class BlendController : MonoBehaviour
    {
        [SerializeField]
        private GradientSync controller;

        [SerializeField]
        [Min(0.1f)]
        private float deltaTimeDivisor = 3f;

        private float blendCounter;

        private void Update()
        {
            blendCounter += (Time.deltaTime / deltaTimeDivisor);

            if (blendCounter > 1) blendCounter = 0;
            controller.ChangePixelColor(0, 1, blendCounter);
        }
    }
}
