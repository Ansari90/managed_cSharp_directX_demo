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

namespace Lights
{
    public partial class Form1 : Form
    {
        Device device;
        VertexBuffer vertexBuffer;
        float angle;

        public Form1()
        {
            InitializeComponent();
        }

        public bool InitializeGraphics()
        {
            //Set the presentation parameters
            PresentParameters presentParameters = new PresentParameters();

            presentParameters.Windowed = true;                        //true=windowed, false=full screened
            presentParameters.SwapEffect = SwapEffect.Discard;        //Flip and Copy are other possibilities

            //Create the device
            device = new Device(0,                                    //the first adapter
                                DeviceType.Hardware,                  //"Reference" is another possibility
                                this,                                 //which control to bind this device to
                                CreateFlags.SoftwareVertexProcessing, //processing done by the CPU
                                presentParameters);                   //how data is present to the screen

            //create the vertex buffer
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionNormalColored),
                 36, device, Usage.Dynamic | Usage.WriteOnly,
                 CustomVertex.PositionNormalColored.Format, Pool.Default);

            //setup the delegate
            vertexBuffer.Created += new EventHandler(OnVertexBufferCreate);

            //called the delegate
            OnVertexBufferCreate(vertexBuffer, null);

            return true;
        }



        //This method mainly initializes the items in the buffers
        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[3];

            //first triangle
            verts[0].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[0].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            verts[0].Color = System.Drawing.Color.Purple.ToArgb();

            verts[1].Position = new Vector3(-1.0f, -1.0f, 1.0f);
            verts[1].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            verts[1].Color = System.Drawing.Color.Pink.ToArgb();

            verts[2].Position = new Vector3(1.0f, -1.0f, 1.0f);
            verts[2].Normal = new Vector3(0.0f, 0.0f, -1.0f);
            verts[2].Color = System.Drawing.Color.Black.ToArgb();

            //transfer the vertices data to the buffer
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            //create the light - Lights will come into effect if lighting is disables for the scene
            device.Lights[0].Type = LightType.Point;
            device.Lights[0].Position = new Vector3();
            device.Lights[0].Diffuse = System.Drawing.Color.Red; //red light
            device.Lights[0].Attenuation0 = 0.2f;
            device.Lights[0].Range = 1000.0f;
            device.Lights[0].Enabled = true;

            device.RenderState.CullMode = Cull.None;
            //Rotation about an arbitrary axis
            device.Transform.World = Matrix.RotationAxis(
              new Vector3(
                angle / ((float)Math.PI * 2.0f),
                angle / ((float)Math.PI * 4.0f),
                angle / ((float)Math.PI * 6.0f)),
                angle / (float)Math.PI);
            angle += 0.1f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              (float)Math.PI / 4,            //angle of the Field Of View
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(0, 0, -5.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;//there are no lights in this scene, let each object emit its own light
        }



        public void Render()
        {
            //clears the back buffer with a blue color
            device.Clear(ClearFlags.Target, System.Drawing.Color.CornflowerBlue, 1.0f, 0);

            //Call the setupcamera method
            SetupCamera();

            //Ready Direct3D to begin drawing
            device.BeginScene();

            //Draw the scene - All your 3D Rendering calls got here
            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionNormalColored.Format;
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            //Indicate to Direct3D that we are done drawing
            device.EndScene();

            //Copy the back buffer to the display
            device.Present();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
