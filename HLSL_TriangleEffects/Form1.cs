using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace HLSL_TriangleEffects
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private VertexBuffer vb = null;
        private Effect effect = null;
        private VertexDeclaration decl = null;

        // Our matrices
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
            effect = Effect.FromFile(device, @"..\..\simple.fx", null, ShaderFlags.None, null);
            effect.Technique = "TransformDiffuse";

            // Create our vertex data
            vb = new VertexBuffer(typeof(CustomVertex.PositionOnly), 6, device,
                Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionOnly.Format,
                Pool.Default);

            vb.Created += new EventHandler(this.OnVertexBufferCreate);
            OnVertexBufferCreate(vb, null);

            // Store our project and view matrices
            projMatrix = Matrix.PerspectiveFovLH((float)Math.PI / 4,
                this.Width / this.Height, 1.0f, 100.0f);

            viewMatrix = Matrix.LookAtLH(new Vector3(0, 0, 5.0f), new Vector3(),
                new Vector3(0, 1, 0));

            // Create our vertex declaration
            VertexElement[] elements = new VertexElement[]
                    {
                        new VertexElement(0, 0, DeclarationType.Float3, 
                        DeclarationMethod.Default,
                        DeclarationUsage.Position, 0),
                        VertexElement.VertexDeclarationEnd
                    };

            decl = new VertexDeclaration(device, elements);
            return canDoShaders;
        }

        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;

            CustomVertex.PositionOnly[] verts = new CustomVertex.PositionOnly[6];
            verts[0].Position = new Vector3(0.0f, 1.0f, 1.0f);
            verts[1].Position = new Vector3(-1.0f, -1.0f, 1.0f);
            verts[2].Position = new Vector3(1.0f, -1.0f, 1.0f);

            verts[3].Position = verts[0].Position;
            verts[4].Position = verts[2].Position;
            verts[5].Position = new Vector3(2.0f, 1.0f, 1.0f);

            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void UpdateWorld()
        {
            worldMatrix = Matrix.RotationY(angle);
            angle += 0.05f;

            // Update our effect variables
            Matrix worldViewProj = worldMatrix * viewMatrix * projMatrix;

            effect.SetValue("Time", (float)Math.Sin(angle / 5.0f));
            effect.SetValue("WorldViewProj", worldViewProj);
        }

        public void Render()
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.CornflowerBlue, 1.0f, 0);

            UpdateWorld();
            device.BeginScene();

            device.SetStreamSource(0, vb, 0);
            device.VertexDeclaration = decl;

            // Render our triangle using an effect
            int numPasses = effect.Begin(0);
            for (int i = 0; i < numPasses; i++)
            {
                effect.BeginPass(i);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                effect.EndPass();
            }

            effect.End();

            device.EndScene();

            device.Present();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
