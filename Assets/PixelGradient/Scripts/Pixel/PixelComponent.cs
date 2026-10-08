using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PixelGradient.Environment
{
    [RequireComponent(typeof(Renderer))]
    public abstract class PixelComponent : MonoBehaviour
    {
        [SerializeField]
        private string pixelId;
        private MaterialPropertyBlock materialBlock;
        private Renderer localRenderer;

        private GradientSync controller;
        public string PixelId => pixelId;

        public MaterialPropertyBlock MaterialBlock
        {
            get
            {
                if (materialBlock == null) materialBlock = new MaterialPropertyBlock();
                return materialBlock;
            }
        }

        #region Unity Methods

        private void Awake()
        {
            controller = FindFirstObjectByType<GradientSync>();
            localRenderer = GetComponent<Renderer>();
            controller.RegisterPixel(this);
        }

        private void OnDestroy()
        {
            Unregister();
        }

        private void OnValidate()
        {
            Debug.Assert(!string.IsNullOrEmpty(PixelId), "The variable 'Pixel Id' can't be empty");
        }

        #endregion     

        #region Public Methods

        public void Unregister()
        {
            controller.UnregisterPixel(this);
        }

        public virtual void UpdatePixelsColor(Texture comparableTexture, Texture startTexture, Texture endTextire, int textureWidth, float blend)
        {
            Debug.LogError("You mUst implement this method");
        }

        public virtual void SetNormalColor()
        {
            Debug.LogError("You mUst implement this method");
        }

        #endregion

        #region Private Methods

        protected void GetMaterialBlock()
        {
            localRenderer.GetPropertyBlock(MaterialBlock);
        }

        protected void SetMaterialBlock()
        {
            localRenderer.SetPropertyBlock(MaterialBlock);
        }

        #endregion
    }

    public static class ShaderProperties
    {
        public static readonly int IsToggled = Shader.PropertyToID("_isToggled");
        public static readonly int Blend = Shader.PropertyToID("_Blend");

        public static readonly int ComparableTexture = Shader.PropertyToID("_ComparableTexture");
        public static readonly int StartTexture = Shader.PropertyToID("_StartTexture");
        public static readonly int EndTexture = Shader.PropertyToID("_EndTexture");

        public static readonly int TextureWidth = Shader.PropertyToID("_TextureWidth");
    }
}