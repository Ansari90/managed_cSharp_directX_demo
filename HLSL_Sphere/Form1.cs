using System;
using System.Windows.Forms;
//added by narendra
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace HLSL_Sphere
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private Effect effect = null;
        private Mesh mesh = null;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;

        private float angle = 0.0f;
        public Form1()
        {
            InitializeComponent();
        }
        public bool InitializeGraphics()
        {
            // Set our presentation parameters
            PresentParameters presentParams = new PresentParameters();

            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            presentParams.EnableAutoDepthStencil = true;

            bool canDoShaders = true;
            // Does a hardware device support shaders?
            Caps hardware = Manager.GetDeviceCaps(0, DeviceType.Hardware);
            if (hardware.VertexShaderVersion >= new Version(1, 1))
            {
                // Default to software processing
                CreateFlags flags = CreateFlags.SoftwareVertexProcessing;

                // Use hardware if it's available
                if (hardware.DeviceCaps.SupportsHardwareTransformAndLight)
                    flags = CreateFlags.HardwareVertexProcessing;

                // Use pure if it's available
                if (hardware.DeviceCaps.SupportsPureDevice)
                    flags |= CreateFlags.PureDevice;

                // Yes, Create our device
                device = new Device(0, DeviceType.Hardware, this, flags, presentParams);
            }
            else
            {
                // No shader support
                canDoShaders = false;

                // Create a reference device
                device = new Device(0, DeviceType.Reference, this,
                    CreateFlags.SoftwareVertexProcessing, presentParams);
            }

            // Create our effect
            //2005/07/11. TheZBuffer.com. Updated for June 2005 SDK
            //effect = Effect.FromFile(device, @"..\..\simple.fx", null, ShaderFlags.None, null);
            effect = Effect.FromFile(device, @"..\..\simple.fx", null, ShaderFlags.None, null);
            effect.Technique = "TransformDiffuse";

            // Store our project and view matrices
            projMatrix = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                this.Width / this.Height, 1.0f, 100.0f);

            viewMatrix = Matrix.LookAtLH(new Vector3(0, 0, 9.0f), new Vector3(),
                new Vector3(0, 1, 0));

            // Create our sphere
            mesh = Mesh.Sphere(device, 3.0f, 36, 36);

            return canDoShaders;
        }

        private void UpdateWorld()
        {
            angle += 0.05f;

            worldMatrix = Matrix.RotationAxis(new Vector3(angle / ((float)Math.PI * 2.0f),
                angle / ((float)Math.PI * 4.0f), angle / ((float)Math.PI * 6.0f)),
                angle / (float)Math.PI);

            Matrix worldViewProj = worldMatrix * viewMatrix * projMatrix;

            effect.SetValue("WorldViewProj", worldViewProj);
            effect.SetValue("Time", angle);
        }

        public void Render()
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.CornflowerBlue, 1.0f, 0);

            device.BeginScene();

            int numPasses = effect.Begin(0);
            for (int i = 0; i < numPasses; i++)
            {
                effect.BeginPass(i);

                UpdateWorld();
                mesh.DrawSubset(0);
                effect.EndPass();
            }
            effect.End();

            device.EndScene();
            device.Present();
        }

    }
}