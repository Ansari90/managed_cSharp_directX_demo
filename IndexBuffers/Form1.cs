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

namespace Lab06_IndexBuffers
{
    public partial class Form1 : Form
    {
        private Device device = null;
        private VertexBuffer vertexBuffer = null;
        private IndexBuffer indexBuffer;
        private static readonly short[] indices = {
                                           0,1,2,   //front
                                           0,2,3,   //front
                                           5,4,7,   //back
                                           5,7,6,   //back
                                           1,5,6,   //right
                                           1,6,2,   //right
                                           4,0,3,   //left
                                           4,3,7,   //left
                                           0,4,5,   //top
                                           0,5,1,    //top
                                           3,2,6,//bottom
                                           3,6,7//bottom
                                          };
        private float angle;

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
            vertexBuffer.Created += new EventHandler(OnVertexBufferCreate);

            //called the delegate
            OnVertexBufferCreate(vertexBuffer, null);

            //create the index buffer
            indexBuffer = new IndexBuffer(typeof(short), indices.Length, device, Usage.WriteOnly, Pool.Default);

            //setup the delegate
            indexBuffer.Created += new EventHandler(OnIndexBufferCreate);

            //called the delegate
            OnIndexBufferCreate(indexBuffer, null);
            return true;
        }

        public void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            CustomVertex.PositionColored[] verts = new CustomVertex.PositionColored[8];

            //first plane
            verts[0] = new CustomVertex.PositionColored(new Vector3(-1.0f, 1.0f, -1.0f), Color.Red.ToArgb());//A
            verts[1] = new CustomVertex.PositionColored(new Vector3(1.0f, 1.0f, -1.0f), Color.Green.ToArgb());//B
            verts[2] = new CustomVertex.PositionColored(new Vector3(1.0f, -1.0f, -1.0f), Color.Blue.ToArgb());//C
            verts[3] = new CustomVertex.PositionColored(new Vector3(-1.0f, -1.0f, -1.0f), Color.Green.ToArgb());//D

            //second plane
            verts[4] = new CustomVertex.PositionColored(new Vector3(-1.0f, 1.0f, 1.0f), Color.Red.ToArgb());//E
            verts[5] = new CustomVertex.PositionColored(new Vector3(1.0f, 1.0f, 1.0f), Color.Green.ToArgb());//F
            verts[6] = new CustomVertex.PositionColored(new Vector3(1.0f, -1.0f, 1.0f), Color.Blue.ToArgb());//G
            verts[7] = new CustomVertex.PositionColored(new Vector3(-1.0f, -1.0f, 1.0f), Color.Green.ToArgb());//H

            //transfer the vertices data to the buffer
            buffer.SetData(verts, 0, LockFlags.None);
        }

        public void OnIndexBufferCreate(object sender, EventArgs e)
        {
            IndexBuffer buffer = (IndexBuffer)sender;
            buffer.SetData(indices, 0, LockFlags.None);
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
            device.Indices = indexBuffer;
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 8, 0, 12);//indices.Length / 3

            //Indicate to Direct3D that we are done drawing
            device.EndScene();

            //Copy the back buffer to the display
            device.Present();
        }

        private void SetupCamera()
        {
            //device.RenderState.CullMode = Cull.None;
            //Rotation about an arbitrary axis
            //device.Transform.World = Matrix.RotationY(angle);
            device.Transform.World = Matrix.RotationAxis(
              new Vector3(
                angle / ((float)Math.PI * 2.0f),
                angle / ((float)Math.PI * 4.0f),
                angle / ((float)Math.PI * 6.0f)),
                angle / (float)Math.PI);
            angle += 0.1f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              1.0f,                          //angle of the Field Of View       //(float)Math.PI / 4
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(3.0f, 2.0f, 4.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;//there are no lights in this scene, let each object emit its own light
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
