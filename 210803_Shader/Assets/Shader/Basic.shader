Shader "Tunier/Basic" // 쉐이더 소속과 명칭
{
    // 01. 프로퍼티 영역
    // 쉐이더에서 사용할 메인 항목들 선언
    Properties 
    {
        // 프로퍼티명, (인스펙터에 노출될 이름, 타입/범위), 초기값
        _MainTex ("Albedo (RGB)1", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB)2", 2D) = "white" {}
    }

    // 02. 서브쉐이더 영역
    // 프로퍼티를 활용하여 실제 셰이더가 수행될 역할에 대한 정의
    // 내부적으로 CGPROGRAM의 영역을 지니고 있음
    SubShader
    {
        // 알파 채널을 작동시키기 위해서 아래 영역을 수정
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        
        // 03. CGPROGRAM 영역 ~ ENDCG까지
        // CG언어를 활용하여 직접 쉐이더를 작성하는 부분
        // 1, 2번은 유니티의 자체 언어로 작성되어 있으며
        // 3번은 CG(쉐이더 전용 언어)로 작성되어 있음.
        CGPROGRAM
        // 3-1 설정부분
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        sampler2D _MainTex2;

        // 3-2 구조체 부분 - 엔진으로부터 받아와야할 데이터
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

        // 3-3 함수영역
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 d = tex2D (_MainTex2, float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y));
            
            // lerp 함수는 2가지 텍스처를 합칠 때 사용.
            // lerp(이미지 1, 이미지 2, 합성 강도)
            // 합성 강도 0 ~ 1
            // 0에 가까울수록 이미지 1, 1에 가까울수록 이미지 2표현
			//o.Albedo = lerp(c.rgb, d.rgb, 1 - c.a);
            
            // 그레이 스케일(흑백전환)
			//o.Albedo = (c.r + c.g + c.b) / 3;

            o.Emission = c.rgb * d.rgb;
            o.Alpha = c.a * d.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
