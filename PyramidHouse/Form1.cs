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

namespace Lab06_PyramidHouse
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
                   36, device, Usage.Dynamic | Usage.WriteOnly,
                   CustomVertex.PositionColored.Format, Pool.Default);

            //setup the delegate
            vertexBuffer.Created += new EventHandler(OnVertexBufferCreateCube);

            //called the delegate
            OnVertexBufferCreateCube(vertexBuffer, null);

            return true;
        }

        public void Render()
        {
            //clears the back buffer with a blue color
            device.Clear(ClearFlags.Target, System.Drawing.Color.CornflowerBlue, 5.0f, 0);
            SetupCamera();

            //Ready Direct3D to begin drawing
            device.BeginScene();

            //Draw the scene - All your 3D Rendering calls got here
            //now bind the vertex buffer to the datastream
            device.SetStreamSource(0, vertexBuffer, 0);
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawPrimitives(PrimitiveType.TriangleList, //the primitive to draw
            0,                                                //the index of the first vertex to load
            12);                                              //the number of primitives to draw

            //Indicate to Direct3D that we are done drawing
            device.EndScene();

            //Copy the back buffer to the display
            device.Present();
        }

        //This method mainly initializes the items in the buffers
        private void OnVertexBufferCreateCube(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            CustomVertex.PositionColored[] verts = new CustomVertex.PositionColored[36];

            //1st face
            verts[0].Position = new Vector3(0.0f, 1.0f, 1.0f);
            verts[0].Color = System.Drawing.Color.Red.ToArgb();

            verts[1].Position = new Vector3(0.0f, 0.0f, 1.0f);
            verts[1].Color = System.Drawing.Color.Green.ToArgb();

            verts[2].Position = new Vector3(1.0f, 0.0f, 1.0f);
            verts[2].Color = System.Drawing.Color.Blue.ToArgb();

            verts[3].Position = new Vector3(0.0f, 1.0f, 1.0f);
            verts[3].Color = System.Drawing.Color.Green.ToArgb();

            verts[4].Position = new Vector3(1.0f, 0.0f, 1.0f);
            verts[4].Color = System.Drawing.Color.Red.ToArgb();

            verts[5].Position = new Vector3(1.0f, 1.0f, 1.0f);
            verts[5].Color = System.Drawing.Color.Blue.ToArgb();

            //2nd face
            verts[6].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[6].Color = System.Drawing.Color.Red.ToArgb();

            verts[7].Position = new Vector3(0.0f, 0.0f, 0.0f);
            verts[7].Color = System.Drawing.Color.Green.ToArgb();

            verts[8].Position = new Vector3(0.0f, 0.0f, 1.0f);
            verts[8].Color = System.Drawing.Color.Blue.ToArgb();

            verts[9].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[9].Color = System.Drawing.Color.Green.ToArgb();

            verts[10].Position = new Vector3(0.0f, 0.0f, 1.0f);
            verts[10].Color = System.Drawing.Color.Red.ToArgb();

            verts[11].Position = new Vector3(0.0f, 1.0f, 1.0f);
            verts[11].Color = System.Drawing.Color.Blue.ToArgb();

            //3rd face
            verts[12].Position = new Vector3(1.0f, 1.0f, 0.0f);
            verts[12].Color = System.Drawing.Color.Red.ToArgb();

            verts[13].Position = new Vector3(1.0f, 0.0f, 0.0f);
            verts[13].Color = System.Drawing.Color.Green.ToArgb();

            verts[14].Position = new Vector3(0.0f, 0.0f, 0.0f);
            verts[14].Color = System.Drawing.Color.Blue.ToArgb();

            verts[15].Position = new Vector3(1.0f, 1.0f, 0.0f);
            verts[15].Color = System.Drawing.Color.Green.ToArgb();

            verts[16].Position = new Vector3(0.0f, 0.0f, 0.0f);
            verts[16].Color = System.Drawing.Color.Red.ToArgb();

            verts[17].Position = new Vector3(0.0f, 1.0f, 0.0f);
            verts[17].Color = System.Drawing.Color.Blue.ToArgb();

            //4th face
            verts[18].Position = new Vector3(1.0f, 1.0f, 1.0f);
            verts[18].Color = System.Drawing.Color.Red.ToArgb();

            verts[19].Position = new Vector3(1.0f, 0.0f, 1.0f);
            verts[19].Color = System.Drawing.Color.Green.ToArgb();

            verts[20].Position = new Vector3(1.0f, 0.0f, 0.0f);
            verts[20].Color = System.Drawing.Color.Blue.ToArgb();

            verts[21].Position = new Vector3(1.0f, 1.0f, 1.0f);
            verts[21].Color = System.Drawing.Color.Green.ToArgb();

            verts[22].Position = new Vector3(1.0f, 0.0f, 0.0f);
            verts[22].Color = System.Drawing.Color.Red.ToArgb();

            verts[23].Position = new Vector3(1.0f, 1.0f, 0.0f);
            verts[23].Color = System.Drawing.Color.Blue.ToArgb();

            //Pyramid (Roof)
            verts[24].Position = new Vector3(0.5f, 1.5f, 0.5f);
            verts[24].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[25].Position = new Vector3(-0.2f, 1.0f, 1.2f);
            verts[25].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[26].Position = new Vector3(1.2f, 1.0f, 1.2f);
            verts[26].Color = System.Drawing.Color.Black.ToArgb();

            verts[27].Position = new Vector3(0.5f, 1.5f, 0.5f);
            verts[27].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[28].Position = new Vector3(-0.2f, 1.0f, -0.2f);
            verts[28].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[29].Position = new Vector3(-0.2f, 1.0f, 1.2f);
            verts[29].Color = System.Drawing.Color.Black.ToArgb();

            verts[30].Position = new Vector3(0.5f, 1.5f, 0.5f);
            verts[30].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[31].Position = new Vector3(1.2f, 1.0f, -0.2f);
            verts[31].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[32].Position = new Vector3(-0.2f, 1.0f, -0.2f);
            verts[32].Color = System.Drawing.Color.Black.ToArgb();

            verts[33].Position = new Vector3(0.5f, 1.5f, 0.5f);
            verts[33].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[34].Position = new Vector3(1.2f, 1.0f, 1.2f);
            verts[34].Color = System.Drawing.Color.Yellow.ToArgb();

            verts[35].Position = new Vector3(1.2f, 1.0f, -0.2f);
            verts[35].Color = System.Drawing.Color.Black.ToArgb();

            //transfer the vertices data to the buffer
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            //device.RenderState.CullMode = Cull.None;
            //Rotation about an arbitrary axis
            device.Transform.World = Matrix.RotationY(angle);
            //device.Transform.World = Matrix.RotationAxis       new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
            angle += 0.025f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              1.0f,                          //angle of the Field Of View       //(float)Math.PI / 4
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(3.0f, 2.0f, 2.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;//there are no lights in this scene, let each object emit its own light
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}