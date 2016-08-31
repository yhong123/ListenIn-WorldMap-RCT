Shader "Custom/JellyFish" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TintAmount("Tint amount", Range(0,1)) = 0.5
		_ColorA ("Color", Color) = (1,1,1,1)
		_ColorB ("Color", Color) = (1,1,1,1)
		_Speed ("Wave Speed", Range(0.1, 80)) = 5
		_Frequency("Wave Frequency", Range(0,5)) = 2
		_Amplitude("Wave Amplitude", Range(-1,1)) = 1
		_TransparentColor("Transparent Color level", Range(0, 10))  = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" "RenderType" = "Transparent" "Queue" = "Transparent"}
		
		LOD 300
		//AlphaTest Less 1.5
		//Read back side of the mesh
		Cull Off
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha vertex:vert

		sampler2D _MainTex;
		float4 _ColorA;
		float4 _ColorB;
		float _TintAmount;
		float _Speed;
		float _Frequency;
		float _Amplitude;
		float _OffsetVal;
		float _TransparentColor;

		struct Input {
			float2 uv_MainTex;
			float3 vertColor;
		};
		
		void vert(inout appdata_full v, out Input o)
		{
			
			//Always implement this bit to intialize Input from struct
			UNITY_INITIALIZE_OUTPUT(Input,o);
			float time = _Time * _Speed;
			float waveValueA = sin(time + v.vertex.z * _Frequency) * _Amplitude;
			//Extra wave value B that add more colors in response to amplitude level
			float waveValueB = cos(time + v.vertex.z * _Frequency) * _Amplitude;
			//This increases chance of displacing vertices on mesh surface
			float d = _Time * _Amplitude;
			
			v.vertex.xyz = float3(v.vertex.x + waveValueB , v.vertex.y + waveValueA, v.vertex.z + waveValueB);
			//Diagonal movement of vertices
			v.vertex.y = d * v.vertex.x + waveValueB;
			v.normal = normalize(float3(v.normal.x * waveValueA, v.normal.y, v.normal.z));
			o.vertColor = float3(waveValueA,waveValueB, waveValueA);
		
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Alpha = tex2D (_MainTex, IN.uv_MainTex).rgb * _TransparentColor;
			float3 tintColor = lerp(_ColorA, _ColorB, IN.vertColor).rgb;
			o.Albedo = o.Alpha * (tintColor * _TintAmount);
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
