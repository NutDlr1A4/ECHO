using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixelGradient.Environment
{
    [RequireComponent(typeof(Renderer))]
    public class PixelTextureComponent : PixelComponent
    {
        public override void UpdatePixelsColor(Texture comparableTexture, Texture startTexture, Texture endTextire, int textureWidth, float blend)
        {
            GetMaterialBlock();
            
            MaterialBlock.SetFloat(ShaderProperties.IsToggled, 1f);
            MaterialBlock.SetFloat(ShaderProperties.Blend, blend);

            MaterialBlock.SetTexture(ShaderProperties.ComparableTexture, comparableTexture);
            MaterialBlock.SetTexture(ShaderProperties.StartTexture, startTexture);
            MaterialBlock.SetTexture(ShaderProperties.EndTexture, endTextire);
            MaterialBlock.SetInt(ShaderProperties.TextureWidth, textureWidth);

            SetMaterialBlock();
        }

        public override void SetNormalColor()
        {
            GetMaterialBlock();

            MaterialBlock.SetFloat(ShaderProperties.IsToggled, 0f);

            SetMaterialBlock();
        }

    }
}
