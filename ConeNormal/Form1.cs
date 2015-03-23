﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NormalCone
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
                 108, device, Usage.Dynamic | Usage.WriteOnly,
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
            CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[108];

            float angle = (float)(Math.PI / 2);
            float change = (float)((2 * Math.PI) / 36);
            float x, z;

            Vector3 top = new Vector3(0.0f, 2.0f, 0.0f);

            //first triangle
            for (int i = 0, j = 0; i < 108; i++, j++)
            {
                if (i % 3 == 0)
                {
                    verts[i].Position = top;
                    if (i != 0)
                    {
                        verts[i - 1].Normal = Vector3.Normalize(Vector3.Cross(Vector3.Subtract(top, verts[i - 1].Position), Vector3.Subtract(verts[i - 2].Position, verts[i - 1].Position)));
                        if(i != 3)
                            verts[i - 2].Normal = verts[i - 4].Normal;
                    }
                }
                else
                {
                    x = (float)Math.Cos((double)angle);
                    z = (float)Math.Sin((double)angle);
                    verts[i].Position = new Vector3((2 * x), -1.0f, (2 * z));
                    verts[i].Color = System.Drawing.Color.White.ToArgb();
                    verts[i].Normal = new Vector3(0.0f, 1.0f, 0.0f);
                }

                if (j == 1)
                    angle -= change;
                if (j == 2)
                    j = -1;
            }
            //last 2 pair of vertices (base of last triangle) are not given a normal in the loop (normal assignment happens at the start of every new triangle
            verts[107].Normal = Vector3.Normalize(Vector3.Cross(Vector3.Subtract(top, verts[107].Position), Vector3.Subtract(verts[106].Position, verts[107].Position)));
            verts[106].Normal = verts[104].Normal;
            //Since 2nd vertex of a triangle's normal is not assigned in loop (2nd vertex assignment happens from the 2nd triangle onwards
            verts[1].Normal = verts[107].Normal;

            //transfer the vertices data to the buffer
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void SetupCamera()
        {
            //create the light
            device.Lights[0].Type = LightType.Point;
            device.Lights[0].Position = new Vector3(-5.0f, 3.0f, -1.5f);
            device.Lights[0].Diffuse = System.Drawing.Color.Red; //red light
            device.Lights[0].Attenuation0 = 0.5f;
            device.Lights[0].Range = 1000.0f;
            device.Lights[0].Enabled = true;

            //device.RenderState.CullMode = Cull.None;
            device.Transform.World = Matrix.RotationY(angle);
            //Rotation about an arbitrary axis
            /*device.Transform.World = Matrix.RotationAxis(
              new Vector3(
                angle / ((float)Math.PI * 2.0f),
                angle / ((float)Math.PI * 4.0f),
                angle / ((float)Math.PI * 6.0f)),
                angle / (float)Math.PI);
            */
            angle += 0.01f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              (float)Math.PI / 4,            //angle of the Field Of View
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(0.0f, 4.0f, -7.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = true;
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
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 36);

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
