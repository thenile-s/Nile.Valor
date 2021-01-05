#version 330 core

layout (points) in;
layout (triangle_strip, max_vertices = 4) out;

uniform mat4 transform;

in vec4 color[];

in vec4 pos[];

in vec4 tex[];

in vec2 origin[];

in vec2 scale[];

in float rot[];

out GS_OUT
{
	vec2 tex;
	vec4 color;
} gs_out;

void main()
{
	vec4 posBuffer;
	vec2 originScale;
	vec2 sizeScale;
	vec2 blPos;
	vec2 originPos;
	vec2 rotBuffer;
	float sine;
	float cosine;
	
	for(int i = 0; i < pos.length(); i++)
	{
		sine = sin(radians(rot[i]));
		cosine = cos(radians(rot[i]));
		originScale = origin[i] * scale[i];
		sizeScale = pos[i].zw * scale[i];
		blPos = pos[i].xy - originScale;
		originPos = pos[i].xy;

		posBuffer = vec4(blPos, 0, 1);
		posBuffer.y += sizeScale.y;
		
		rotBuffer = posBuffer.xy - originPos;

		posBuffer.x = rotBuffer.x * cosine - rotBuffer.y * sine + originPos.x;
		posBuffer.y = rotBuffer.x * sine + rotBuffer.y * cosine + originPos.y;

		gl_Position = posBuffer * transform;
		gs_out.tex = vec2(tex[i].x, tex[i].y);
		gs_out.color = color[i];
		EmitVertex();

		posBuffer = vec4(blPos + sizeScale, 0, 1);

		rotBuffer = posBuffer.xy - originPos;

		posBuffer.x = rotBuffer.x * cosine - rotBuffer.y * sine + originPos.x;
		posBuffer.y = rotBuffer.x * sine + rotBuffer.y * cosine + originPos.y;

		gl_Position = posBuffer * transform;
		gs_out.tex = vec2(tex[i].x + tex[i].z, tex[i].y);
		gs_out.color = color[i];
		EmitVertex();

		posBuffer = vec4(blPos, 0, 1);

		rotBuffer = posBuffer.xy - originPos;

		posBuffer.x = rotBuffer.x * cosine - rotBuffer.y * sine + originPos.x;
		posBuffer.y = rotBuffer.x * sine + rotBuffer.y * cosine + originPos.y;

		gl_Position = posBuffer * transform;
		gs_out.tex = vec2(tex[i].x, tex[i].y + tex[i].w);
		gs_out.color = color[i];
		EmitVertex();

		posBuffer = vec4(blPos, 0, 1);
		posBuffer.x += sizeScale.x;

		rotBuffer = posBuffer.xy - originPos;

		posBuffer.x = rotBuffer.x * cosine - rotBuffer.y * sine + originPos.x;
		posBuffer.y = rotBuffer.x * sine + rotBuffer.y * cosine + originPos.y;

		gl_Position = posBuffer * transform;
		gs_out.tex = vec2(tex[i].x + tex[i].z, tex[i].y + tex[i].w);
		gs_out.color = color[i];
		EmitVertex();

		EndPrimitive();
	}
}