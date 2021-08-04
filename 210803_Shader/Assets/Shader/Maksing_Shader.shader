Shader "Tunier/Maksing_Shader" // 쉐이더 소속과 명칭
{
    Properties 
    {
        _MainTex ("Albedo (RGB) Base", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB) 2", 2D) = "white" {}
        _MainTex3 ("Albedo (RGB) 3", 2D) = "white" {}
        _MainTex4 ("Albedo (RGB) 4", 2D) = "white" {}
        _MainTex5 ("Albedo (RGB) 5", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _MainTex3;
        sampler2D _MainTex4;
        sampler2D _MainTex5;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 maskC = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 baseC = tex2D(_MainTex2, IN.uv_MainTex);
            fixed4 c3 = tex2D(_MainTex3, IN.uv_MainTex);
            fixed4 c4 = tex2D(_MainTex4, IN.uv_MainTex);
            fixed4 c5 = tex2D(_MainTex5, IN.uv_MainTex);

            o.Albedo = c3.rgb * maskC.r +
                       c4.rgb * maskC.g +
                       c5.rgb * maskC.b +
                       baseC * (1 - (maskC.r + maskC.g + maskC.b)); // RGB채널을 제외한 나머지 영역
        }
        ENDCG
    }
    FallBack "Diffuse"
}
