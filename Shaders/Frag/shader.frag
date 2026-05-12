#version 330 core

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light lights[10];
uniform vec3 viewPos;
uniform sampler2D texture0;
uniform sampler2D shadowMap;
uniform sampler2D normalMap;
uniform sampler2D metallicMap;
uniform sampler2D roughnessMap;
uniform sampler2D emissionMap;
uniform samplerCube skybox;
uniform int lightsCount;
uniform int hasNormalMap;
uniform int hasEmission;
uniform int hasSkybox;

in vec2 texCoord;
in vec3 Tangent;
in vec3 Bitangent;
in vec3 Normal;
in vec3 FragPos;
in vec4 FragPosLightSpace;

out vec4 outputColor;

const float PI = 3.14159265359;

float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a = roughness * roughness;
    float a2 = a * a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH * NdotH;
    float nom = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;
    return nom / max(denom, 0.0001);
}

float GeometrySchlickGGX(float NdotV, float roughness)
{
    float a = roughness * roughness;
    float k = (a * a) / 2.0;

    float nom = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return nom / max(denom, 0.0001);
}

float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);

    float a = roughness * roughness;
    float k = (a * a) / 2.0;

    float GGXV = NdotV / (NdotV * (1.0 - k) + k);
    float GGXL = NdotL / (NdotL * (1.0 - k) + k);
    
    return GGXV * GGXL;
}

vec3 FresnelSchlick(float cosTheta, vec3 F0)
{
    cosTheta = clamp(cosTheta, 0.0, 1.0);
    vec3 F90 = vec3(1.0);
    vec3 F = F0 + (F90 - F0) * pow(1.0 - cosTheta, 5.0);
    return min(F, vec3(1.0));
}

float ShadowCalculation(vec4 fragPosLightSpace, vec3 normal, vec3 lightDir)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;
    if(projCoords.z > 1.0 || projCoords.x < 0.0 || projCoords.x > 1.0 || projCoords.y < 0.0 || projCoords.y > 1.0)
        return 0.0;
    float closestDepth = texture(shadowMap, projCoords.xy).r;
    float currentDepth = projCoords.z;
    float bias = max(0.01 * (1.0 - dot(normal, lightDir)), 0.005);
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r;
            shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;
        }
    }
    shadow /= 9.0;
    return shadow;
}

vec3 FresnelSchlickRoughness(float cosTheta, vec3 F0, float roughness)
{
    cosTheta = clamp(cosTheta, 0.0, 1.0);
    vec3 F90 = max(vec3(1.0 - roughness), F0);
    return F0 + (F90 - F0) * pow(1.0 - cosTheta, 5.0);
}

void main()
{
    vec4 textureColor = texture(texture0, texCoord);
    vec3 em = vec3(0.0);
    if (hasEmission == 1)
    {
        em = texture(emissionMap, texCoord).rgb;
    }
    vec3 albedo = textureColor.rgb;

    vec3 norm;
    if (hasNormalMap == 1)
    {
        vec3 N = normalize(Normal);
        vec3 T = normalize(Tangent);
        vec3 B = normalize(Bitangent);

        mat3 TBN = mat3(T, B, N);

        vec3 normalFromMap = texture(normalMap, texCoord).rgb;
        normalFromMap = normalize(normalFromMap * 2.0 - 1.0);
      
        norm = normalize(TBN * normalFromMap);
    }
    else
    {
        norm = normalize(Normal);
    }

    vec3 viewDir = normalize(viewPos - FragPos);

    float metallic = 0.5;
    float roughness = 0.5;
    
    if(textureSize(metallicMap, 0).x > 1)
    {
        metallic = texture(metallicMap, texCoord).r;
    }
    if(textureSize(roughnessMap, 0).x > 1)
    {
        roughness = texture(roughnessMap, texCoord).r;
    }
    
    roughness = clamp(roughness, 0.04, 1.0);
    metallic = clamp(metallic, 0.0, 1.0);
    vec3 F0 = vec3(0.04);
    F0 = mix(F0, albedo, metallic);
    
    vec3 ambient = vec3(0.03) * albedo;
    vec3 Lo = vec3(0.0);
    
    if(lightsCount > 0)
    {
        for(int i = 0; i < lightsCount && i < 10; i++)
        {
            vec3 lightDir = normalize(lights[i].position - FragPos);
            vec3 H = normalize(viewDir + lightDir);
            float distance = length(lights[i].position - FragPos);
            float attenuation = 1.0 / (1.0 + 0.09 * distance + 0.032 * distance * distance);

            vec3 radiance = lights[i].diffuse * attenuation;

            float NDF = DistributionGGX(norm, H, roughness);
            float G = GeometrySmith(norm, viewDir, lightDir, roughness);
            vec3 F = FresnelSchlick(max(dot(H, viewDir), 0.0), F0);

            vec3 numerator = NDF * G * F;
            float NdotV = max(dot(norm, viewDir), 0.0);
            float NdotL = max(dot(norm, lightDir), 0.0);
            float denominator = 4.0 * NdotV * NdotL + 0.0001;

            vec3 specular = numerator / denominator;

            vec3 kS = F;
            vec3 kD = (vec3(1.0) - kS) * (1.0 - metallic);

            float shadow = ShadowCalculation(FragPosLightSpace, norm, lightDir);

            Lo += (kD * albedo / PI + specular) * radiance * NdotL * (1.0 - shadow);
        }
    }
    else
    {
        vec3 lightPos = vec3(5.0, 5.0, 5.0);
        vec3 lightColor = vec3(1.0, 1.0, 1.0);
        vec3 lightDir = normalize(lightPos - FragPos);
        vec3 H = normalize(viewDir + lightDir);
        float distance = length(lightPos - FragPos);
        float attenuation = 1.0 / (1.0 + 0.09 * distance);
        vec3 radiance = lightColor * attenuation;
        float NDF = DistributionGGX(norm, H, roughness);
        float G = GeometrySmith(norm, viewDir, lightDir, roughness);
        vec3 F = FresnelSchlick(max(dot(H, viewDir), 0.0), F0);
        vec3 numerator = NDF * G * F;
        float denominator = 4.0 * max(dot(norm, viewDir), 0.0) * max(dot(norm, lightDir), 0.0) + 0.0001;
        vec3 specular = numerator / denominator;
        vec3 kS = F;
        vec3 kD = (vec3(1.0) - kS) * (1.0 - metallic);
        float NdotL = max(dot(norm, lightDir), 0.0);
        Lo = (kD * albedo / PI + specular) * radiance * NdotL;
    }
    
    if (hasSkybox == 1)
    {
        vec3 I = normalize(FragPos - viewPos);
        vec3 R = reflect(I, norm);
        vec3 skyColor = texture(skybox, R).rgb;
    
        vec3 F = FresnelSchlickRoughness(max(dot(norm, viewDir), 0.0), F0, roughness);

        float reflectionStrength = 0.3;
        Lo += skyColor * F * reflectionStrength;
        ambient = vec3(0.01) * albedo;
    }

    vec3 color = vec3(0.0);
    if(hasEmission == 1)
    {
        color = ambient + Lo + em;
    }
    else
    {
        color =  ambient + Lo;
    }

    color = color / (color + vec3(1.0));
    
    color = pow(color, vec3(1.0/2.2));

    outputColor = vec4(color, textureColor.a);
}