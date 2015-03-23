using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Mesh_Intro
{
    public partial class Form1 : Form
    {
        Device device;
        float angle;
        Mesh mesh;

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

            //setup the delegate
            device.DeviceReset += new EventHandler(OnDeviceReset);

            //called the delegate
            OnDeviceReset(device, null);

            return true;
        }

        //This method mainly initializes the mesh
        private void OnDeviceReset(object sender, EventArgs e)
        {
            Device dev = (Device)sender;

            //set up material with diffuse and ambient white color 
            Material myMaterial = new Material();
            myMaterial.Diffuse = myMaterial.Ambient = Color.White;
            dev.Material = myMaterial;

            //initialize the mesh variable
            //mesh = Mesh.Teapot(dev);
            //mesh = Mesh.FromFile(@"..\..\Object.x", MeshFlags.Managed, dev);

            //mesh = Mesh.Cylinder(dev, .9f, .4f, 1.4f, 24, 12);
            //  mesh = Mesh.Box(dev, 1, 1, 1);
            //  mesh = Mesh.Sphere(dev, 1.5f, 48, 48);
            mesh = Mesh.Torus(dev, .3f, .9f, 24, 48);
            // System.Drawing.Font font = new System.Drawing.Font("Times New Roman", 4.0f, FontStyle.Regular);
            //  mesh = Mesh.TextFromFont(device, font, "Narendra is the greatest", 0.001f, 0.2f);

            int deviceResetCounter = 0;
            //for debugging
            this.Text = String.Format("Device reset {0} times", deviceResetCounter++);
        }

        private void SetupCamera()
        {
            //create the light
            device.Lights[0].Type = LightType.Point;
            device.Lights[0].Position = new Vector3(-3, 5, -3);
            device.Lights[0].Diffuse = System.Drawing.Color.Red;//.Gold; //red light
            device.Lights[0].Attenuation0 = 0.2f;
            device.Lights[0].Range = 1000.0f;
            device.Lights[0].Enabled = true;

            //Rotation about an arbitrary axis
            device.Transform.World = Matrix.RotationY(angle);
            /*device.Transform.World = Matrix.RotationAxis(
              new Vector3(
                angle / ((float)Math.PI * 2.0f),
                angle / ((float)Math.PI * 4.0f),
                angle / ((float)Math.PI * 6.0f)),
                angle / (float)Math.PI);*/
            angle += 0.01f;

            device.Transform.Projection = Matrix.PerspectiveFovLH(
              (float)Math.PI / 4,            //angle of the Field Of View
              this.Width / this.Height,      //similar to the aspect of a tv
              1.0f,                          //the near plane
              100.0f);                       //the far plane

            device.Transform.View = Matrix.LookAtLH(
              new Vector3(0, 0, -5.0f),        //camera position
              new Vector3(),                 //what direction to look
              new Vector3(0, 1, 0));           //what direction is up
            device.RenderState.Lighting = true;//there are no lights in this scene, let each object emit its own light
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
            mesh.DrawSubset(0);

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
