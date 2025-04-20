using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SamLabs.Shaders;

public class ShaderRenderScene
{
    private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    public int Program { get; set; }
    public int VAO { get; set; }
    public int VBO { get; set; }

    struct Vertex
    {
        public Vector2 Position;
        public Color4 Color;

        public Vertex(Vector2 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }

    private static readonly Vertex[] vertices =
    {
        new Vertex((0.0f, 0.5f), Color4.Red),
        new Vertex((0.58f, -0.5f), Color4.Green),
        new Vertex((-0.58f, -0.5f), Color4.Blue),
    };

    private static readonly string VertexShaderSource =
        @"#version 330 core

        in vec2 vPosition;
        in vec4 vColor;

        out vec4 fColor;

        void main()
        {
            gl_Position = vec4(vPosition, 0, 1);
            fColor = vColor;
        }
        ";

    private static readonly string FragmentShaderSource =
        @"#version 330 core

        in vec4 fColor;

        out vec4 oColor;

        void main()
        {
            oColor = fColor;
        }
        ";


    public void Initialize()
    {
        
        //Read vertex shader text -> compile it
        //Read fragment shader text -> compile it
        //Create program -> attach the shaders to the program -> link the program
        //Detach and delete the shader from the program
        
        //Create the program
        Program = GL.CreateProgram();

        //read in and compile the vertex shader (const string from above)
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, VertexShaderSource);
        GL.CompileShader(vertexShader);
        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out var vertexCode);

        if (vertexCode == 0)
        {
            string log = GL.GetShaderInfoLog(vertexShader);
            Trace.WriteLine($"Vertex shader compile error: {log}");
        }

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, FragmentShaderSource);
        GL.CompileShader(fragmentShader);
        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out var fragmentCode);

        if (fragmentCode == 0)
        {
            string log = GL.GetShaderInfoLog(fragmentShader);
            Trace.WriteLine($"Fragment shader compile error {log}");
        }
        
        GL.AttachShader(Program, vertexShader);
        GL.AttachShader(Program, fragmentShader);
        GL.LinkProgram(Program);
        GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out var success);
        if (success == 0)
        {
            string log = GL.GetProgramInfoLog(Program);
            Trace.WriteLine($"Program link error: {log}");
        }
        

        GL.DetachShader(Program, vertexShader);
        GL.DetachShader(Program, fragmentShader);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        int positionLocation = GL.GetAttribLocation(Program, "vPosition");
        int colorLocation = GL.GetAttribLocation(Program, "vColor");

        VAO = GL.GenVertexArray();
        GL.BindVertexArray(VAO);

        VBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Unsafe.SizeOf<Vertex>(), vertices,
            BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(), 0);

        GL.EnableVertexAttribArray(colorLocation);
        GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, Unsafe.SizeOf<Vertex>(),
            Unsafe.SizeOf<Vector2>());
    }
    
    public void Render(float alpha = 1.0f) {
        var hue = (float) _stopwatch.Elapsed.TotalSeconds * 0.15f % 1;
        var c = Color4.FromHsv(new Vector4(alpha * hue, alpha * 0.75f, alpha * 0.75f, alpha));
        GL.ClearColor(c);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.UseProgram(Program);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}