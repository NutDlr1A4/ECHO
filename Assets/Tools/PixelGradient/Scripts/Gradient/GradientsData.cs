using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PixelGradient.Environment
{
    [CreateAssetMenu(fileName = "GradientsData", menuName = "PixelGradient/GradientsData")]
    public class GradientsData : ScriptableObject
    {
        [SerializeField]
        [Min(0)]
        private int pixelStatesAmount;

        [SerializeField]
        private List<GradientTuple> gradients;

        #region Public Gets/Setts

        public List<string> GradientTypes
        {
            get
            {
                return gradients.Select(gradient => gradient.id).ToList();
            }
        }

        #endregion

        #region Public Methods

        public PixelTextureData GetData(string pixelType)
        {
            var gradientTuple = gradients.Find(gradient => gradient.id == pixelType);

            return gradientTuple.pixelTextureData;
        }

        #endregion
    }

    [Serializable]
    public struct GradientTuple
    {
        public string id;
        public PixelTextureData pixelTextureData;
    }
}

