precision mediump float;

uniform vec2 u_resolution;
uniform float u_time;

float PI = 3.14159;

void main() {
    vec2 st = gl_FragCoord.xy/u_resolution.xy;
    vec3 color = vec3(sin(st.x*PI-u_time));
    gl_FragColor = vec4(color, 1.0);
}