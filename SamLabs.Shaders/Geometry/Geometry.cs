using OpenTK.Mathematics;

namespace SamLabs.Shaders.Geometry;

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

public static class Geometry
{
    private static readonly Vertex[] Triangle =
    {
        new Vertex((0.0f, 0.5f), Color4.Red),
        new Vertex((0.58f, -0.5f), Color4.Green),
        new Vertex((-0.58f, -0.5f), Color4.Blue),
    };
    
    
    private static readonly Vertex[] Quad =
    {
        new Vertex((0.0f, 0.5f), Color4.Red),
        new Vertex((0.0f, -0.5f), Color4.Green),
        new Vertex((-0.5f, -0.0f), Color4.Blue),
        new Vertex((-0.58f, -0.5f), Color4.Yellow),
    };
}