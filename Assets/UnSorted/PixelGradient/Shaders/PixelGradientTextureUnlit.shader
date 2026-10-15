Shader "PixelGradient/Texture Unlit"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		[MaterialToggle] _isToggled("Active conversion", Float) = 0

		_Color ("Tint", Color) = (1,1,1,1)		
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _isToggled;
			
			half _Blend;

			sampler2D _ComparableTexture;
			sampler2D _StartTexture;
			sampler2D _EndTexture;

			int _TextureWidth;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;						

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

				#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
				#endif

				return color;
			}

			fixed4 InputTexture (sampler2D tex, float2 uv)
			{
				return tex2D (tex, uv);
			}

			float2 SearchIdealUV(fixed4 mainColor)
			{				
				float2 texcoord = float2(0, 0);

				for (int x = 0; x < _TextureWidth; x++) 
				{
					float2 localPosition = float2( (x + 0.5) / _TextureWidth, 0.5);
					fixed4 comparableTexture = InputTexture (_ComparableTexture, localPosition);

					fixed4 delta = abs(mainColor - comparableTexture);

					if (length(delta) < 0.001) 
					{						
						texcoord = localPosition;
					}
				}

				return texcoord;
			}

			fixed4 MapColor (fixed4 mainColor) 
			{
				fixed4 newColor = mainColor;

				float2 texcoord = SearchIdealUV(mainColor);
	
				if (texcoord.y)
				{										
					fixed4 startTexture = InputTexture (_StartTexture, texcoord);
					fixed4 endTexture = InputTexture (_EndTexture, texcoord);

					newColor = lerp(startTexture,  endTexture, _Blend);
				}

				return newColor;
			}

			fixed4 frag(v2f IN) : SV_Target
			{                
				fixed4 c = SampleSpriteTexture (IN.texcoord);

				if(_isToggled){
					fixed4 outColor = MapColor(c);
					c = outColor;
				}  

				c *= IN.color;
				c.rgb *= c.a;				
				return c;
			}
			ENDCG
		}
	}
}
