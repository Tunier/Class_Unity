Shader "Tunier/Basic" // 쉐이더 소속과 명칭
{
    Properties 
    {
        _MainTex ("Albedo (RGB) 1", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB) 2", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        sampler2D _MainTex2;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // d를 먼저 수행하는 이유는 베이스가 되는 c 에서 d의 UV를 활용하기 위함.
            fixed4 d = tex2D (_MainTex2,float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y));
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex + d.r);
            
            // lerp 함수는 2가지 텍스처를 합칠 때 사용.
            // lerp(이미지 1, 이미지 2, 합성 강도)
            // 합성 강도 0 ~ 1
            // 0에 가까울수록 이미지 1, 1에 가까울수록 이미지 2표현
			//o.Albedo = lerp(c.rgb, d.rgb, 1 - c.a);
            
            // 그레이 스케일(흑백전환)
			//o.Albedo = (c.r + c.g + c.b) / 3;

            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
