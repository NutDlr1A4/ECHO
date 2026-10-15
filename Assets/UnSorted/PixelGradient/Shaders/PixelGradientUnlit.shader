Shader "PixelGradient/Unlit"
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

			int _ColorsLength;
			half _Blend;

			fixed4 _ComparableColors[100];
			fixed4 _StartColors[100];
			fixed4 _EndColors[100];            					

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

			fixed4 MapColor (fixed4 mainColor) 
			{
				for (int i = 0; i < _ColorsLength; i++) 
				{
					fixed4 colorComparable = _ComparableColors[i];

					half3 delta = abs(mainColor.rgb - colorComparable.rgb);					

					if (length(delta) < 0.001) 
					{
						fixed4 colorStart = _StartColors[i];
						fixed4 colorEnd = _EndColors[i];

						fixed4 newColor;

						newColor.rgb = lerp(colorStart.rgb,  colorEnd.rgb, _Blend);
						newColor.a = lerp(colorStart.a, colorEnd.a, _Blend);
						
						return newColor;
					}
				}
				return mainColor;
			}

			fixed4 frag(v2f IN) : SV_Target
			{                
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				if(_isToggled){
					fixed4 outColor = MapColor(c);
					c.rgb = outColor.rgb;
				}                
				
				c.rgb *= c.a;				
				return c;
			}
			ENDCG
		}
	}
}
