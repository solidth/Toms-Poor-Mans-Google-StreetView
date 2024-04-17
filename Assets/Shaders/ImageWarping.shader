// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/ImageWarping"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        // Invert the normals for rendering inside of the sphere.
        Cull Front

        // Add stencil buffer to only render curren sphere
        Stencil{
			Ref 1 // Set a reference value for the stencil buffer
            Comp always // Always pass stencil test
            Pass replace // Replace stencil buffer value
		}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // include file that contains UnityObjectToWorldNormal helper function
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD0;
            };

            sampler2D _MainTex;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }            
            fixed4 frag(v2f i) : SV_Target {
                // Calculate direction vectors for each vertex to the camera
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                
                // Calculate UV coordinates per fragment
                float pi = 3.14159265359;
                float dx = viewDir.x;
                float dy = viewDir.y;
                float dz = viewDir.z;
                float u = 0.5 + atan2(dx, dz) / (2 * pi);
                float v = 0.5 + asin(dy) / pi;
                // Mirror Y-coordinate
                v = 1.0 - v;
                // Sample texture
                fixed4 col = tex2D(_MainTex, float2(u, v));
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}