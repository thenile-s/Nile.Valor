#version 330 core

in vec4 ipos;

in vec4 itex;

in vec4 icolor;

in vec2 iorigin;

in vec2 iscale;

in float irot;

out vec4 pos;

out vec4 tex;

out vec4 color;

out vec2 origin;

out vec2 scale;

out float rot;

void main()
{
	pos = ipos;
	tex = itex;
	color = icolor;
	origin = iorigin;
	scale = iscale;
	rot = irot;
}
