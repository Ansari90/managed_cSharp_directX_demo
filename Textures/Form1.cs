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

namespace Textures
{
    public partial class Form1 : Form
    {
        Device device;
        VertexBuffer vertexBuffer;
        float angle;
        Texture texture;

        public Form1()
        {
            InitializeComponent();
        }

        //the InitializeGraphics() method in Form.cs
        public void InitializeGraphics()
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

            //Create the vertex buffer
            vertexBuffer = new VertexBuffer(
                                typeof(CustomVertex.PositionTextured),
                                36,
                                device,
                                Usage.Dynamic | Usage.WriteOnly,
                                CustomVertex.PositionTextured.Format,
                                Pool.Default);

            //install the delegate that will be called when the buffer needs to refresh its data
            vertexBuffer.Created += new EventHandler(this.OnVertexBufferCreate);
            //invoke the delegate
            OnVertexBufferCreate(vertexBuffer, null);

            //installs the delegate that will be called wht the device is reset
            device.DeviceReset += new EventHandler(OnDeviceReset);

            //invoke the device reset delegate
            OnDeviceReset(device, null);
        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
            texture = new Texture(dev, new Bitmap(Image.FromFile("C:\\Users\\Abdullah AA\\COMP392\\GraphicsWork\\Lab01\\Textures\\call_of_duty_2.jpg")), Usage.Dynamic, Pool.Default);
        }

        //the OnVertexBufferCreate() method in Form.cs
        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            CustomVertex.PositionTextured[] verts = new CustomVertex.PositionTextured[36];
            
            //1st face
            verts[0] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.1f, 1.0f, 0.0f);

            verts[1] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.1f, 1.0f, 1.0f);

            verts[2] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.1f, 0.52f, 1.0f);

            verts[3] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.1f, 1.0f, 0.0f);

            verts[4] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.1f, 0.52f, 1.0f);

            verts[5] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.1f, 0.52f, 0.0f);

            //2nd face
            verts[6] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.0f, 0.52f, 0.0f);

            verts[7] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.52f, 1.0f);

            verts[8] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.1f, 0.47f, 1.0f);

            verts[9] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.0f, 0.52f, 0.0f);

            verts[10] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.1f, 0.47f, 1.0f);

            verts[11] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.1f, 0.47f, 0.0f);

            //3rd face
            verts[12] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.0f, 0.47f, 0.0f);

            verts[13] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.0f, 0.47f, 1.0f);

            verts[14] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.0f, 1.0f);

            verts[15] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.0f, 0.47f, 0.0f);

            verts[16] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.0f, 1.0f);

            verts[17] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.0f, 0.0f, 0.0f);

            //4th face
            verts[18] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.1f, 0.52f, 0.0f);

            verts[19] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.1f, 0.52f, 1.0f);

            verts[20] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.0f, 0.47f, 1.0f);

            verts[21] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.1f, 0.52f, 0.0f);

            verts[22] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.0f, 0.47f, 1.0f);

            verts[23] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.0f, 0.47f, 0.0f);

            //5th face (top face)
            verts[24] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.0f, 0.6f, 0.0f);

            verts[25] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.1f, 0.6f, 0.0f);

            verts[26] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.1f, 0.6f, 0.0f);

            verts[27] = new CustomVertex.PositionTextured(0.0f, 1.0f, 0.0f, 0.6f, 0.0f);

            verts[28] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.1f, 0.6f, 0.0f);

            verts[29] = new CustomVertex.PositionTextured(0.8f, 1.0f, 0.0f, 0.6f, 0.0f);

            //6th face (bottom)
            verts[30] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.6f, 0.0f);

            verts[31] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.1f, 0.6f, 0.0f);

            verts[32] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.1f, 0.6f, 0.0f);

            verts[33] = new CustomVertex.PositionTextured(0.0f, 0.0f, 0.0f, 0.6f, 0.0f);

            verts[34] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.0f, 0.6f, 0.0f);

            verts[35] = new CustomVertex.PositionTextured(0.8f, 0.0f, 0.1f, 0.6f, 0.0f);


            buffer.SetData(verts, 0, LockFlags.None);
        }

        //the SetupCamera() method in Form.cs
        private void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1.0f, 100.0f);
            device.Transform.View = Matrix.LookAtLH(
              new Vector3(0.0f, 10.0f, 10.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;
            //device.RenderState.CullMode = Cull.None;
        }

        //the Render() method in Form.cs
        public void Render()
        {
            device.Clear(ClearFlags.Target, Color.CornflowerBlue, 1.0f, 0);
            SetupCamera();
            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, vertexBuffer, 0);
            device.SetTexture(0, texture);
            angle += 0.03f;

            
            /*
            device.Transform.World = Matrix.RotationYawPitchRoll(
              angle / (float)Math.PI,
              angle / (float)Math.PI * 2.0f,
              angle / (float)Math.PI / 4.0f);
            */


            device.Transform.World = Matrix.RotationAxis(new Vector3((float)(angle / (Math.PI * 2)), (float)(angle / (Math.PI * 3)), (float)(angle / (Math.PI * 4))), angle);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);

            device.Transform.World = Matrix.Translation(-3, 3.5f, 0) * Matrix.RotationAxis(new Vector3((float)(angle / (Math.PI * 3)), (float)(angle / (Math.PI)), (float)(angle / (Math.PI * 6))), 2 * angle);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);

            device.Transform.World = Matrix.Translation(3, -3.5f, 0) * Matrix.RotationY(angle);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);

            device.EndScene();
            device.Present();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
