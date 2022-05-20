#version 330 core

uniform vec3 LightColor;
uniform vec3 ObjectColor;
uniform vec3 CameraPosition;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragmentPosition;

struct Material
{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float shininess;
};

uniform Material material;

struct Light
{
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

uniform Light light;

vec3 getAmbient()
{
	return material.ambient * light.ambient;
}

vec3 getDiffuse(vec3 normalizedNormal, vec3 lightDirection)
{
	float diff = max(dot(normalizedNormal, lightDirection), 0.0);
	return material.diffuse * diff * light.diffuse;
}

vec3 getSpecular(vec3 normalizedNormal, vec3 lightDirection)
{
	vec3 viewDirection = normalize(CameraPosition - FragmentPosition);
	vec3 reflected = reflect(-lightDirection, normalizedNormal);
	float spec = pow(max(dot(viewDirection, reflected), 0.0), material.shininess);
	return spec * material.specular * light.specular;
}

void main()
{

	vec3 ambient = getAmbient();

	vec3 normalizedNormal = normalize(Normal);
	vec3 lightDirection = normalize(light.position - FragmentPosition);

	vec3 diffuse = getDiffuse(normalizedNormal, lightDirection);
	
	vec3 specular = getSpecular(normalizedNormal, lightDirection);

	vec3 result = (ambient + diffuse + specular) * ObjectColor;
	FragColor = vec4(result, 1.0f);
}