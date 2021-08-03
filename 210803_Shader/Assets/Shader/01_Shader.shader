Shader "Tunier/01_Shader" // ���̴� �ҼӰ� ��Ī
{
    // 01. ������Ƽ ����
    // ���̴����� ����� ���� �׸�� ����
    Properties 
    {
        // ������Ƽ��, (�ν����Ϳ� ����� �̸�, Ÿ��/����), �ʱⰪ
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        // ������� ������Ƽ �߰�
        _Red("Red", Range(0,1)) = 0
        _Green("Green", Range(0, 1)) = 0
        _Blue("Blue", Range(0, 1)) = 0
    }

    // 02. ���꽦�̴� ����
    // ������Ƽ�� Ȱ���Ͽ� ���� ���̴��� ����� ���ҿ� ���� ����
    // ���������� CGPROGRAM�� ������ ���ϰ� ����
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        // 03. CGPROGRAM ���� ~ ENDCG����
        // CG�� Ȱ���Ͽ� ���� ���̴��� �ۼ��ϴ� �κ�
        // 1, 2���� ����Ƽ�� ��ü ���� �ۼ��Ǿ� ������
        // 3���� CG(���̴� ���� ���)�� �ۼ��Ǿ� ����.
        CGPROGRAM
        // 3-1 �����κ�
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        // 3-2 ����ü �κ� - �������κ��� �޾ƿ;��� ������
        struct Input
        {
            float2 uv_MainTex;
        };

        float _Red;
        float _Green;
        float _Blue;

        // 3-3 �Լ�����
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color

            // �ؽ�ó ����(���/����/���� ��) * �÷� ����
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            
            // �����񸵿� ���ؼ� RGB(1, 0, 0) => GBR(0, 0, 1) �� �����.
            // �������� �Ķ������� �����.
            // Albedo �� ��Ͽ� ������ ����.
            // Emission �� ���� ���� ���� ���� �״��.
            o.Albedo = float3(_Red, _Green, _Blue);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
