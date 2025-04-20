using System.IO;
using OpenTK.Graphics.OpenGL;

namespace SamLabs.Shaders.Utilites;

//https://github.com/opentk/LearnOpenTK/tree/master

public class Shader
{
    public int Program { get; private set; }
    private readonly Dictionary<string, int> _uniformLocations;
    
    public Shader(string fragmentPath, string vertPath)
    {
        var fragmentSource = File.ReadAllText(fragmentPath);
        var vertSource = File.ReadAllText(vertPath);

        var vertexShader = CompileShader(vertSource, ShaderType.VertexShader);
        var fragmentShader = CompileShader(fragmentSource, ShaderType.FragmentShader);
        
        Program = CreateAndLinkProgram(vertexShader, fragmentShader);
        
        
        GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        _uniformLocations = new Dictionary<string, int>();

        for (var i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(Program, i, out _, out _);

            var location = GL.GetUniformLocation(Program, key);

            _uniformLocations.Add(key, location);
        }
        
    }

    private int CompileShader(string shaderSource, ShaderType shaderType)
    {
        var shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, shaderSource);
        GL.CompileShader(shader);
        
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
        }
    
        return shader;
    }

    private int CreateAndLinkProgram(int vertexShader, int fragmentShader)
    {
        
        Program = GL.CreateProgram();

        GL.AttachShader(Program, vertexShader);
        GL.AttachShader(Program, fragmentShader);
        
        GL.LinkProgram(Program);
        
        GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({Program})");
        }
        
        GL.DetachShader(Program, vertexShader);
        GL.DetachShader(Program, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);
        
        return Program;
    }
    public void Use()
    {
        if(Program == 0)
            return;
        
        GL.UseProgram(Program);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Program, attribName);
    }
    
    
    
}