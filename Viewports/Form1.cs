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

namespace Viewports
{
    public partial class Form1 : Form
    {
        Point lastMousePosition;
        Point currentMousePosition;
        bool mousing;
        int spinX;
        int spinY;
        float angle;

        Material teapotMaterial;
        Mesh teapotMesh, mesh;
        Device device;

        public Form1()
        {
            InitializeComponent();

            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
        }

        protected void OnMouseDown(Object sender, MouseEventArgs e)
        {
            lastMousePosition = currentMousePosition = PointToScreen(new Point(e.X, e.Y));
            mousing = true;
        }

        protected void OnMouseMove(Object sender, MouseEventArgs e)
        {
            currentMousePosition = PointToScreen(new Point(e.X, e.Y));
            if (mousing)
            {
                spinX -= currentMousePosition.X - lastMousePosition.X;
                spinY -= currentMousePosition.Y - lastMousePosition.Y;
            }
            lastMousePosition = currentMousePosition;
        }

        protected void OnMouseUp(Object sender, MouseEventArgs e1)
        {
            mousing = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void InitializeGraphics()
        {
            //detect hardware capability
            CreateFlags createFlags;
            if (Manager.GetDeviceCaps(
            Manager.Adapters.Default.Adapter, DeviceType.Hardware).DeviceCaps.SupportsHardwareTransformAndLight)
            {
                createFlags = CreateFlags.HardwareVertexProcessing;
            }
            else
            {
                createFlags = CreateFlags.SoftwareVertexProcessing;
            }
            //Set the presentation parameters
            PresentParameters presentParameters = new PresentParameters();

            presentParameters.Windowed = true;                        //true=windowed, false=full screened
            presentParameters.SwapEffect = SwapEffect.Discard;        //Flip and Copy are other possibilities
            presentParameters.EnableAutoDepthStencil = true;
            presentParameters.AutoDepthStencilFormat = DepthFormat.D16;
            presentParameters.BackBufferFormat = Format.Unknown;

            //Create the device
            device = new Device(0,                                    //the first adapter
                                DeviceType.Hardware,                  //"Reference" is another possibility
                                this,                                 //which control to bind this device to
                                createFlags,                          //processing done by the GPU/CPU
                                presentParameters);                   //how data is present to the screen

            //setup the delegate
            device.DeviceReset += new EventHandler(OnDeviceReset);

            //called the delegate
            OnDeviceReset(device, null);

            return;
        }

        //This method mainly re-initializes the mesh and the projection matrix
        private void OnDeviceReset(object sender, EventArgs e)
        {
            Device dev = (Device)sender;

            //setup the viewing frustrum
            dev.Transform.Projection = Matrix.PerspectiveFovLH(
                (float)Math.PI / 4.0f,
                (float)this.ClientSize.Width / (float)this.ClientSize.Height,
                0.1f, 100.0F);

            dev.RenderState.ZBufferEnable = true;
            dev.RenderState.Lighting = true;
            dev.RenderState.SpecularEnable = true;

            //setup the light
            dev.Lights[0].Type = LightType.Directional;
            dev.Lights[0].Direction = new Vector3(1, 0, 1);
            dev.Lights[0].Diffuse = Color.White;
            dev.Lights[0].Specular = Color.White;
            dev.Lights[0].Enabled = true;

            dev.RenderState.Ambient = Color.Gray;

            teapotMesh = Mesh.Teapot(dev);

            //initialize the mesh variable
            mesh = Mesh.Teapot(dev);

            //setup the material for the teapot
            teapotMaterial = new Microsoft.DirectX.Direct3D.Material();
            teapotMaterial.Diffuse = Color.White;
        }

        public void Render()
        {
            //create the bottom left viewport
            Microsoft.DirectX.Direct3D.Viewport bottomLeftViewport = new Microsoft.DirectX.Direct3D.Viewport();
            bottomLeftViewport.X = 0;
            bottomLeftViewport.Y = 0;
            bottomLeftViewport.Width = this.ClientSize.Width / 2;
            bottomLeftViewport.Height = this.ClientSize.Height / 2;


            //create the bottom right viewport
            Microsoft.DirectX.Direct3D.Viewport bottomRightViewport = new Microsoft.DirectX.Direct3D.Viewport();
            bottomRightViewport.X = this.ClientSize.Width / 2;
            bottomRightViewport.Y = 0;
            bottomRightViewport.Width = this.ClientSize.Width / 2;
            bottomRightViewport.Height = this.ClientSize.Height / 2;

            //create the top left viewport
            Microsoft.DirectX.Direct3D.Viewport topLeftViewport = new Microsoft.DirectX.Direct3D.Viewport();
            topLeftViewport.X = 0;
            topLeftViewport.Y = this.ClientSize.Height / 2;
            topLeftViewport.Width = this.ClientSize.Width / 2;
            topLeftViewport.Height = this.ClientSize.Height / 2;

            //create the top right viewport
            Microsoft.DirectX.Direct3D.Viewport topRightViewport = new Microsoft.DirectX.Direct3D.Viewport();
            topRightViewport.X = this.ClientSize.Width / 2;
            topRightViewport.Y = this.ClientSize.Height / 2;
            topRightViewport.Width = this.ClientSize.Width / 2;
            topRightViewport.Height = this.ClientSize.Height / 2;

            RenderToViewport(bottomLeftViewport, Color.FromArgb(255, 255, 0, 0), 1);
            RenderToViewport(bottomRightViewport, Color.FromArgb(255, 0, 255, 0), -1);
            RenderToViewport(topLeftViewport, Color.FromArgb(255, 0, 0, 255), -5);
            RenderToViewport(topRightViewport, Color.FromArgb(255, 100, 100, 100), 25);
            angle += 0.1f;

            //Copy the back buffer to the display
            device.Present();
        }

        private void RenderToViewport(Microsoft.DirectX.Direct3D.Viewport viewport, Color color, int flip)
        {
            device.Viewport = viewport;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
            device.BeginScene();
            device.Transform.View = Matrix.Identity;
            device.Transform.World = Matrix.RotationYawPitchRoll(
                Geometry.DegreeToRadian((spinX + angle) * flip),
                Geometry.DegreeToRadian((spinY + angle) * flip),
                0.0f)
                * Matrix.Translation(0.0f, 0.0f, 5.0f);
            device.Material = teapotMaterial;
            teapotMesh.DrawSubset(0);
            device.EndScene();
        }

    }
}
