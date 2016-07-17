#ifdef GL_ES
    precision highp float;
#endif
varying vec3 viewpos;
varying vec3 normal;
varying vec2 uv;
varying vec3 modelpos;
uniform vec3 albedo;
uniform float shininess;
uniform float specfactor;
uniform vec3 speccolor;
uniform vec3 ambientcolor;
uniform float ambientMix;
uniform sampler2D texture;
uniform float texmix;
uniform vec2 minMaxHeight;

void main()
{
	//NormalizedHeight liest Pixel aus einzeiliger Textur.
	float normalizedHeight = (modelpos.y - minMaxHeight.x)/(minMaxHeight.y - minMaxHeight.x);
	vec3 resultingAlbedo = (1.0-texmix) * albedo + texmix * vec3(texture2D(texture, vec2(normalizedHeight, normalizedHeight)));

	//Diffuse Calc
	vec3 nnormal = normalize(normal);
	vec3 lightdir = vec3(0, 0, -1);
    float intensityDiff = dot(nnormal, lightdir);

	//Modified Settings for Waterdisplay
	float waterLevel = 0.22;
	if(normalizedHeight < waterLevel) {
		intensityDiff = intensityDiff + 0.5;
	}

	// Specular
    float intensitySpec = 0.0;
	if (intensityDiff > 0.0)
	{
		vec3 viewdir = -viewpos;
		vec3 h = normalize(viewdir+lightdir);
		intensitySpec = specfactor * pow(max(0.0, dot(h, nnormal)), shininess);
	}

    gl_FragColor = vec4(ambientcolor * ambientMix + (intensityDiff + 1.0) * resultingAlbedo + intensitySpec * speccolor, 1);
}
