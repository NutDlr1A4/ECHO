using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixelGradient.Environment
{
    public class GradientSync : MonoBehaviour
    {
        [SerializeField]
        private GradientsData gradientsData;
        private List<PixelComponent> pixelComponents = new List<PixelComponent>();
        public List<string> GradientTypes => (gradientsData != null) ? gradientsData.GradientTypes : new List<string>();   

        #region Public Methods

        /// <summary>
        /// Make all Pixel Components use the described color with the selected index
        /// </summary>
        /// <param name="newState"> state index defined inside the GradientsData </param>
        public void ChangePixelColor(int newState)
        {
            ChangeComponentsPixelColor(newState, 1);
        }

        /// <summary>
        /// Make all Pixel Components use the described color with the selected index and blend
        /// </summary>
        /// <param name="currentState"> initial state color </param>
        /// <param name="newState"> next state color </param>
        /// <param name="blend"> a blend between [0,1] to selected a new color </param>
        public void ChangePixelColor(int currentState, int nextState, float blend)
        {
            ChangeComponentsPixelColor(currentState, nextState, blend);
        }

        internal void RegisterPixel(PixelComponent component)
        {
            pixelComponents.Add(component);
        }

        internal void UnregisterPixel(PixelComponent component)
        {
            pixelComponents.Remove(component);
        }

        #endregion

        #region Private Methods        

        private void ChangeComponentsPixelColor(int newState, float blend)
        {
            ChangeComponentsPixelColor(newState, newState, blend);
        }

        private void ChangeComponentsPixelColor(int currentState, int nextState, float blend)
        {
            for (int i = 0; i < pixelComponents.Count; i++)
            {
                var component = pixelComponents[i];

                var pixelTextureData = gradientsData.GetData(component.PixelId);

                if (pixelTextureData)
                {                    
                    UpdatePixelsColor(currentState, nextState, pixelTextureData, i, blend);                    
                }
                else
                {
                    Debug.Assert(pixelTextureData != null, $"We can't find to <b>{component.name}</b> a reference for <b>{component.PixelId}</b> inside the <b>GradientData</b>");
                    component.SetNormalColor();
                }
            }
        }

        private void UpdatePixelsColor(int currentState, int nextState, PixelTextureData data, int index, float blend)
        {
            var pixelsComparable = data.comparableTexture;

            var pixelsStart = data.GetTexture(currentState);
            var pixelsEnd = data.GetTexture(nextState);

            var textureSize = data.GetTextureWidth();            
            pixelComponents[index].UpdatePixelsColor(pixelsComparable, pixelsStart, pixelsEnd, textureSize, blend);            
        }

        #endregion
    }
}