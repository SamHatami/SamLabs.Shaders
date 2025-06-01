precision mediump float;

uniform vec2 u_resolution;

//https://thebookofshaders.com/edit.php#10/ikeda-simple-grid.frag

float scale = 400.0;
float l_thickness = 0.01;

float grid(vec2 st, float res, float t){
    vec2 grid = fract(st * res);
    return 1.-(step(t,grid.x) * step(t,grid.y));    
}

void main() {
    vec2 st = gl_FragCoord.xy/u_resolution.xy;
    vec3 color = vec3(0.0);

    vec2 grid_scale = st*scale; // scale

    color += vec3(0.5725, 0.5725, 0.5725)*grid(grid_scale,0.04, l_thickness);

    gl_FragColor = vec4(color,1.0);
}