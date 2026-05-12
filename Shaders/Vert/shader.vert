#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;
layout (location = 3) in vec3 aTangent;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightSpaceMatrix;

out vec2 texCoord;
out vec3 Normal;
out vec3 FragPos;
out vec4 FragPosLightSpace;
out vec3 Tangent;
out vec3 Bitangent;

void main(void)
{
    texCoord = aTexCoord;
    
    mat3 normalMatrix = mat3(transpose(inverse(model)));
    Normal = normalize(normalMatrix * aNormal);
    Tangent = normalize(normalMatrix * aTangent);
    Bitangent = cross(Normal, Tangent);
    
    vec4 worldPosition = model * vec4(aPosition, 1.0);
    FragPos = worldPosition.xyz;
    FragPosLightSpace = lightSpaceMatrix * worldPosition;
    
    gl_Position = projection * view * worldPosition;
}