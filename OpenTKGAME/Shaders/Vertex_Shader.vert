#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragmentPosition;

void main()
{
	FragmentPosition = vec3(model * vec4(aPosition, 1.0));
	Normal = aNormal * transpose(inverse(mat3(model)));
	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}