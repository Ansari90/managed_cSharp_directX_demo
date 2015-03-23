using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Lab05
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private VertexBuffer vertexBuffer = null;
        private float angle;

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
            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored),
                   5, device, Usage.Dynamic | Usage.WriteOnly,
                   CustomVertex.PositionColored.Format, Pool.Default);

            //setup the delegate
            vertexBuffer.Created += new EventHandler(OnVertexBufferCreatePyramid);

            //called the delegate
            OnVertexBufferCreatePyramid(vertexBuffer, null);

            return true;
        }

        public void Render()
        {
            //clears the back buffer with a blue color
            device.Clear(ClearFlags.Target, System.Drawing.Color.CornflowerBlue, 1.0f, 0);
            SetupCamera();

            //Ready Direct3D to begin drawing
            device.BeginScene();

            //Draw the scene - All your 3D Rendering calls got here
            //now bind the vertex buffer to the datastream
            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawPrimitives(PrimitiveType.TriangleList, //the primitive to draw
            0,                                                //the index of the first vertex to load
            4);                                               //the number of primitives to draw

            //Indicate to Direct3D that we are done drawing
            device.EndScene();

            //Copy the back buffer to the display
            device.Present();
        }

        //This method mainly initializes the items in the buffers
        private void OnVertexBufferCreatePyramid(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            CustomVertex.PositionColored[] verts = new CustomVertex.PositionColored[12];

            verts[0].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[0].Color = System.Drawing.Color.Green.ToArgb();

            verts[1].Position = new Vector3(1.0f, 0.0f, 0.0f);
            verts[1].Color = System.Drawing.Color.Red.ToArgb();

            verts[2].Position = new Vector3(0.0f, 0.0f, -1.0f);
            verts[2].Color = System.Drawing.Color.Blue.ToArgb();

            verts[3].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[3].Color = System.Drawing.Color.Red.ToArgb();

            verts[4].Position = new Vector3(0.0f, 0.0f, -1.0f);
            verts[4].Color = System.Drawing.Color.Green.ToArgb();

            verts[5].Position = new Vector3(-1.0f, 0.0f, 0.0f);
            verts[5].Color = System.Drawing.Color.Blue.ToArgb();

            verts[6].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[6].Color = System.Drawing.Color.Red.ToArgb();

            verts[7].Position = new Vector3(-1.0f, 0.0f, 0.0f);
            verts[7].Color = System.Drawing.Color.Green.ToArgb();

            verts[8].Position = new Vector3(0.0f, 0.0f, 1.0f);
            verts[8].Color = System.Drawing.Color.Blue.ToArgb();

            verts[9].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[9].Color = System.Drawing.Color.Red.ToArgb();

            verts[10].Position = new Vector3(0.0f, 0.0f, 1.0f);
            verts[10].Color = System.Drawing.Color.Green.ToArgb();

            verts[11].Position = new Vector3(1.0f, 0.0f, 0.0f);
            verts[11].Color = System.Drawing.Color.Blue.ToArgb();

            //transfer the vertices data to the buffer
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            //device.RenderState.CullMode = Cull.None;
            //Rotation about an arbitrary axis
            device.Transform.World = Matrix.RotationY(angle);
            //device.Transform.World = Matrix.RotationAxis       new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
            angle += 0.01f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              0.4f,                          //angle of the Field Of View       //(float)Math.PI / 4
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(1.0f, 4.5f, 4.5f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;//there are no lights in this scene, let each object emit its own light
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}