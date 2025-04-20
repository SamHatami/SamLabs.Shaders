using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System.Windows;

namespace SamLabs.Shaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShaderRenderScene mainScene = new ShaderRenderScene();
        public MainWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 3
            };
            OpenTkControl.Start(settings);
            mainScene.Initialize();
        }

        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            mainScene.Render();
        }
    }
}