﻿#version 330 core

in vec2 vPosition;
in vec4 vColor;

out vec4 fColor;

void main()
{
    gl_Position = vec4(vPosition, 0, 2);
    fColor = vColor;
}
