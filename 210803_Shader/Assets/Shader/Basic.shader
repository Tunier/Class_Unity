Shader "Tunier/Basic" // ���̴� �ҼӰ� ��Ī
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
            // d�� ���� �����ϴ� ������ ���̽��� �Ǵ� c ���� d�� UV�� Ȱ���ϱ� ����.
            fixed4 d = tex2D (_MainTex2,float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y - _Time.y));
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex + d.r);
            
            // lerp �Լ��� 2���� �ؽ�ó�� ��ĥ �� ���.
            // lerp(�̹��� 1, �̹��� 2, �ռ� ����)
            // �ռ� ���� 0 ~ 1
            // 0�� �������� �̹��� 1, 1�� �������� �̹��� 2ǥ��
			//o.Albedo = lerp(c.rgb, d.rgb, 1 - c.a);
            
            // �׷��� ������(�����ȯ)
			//o.Albedo = (c.r + c.g + c.b) / 3;

            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
