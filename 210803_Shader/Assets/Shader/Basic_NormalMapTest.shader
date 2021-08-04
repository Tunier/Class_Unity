Shader "Tunier/Basic_Normal" // ���̴� �ҼӰ� ��Ī
{
    Properties 
    {
        _MainTex ("Albedo (RGB) 1", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {} // �븻�ʿ�

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
        
        sampler2D _Occlusion; // ȯ������(ȯ�汤, Ambient Occlusion)
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
            // ��Ŭ������ ��� ������ UV�� Ȱ���ϴ� ���� �ƴ϶�
            // ��Ŭ������ �����ϰ��� �ϴ� �ؽ�ó�� uv�� Ȱ��
            o.Occlusion = tex2D(_Occlusion, IN.uv_MainTex);
            // �븻�������� �Ϲ� �ؽ�ó�� ���̰� ����.
            // �븻���� �Ϲ� �ؽ��Ŀ� ������ �ٸ�.
            // ������ ������ �븻���� ǰ���� �����ϱ����� AG ����(����)��.
            // �׷��� �븻���� ó���� ���� �ٸ�(���� ������ ����ߵ�).
            // �븻���� ������ �����ϱ� ���Ͽ� �ٷ� �����ϴ� ���� �ƴ϶�
            // n �������ٰ� ������ �����ϵ�����.
            fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            // ������ 2(�����)�� �����ϸ� �븻���� ������ �����.
            o.Normal = float3(n.x * 20, n.y * 20, n.z);

            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
