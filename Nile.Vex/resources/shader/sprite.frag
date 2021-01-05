#version 330 core

uniform sampler2D texture0;

in GS_OUT
{
	vec2 tex;

	vec4 color;
} gs_out;

out vec4 fragColor;

void main()
{
	fragColor = texture(texture0, gs_out.tex) * gs_out.color;
}