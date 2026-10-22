using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixelGradient.Environment
{
    [CreateAssetMenu(fileName = "PixelTextureData", menuName = "PixelGradient/PixelTextureData")]
    [Serializable]
    public class PixelTextureData : ScriptableObject
    {
        public Texture comparableTexture;
        public Texture[] textureStates;

        public Texture GetTexture(int state)
        {
            return textureStates[state];
        }

        public int GetTextureWidth()
        {
            return comparableTexture.width;
        }
    }
}
