#version 330 core
out vec4 outputColor;
  
uniform vec3 objectColor;
uniform vec3 lightColor;

void main()
{
    outputColor = vec4(lightColor * objectColor, 1.0);
}