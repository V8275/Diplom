#version 330 core
out vec4 outputColor;
  
uniform vec3 objectColor;
uniform vec3 lightColor;
in vec2 texCoord;

uniform vec3 bottomColor = vec3(0.0, 0.0, 1.0);    // Синий снизу
uniform vec3 topColor = vec3(1.0, 0.0, 0.0);       // Красный сверху

void main()
{
    float gradient = texCoord.y;
    vec3 color = mix(bottomColor, topColor, gradient);
    outputColor = vec4(lightColor * color, 1.0);
}