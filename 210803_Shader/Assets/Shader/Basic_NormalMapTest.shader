Shader "Tunier/Basic_Normal" // 쉐이더 소속과 명칭
{
    Properties 
    {
        _MainTex ("Albedo (RGB) 1", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {} // 노말맵용

        _Metallic("Metalic", Range(0,1)) = 0
		_Smoothness("Smoothness", Range(0,1)) = 0
        _Occlusion("Occlusion", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Standard
        sampler2D _MainTex;
        sampler2D _BumpMap;
        
        sampler2D _Occlusion; // 환경차폐(환경광, Ambient Occlusion)
        float _Metallic;
        float _Smoothness;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            // 오클루전의 경우 독립된 UV를 활용하는 것이 아니라
            // 오클루전을 적용하고자 하는 텍스처의 uv를 활용
            o.Occlusion = tex2D(_Occlusion, IN.uv_MainTex);
            // 노말맵이지만 일반 텍스처와 차이가 없음.
            // 노말맵은 일반 텍스쳐와 포맷이 다름.
            // 파일의 포맷이 노말맵의 품질을 유지하기위한 AG 형식(압축)임.
            // 그래서 노말맵의 처리는 조금 다름(압축 해제를 해줘야됨).
            // 노말맵의 강도를 조정하기 위하여 바로 대입하는 것이 아니라
            // n 변수에다가 저장후 조작하도록함.
            fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            // 곱해진 2(상수값)을 조정하면 노말맵의 강도가 변경됨.
            o.Normal = float3(n.x * 20, n.y * 20, n.z);

            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
