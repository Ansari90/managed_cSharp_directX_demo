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


namespace BumpMapping
{
    public partial class Form1 : Form
    {
        Device device;
        VertexBuffer vertexBuffer;
        float angle;
        Texture texture;
        CustomVertex.PositionColored[] verts;
        Bitmap theBitmap;
        Cull theCull = Cull.None;
        FillMode theMode = FillMode.Solid;

        float xView = 0.0f;
        float yView = 300.0f;
        float zView = 300.0f;


        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(keyPressed);
        }

        public void keyPressed(Object sender, EventArgs e)
        {
            KeyEventArgs thePressedKey = (KeyEventArgs)e;

            if (thePressedKey.KeyCode == Keys.W)
            {
                yView += 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.A)
            {
                xView -= 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.S)
            {
                yView -= 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.D)
            {
                xView += 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.Q)
            {
                zView -= 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.E)
            {
                zView += 10.0f;
            }
            if (thePressedKey.KeyCode == Keys.H)
            {
                zView = 300.0f;
                yView = 300.0f;
                xView = 0f;
            }
            if (thePressedKey.KeyCode == Keys.F)
            {
                if (theMode == FillMode.WireFrame)
                    theMode = FillMode.Solid;
                else
                {
                    if (theMode == FillMode.Solid)
                        theMode = FillMode.Point;
                    else
                    {
                        if (theMode == FillMode.Point)
                            theMode = FillMode.WireFrame;
                    }
                }
            }
            if (thePressedKey.KeyCode == Keys.C)
            {
                if (theCull == Cull.None)
                    theCull = Cull.Clockwise;
                else
                {
                    if (theCull == Cull.Clockwise)
                        theCull = Cull.CounterClockwise;
                    else
                    {
                        if (theCull == Cull.CounterClockwise)
                            theCull = Cull.None;
                    }
                }
            }
        }

        public void setupAllVertices()
        {
            float yValue = 200000;
            bool firstSquare = true, firstHorizontalStrip = true;
            theBitmap = new Bitmap(Image.FromFile("C:\\Users\\Abdullah AA\\COMP392\\GraphicsWork\\Lab01\\BumpMapping\\gradient5.bmp"));
            Bitmap textureMap = new Bitmap(Image.FromFile("C:\\Users\\Abdullah AA\\COMP392\\GraphicsWork\\Lab01\\BumpMapping\\gradient5.bmp"));
            int u = 0, v = 0, u1 = 0, v1 = 0, reduceU = 0;

            //allColors[(int)triangleCounter].ToArgb()
            verts = new CustomVertex.PositionColored[540000];
            for (float x = -150.0f, triangleCounter = 0.0f; x < 150.0f; x++, u++, u1++)
            {
                v = 0; v1 = 0;
                firstSquare = true;
                for (float z = -150.0f; z < 150.0f; z++, v++, v1++)
                {
                    if (firstSquare == true)
                    {
                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3(x, -1 * theBitmap.GetPixel(u - reduceU, v).ToArgb() / yValue, z);
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb();
                        triangleCounter++;

                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3(x, -1 * theBitmap.GetPixel(u - reduceU, v).ToArgb() / yValue, (z + 1));
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb();
                        triangleCounter++;

                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3((x + 1), -1 * theBitmap.GetPixel(u, v).ToArgb() / yValue, (z + 1));
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb(); ;
                        triangleCounter++;

                        //2nd triangle
                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 3.0f)];
                        triangleCounter++;

                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 2.0f)];
                        triangleCounter++;

                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3((x + 1), -1 * theBitmap.GetPixel(u, v).ToArgb() / yValue, z);
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb();
                        triangleCounter++;
                        firstSquare = false;
                    }
                    else
                    {
                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 5.0f)];
                        triangleCounter++;

                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3(x, -1 * theBitmap.GetPixel(u - reduceU, v).ToArgb() / yValue, (z + 1));
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb();
                        triangleCounter++;

                        verts[(int)triangleCounter] = new CustomVertex.PositionColored();
                        verts[(int)triangleCounter].Position = new Vector3((x + 1), -1 * theBitmap.GetPixel(u, v).ToArgb() / yValue, (z + 1));
                        verts[(int)triangleCounter].Color = textureMap.GetPixel(u1, v1).ToArgb(); ;
                        triangleCounter++;

                        //2nd triangle
                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 3.0f)];
                        triangleCounter++;

                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 2.0f)];
                        triangleCounter++;

                        verts[(int)triangleCounter] = verts[(int)(triangleCounter - 7.0f)];
                        triangleCounter++;
                    }
                }
                if (firstHorizontalStrip == true)
                {
                    reduceU = 1;
                    firstHorizontalStrip = false;
                }
            }
        }

        //the InitializeGraphics() method in Form.cs
        public void InitializeGraphics()
        {
            setupAllVertices();

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
                                typeof(CustomVertex.PositionColored),
                                540000,
                                device,
                                Usage.Dynamic | Usage.WriteOnly,
                                CustomVertex.PositionColored.Format,
                                Pool.Default);

            //install the delegate that will be called when the buffer needs to refresh its data
            vertexBuffer.Created += new EventHandler(this.OnVertexBufferCreate);
            //invoke the delegate
            OnVertexBufferCreate(vertexBuffer, null);
            /*
            //installs the delegate that will be called wht the device is reset
            device.DeviceReset += new EventHandler(OnDeviceReset);

            //invoke the device reset delegate
            OnDeviceReset(device, null);
             * */
        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            Device dev = (Device)sender;
            texture = new Texture(dev, theBitmap, Usage.Dynamic, Pool.Default);
        }

        //the OnVertexBufferCreate() method in Form.cs
        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;
            buffer.SetData(verts, 0, LockFlags.None);
        }

        //the SetupCamera() method in Form.cs
        private void SetupCamera()
        {
            this.Text = "X: " + xView + " Y: " + yView + " Z: " + zView; 
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1.0f, 6000.0f);
            device.Transform.View = Matrix.LookAtLH(
              new Vector3(xView, yView, zView),        //camera Position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = false;
            device.RenderState.CullMode = theCull;
        }

        //the Render() method in Form.cs
        public void Render()
        {
            device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
            SetupCamera();
            device.BeginScene();
            device.RenderState.FillMode = theMode;
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.SetStreamSource(0, vertexBuffer, 0);
            //device.SetTexture(0, texture);
            angle += 0.01f;


            /*
            device.Transform.World = Matrix.RotationYawPitchRoll(
              angle / (float)Math.PI,
              angle / (float)Math.PI * 2.0f,
              angle / (float)Math.PI / 4.0f);
            */

            device.Transform.World = Matrix.RotationAxis(new Vector3(0.0f, 1.0f, 0.0f), angle);
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 180000);

            device.EndScene();
            device.Present();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
