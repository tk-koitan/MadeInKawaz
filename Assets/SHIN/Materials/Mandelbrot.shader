Shader "Custom/Mandelbrot"
{
    Properties{
        _MaxIteration("MaxIteration", Range(1, 2048)) = 512
        _Threshold("Threshold", Range(1, 100)) = 2
        _PosX("PosX",Range(-2,2)) = -0.685
        _PosY("PosY",Range(-2,2)) = -0.3
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Threshold;
            int _MaxIteration;
            float _PosX;
            float _PosY;

            float2 cpow(float2 v)
            {
                float a = 3.14;
                if (v.x != 0){
                    a = atan2(v.y, v.x) * 2;
                    }
                return float2(cos(a), sin(a)) * pow(length(v), 2);
            }

            half mandelbrot(half2 c)
            {
                half2 z = half2(0, 0);
                for (int i = 0; i < _MaxIteration; i++)
                {
                    z = cpow(z);
                    z += c;
                    if (length(z) > _Threshold){
                    return (float)i / _MaxIteration;
                    }
                }
                return 1.0;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float scale = 2.1+pow(sin(_Time.x),5)*2;
                scale = pow(10,scale)-1;
                i.uv.x = i.uv.x + _PosX * scale;
                i.uv.y = i.uv.y + _PosY * scale;
                fixed col = mandelbrot((i.uv - 0.5)/scale);
                fixed col_r = 0.5+sin(col*6.28)/2;
                fixed col_g = 0.5+sin(col*6.28+2.09)/2;
                fixed col_b = 0.5+sin(col*6.28+4.19)/2;
                if (col == 1){
                    col_r = 0;
                    col_g = 0;
                    col_b = 0;
                }
                return fixed4(col_r,col_g,col_b,0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
