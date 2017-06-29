//
// Copyright (c) 2008-2015 the Urho3D project.
// Copyright (c) 2015 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

//Stage 2 Created by thinkyfish at freelancer for teodor5


using System;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.Globalization;
using Urho;
using Urho.Gui;
//using Urho.Actions;
//using Urho.Shapes;
//using Urho.Urho2D;
using Urho.Samples;
using Urho.Resources;

namespace RenderCube
{
    public enum PanelMode { Material, Node };
    public class MyGame : Application
    {

        Scene scene;

        UrhoConsole Console;
        DebugHud DebugHud;
        PanelMode PanelMode;
        public string CurrentModel; //this is the key that tells us which model is currently selected
        Panel CurrentPanel;
        MaterialPanel MaterialPanel;
        NodePanel NodePanel;
        Camera Camera;
        Dictionary<string, XmlFile> materialxml;
        Dictionary<string, Model> models;
        Dictionary<string, Node> userNodes = new Dictionary<string, Node>();
        Dictionary<string, MaterialHelper> userMaterials = new Dictionary<string, MaterialHelper>();

        protected const float TouchSensitivity = 2;
        protected float Yaw { get; set; }
        protected float Pitch { get; set; }
        protected bool TouchEnabled { get; set; }
        protected LookSphereNode CameraNode { get; set; }
        protected MonoDebugHud MonoDebugHud { get; set; }
        private Vector3 currentPosition = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 nextPosition
        {
            get
            {
                Vector3 ret = currentPosition;
                currentPosition += new Vector3(0.0f, 0.0f, 2.0f);
                return ret;
            }
        }
        [Preserve]
        public MyGame(ApplicationOptions options = null) : base(options) { }

        static MyGame()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
            };
        }

        protected override void Start()
        {
            // Set style to the UI root so that elements will inherit it
            UI.Root.SetDefaultStyle(ResourceCache.GetXmlFile("UI/DefaultStyle.xml"));
            materialxml = new Dictionary<string, XmlFile>
            {
                {"Default", ResourceCache.GetXmlFile("Materials/DefaultMaterial.xml")},
               // {"BoxMaterial",  new MaterialHelper(ResourceCache.GetXmlFile("Materials/BoxMaterial.xml")) },
               // {"TeapotMaterial",  new MaterialHelper(ResourceCache.GetXmlFile("Materials/TeapotMaterial.xml")) }
            };

            models = new Dictionary<string, Model>
            {
                {"Sphere", ResourceCache.GetModel("Models/Sphere.mdl")},
                {"Box", ResourceCache.GetModel("Models/Box.mdl") },
                {"Suzanne", ResourceCache.GetModel("Models/Suzanne.mdl") },
                {"Teapot", ResourceCache.GetModel("Models/Teapot.mdl") },
            };

            userNodes = new Dictionary<string, Node> { };

            Log.LogMessage += e => Debug.WriteLine($"[{e.Level}] {e.Message}");
            TouchEnabled = true;
            if (Platform == Platforms.Android ||
                Platform == Platforms.iOS ||
                Options.TouchEmulation)
            {
                InitTouchInput();
            }
            if (Platform == Platforms.Windows)
            {
                Input.SetMouseVisible(true, false);
                //this.JoystickLayoutPatch = JoystickLayoutPatches.Hidden;
                SimpleCreateInstructions("Control The Camera with WASD");
            }
            Input.Enabled = true;
            MonoDebugHud = new MonoDebugHud(this);
            MonoDebugHud.Show();

            //CreateLogo();
            SetWindowAndTitleIcon();
            CreateConsoleAndDebugHud();
            Input.KeyDown += HandleKeyDown;
            CreateScene();
            //debugHud.ToggleAll();


            base.Start();
        }

        //this tells the game engine to use the joystick and zoom in (only on android and IOS)
        protected string JoystickLayoutPatch => JoystickLayoutPatches.Hidden;

        protected override void OnUpdate(float timeStep)
        {

            //MoveCameraByTouches(timeStep);
            base.OnUpdate(timeStep);
            if (Platform == Platforms.Windows)
                SimpleMoveCamera2D(timeStep);
            else if (Platform == Platforms.Android)
                MoveCameraByTouches(timeStep);

        }

        public void SetPanelMode(PanelMode mode)
        {
            if (CurrentPanel != null)
            {
                CurrentPanel.Visible = false;
                //currentPanel.Remove(); //this doesn't work, so we just leave all the invisible panels.  This is a leak.
                CurrentPanel = null;
            }
            switch (mode)
            {
                case PanelMode.Material:

                    MaterialPanel = new MaterialPanel(UI.Root, userMaterials[CurrentModel], ResourceCache);
                    MaterialPanel.Visible = true;

                    MaterialPanel.ControlBar.OnPrevious = (args => this.PreviousPanelMode());
                    MaterialPanel.ControlBar.OnNext = (args => this.NextPanelMode());
                    MaterialPanel.ControlBar.OnCube = (args => this.SelectModel("Box"));
                    MaterialPanel.ControlBar.OnSuzanne = (args => this.SelectModel("Suzanne"));
                    MaterialPanel.ControlBar.OnTeapot = (args => this.SelectModel("Teapot"));
                    CurrentPanel = MaterialPanel;
                    this.PanelMode = mode;
                    break;

                case PanelMode.Node:

                    NodePanel = new NodePanel(UI.Root, userNodes[CurrentModel], ResourceCache);
                    NodePanel.ControlBar.OnPrevious = (args => this.PreviousPanelMode());
                    NodePanel.ControlBar.OnNext = (args => this.NextPanelMode());
                    NodePanel.ControlBar.OnCube = (args => this.SelectModel("Box"));
                    NodePanel.ControlBar.OnSuzanne = (args => this.SelectModel("Suzanne"));
                    NodePanel.ControlBar.OnTeapot = (args => this.SelectModel("Teapot"));
                    NodePanel.PositionField.OnTextFinished = (args =>
                    {
                        userNodes[CurrentModel].Position = args.Value;
                        CameraNode.UpdatePosition();
                    });
                    NodePanel.RotationField.OnTextFinished = (args =>
                    {
                        userNodes[CurrentModel].Rotation = new Quaternion(Vector3.Zero, 1.0f);
                        userNodes[CurrentModel].Rotate(args.Value, TransformSpace.World);
                        CameraNode.UpdatePosition();
                    });
                    NodePanel.ScaleSlider.SliderChanged += (args) => {
                        userNodes[CurrentModel].Scale = new Vector3(args.Value, args.Value, args.Value);
                        };

                    CurrentPanel = this.NodePanel;
                    this.PanelMode = mode;
                    break;

                default:
                    this.PanelMode = mode;
                    break;

            }
            if (Platform == Platforms.Android)
            {
                CurrentPanel.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
                Renderer.SetViewport(0, new Viewport(Context, scene, Camera, new IntRect(0, CurrentPanel.Height, Graphics.Width, Graphics.Height), null));
            }
            else
                Renderer.SetViewport(0, new Viewport(Context, scene, Camera, new IntRect(0, 0, Graphics.Width, Graphics.Height - CurrentPanel.Height), null));

        }

        public void PreviousPanelMode()
        {
            switch (this.PanelMode)
            {
                case (PanelMode.Material):
                    this.SetPanelMode(PanelMode.Node);

                    break;
                case (PanelMode.Node):
                    this.SetPanelMode(PanelMode.Material);
                    break;
            }
        }
        public void NextPanelMode()
        {
            switch (this.PanelMode)
            {
                case (PanelMode.Material):
                    this.SetPanelMode(PanelMode.Node);

                    break;
                case (PanelMode.Node):
                    this.SetPanelMode(PanelMode.Material);
                    break;
            }
        }

        public void AddModel(string name)
        {
            Node modelnode = scene.CreateChild(name);
            modelnode.Position = this.nextPosition;
            modelnode.SetScale(1f);
            modelnode.Rotation = new Quaternion(x: 0, y: 0, z: 0);
            modelnode.Name = name;
            CurrentModel = name;
            //The box material is set up as 50% texture and 50% cubemap
            userNodes.Add(CurrentModel, modelnode);

            StaticModel model1 = modelnode.CreateComponent<StaticModel>();
            model1.Model = models[name];


            MaterialHelper material1 = new MaterialHelper(materialxml["Default"]);
            material1.RefractIndex = 0.66f;
            material1.MatRefractColor = new Vector3(1.0f, 1.0f, 1.0f);
            userMaterials.Add(CurrentModel, material1);

            model1.SetMaterial(material1);

        }
        void SelectModel(string name)
        {
            CurrentModel = name;
            CameraNode.UpdatePosition();
            SetPanelMode(this.PanelMode);

        }
        void CreateScene()
        {

            // 3D scene with Octree
            scene = new Scene(Context);
            scene.CreateComponent<Octree>();

            this.AddModel("Suzanne");
            this.AddModel("Box");
            this.AddModel("Teapot");
            CurrentModel = "Box";
            //correct for teapot model's origin
            //userNodes["Teapot"].Position += new Vector3(0.0f, -0.5f, 0.0f); 

            //Button b = CreateButton(10, 10, 100, 100, "test");
            // Box




            //BoxMaterial.SetShaderParameter("MatEnvMapColor", new Vector3(1.0f, 1.0f, 1.0f));
            //BoxMaterial.SetShaderParameter("MatDiffColor", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            //Set up test material panel




            // Light
            Node lightNode = scene.CreateChild(name: "light");
            var light = lightNode.CreateComponent<Light>();
            light.Range = 20;
            light.Brightness = 0.9f;
            lightNode.Position = new Vector3(x: 5, y: 5, z: 8);


            //skybox
            var skyNode = scene.CreateChild("Sky");
            skyNode.SetScale(500.0f); // The scale actually does not matter
            var skybox = skyNode.CreateComponent<Skybox>();
            skybox.Model = ResourceCache.GetModel("Models/Box.mdl");
            skybox.SetMaterial(ResourceCache.GetMaterial("Materials/Skybox3.xml"));

            var origin = new Vector3(0.0f, 0.0f, 0.0f);
            // Camera
            CameraNode = new LookSphereNode(name: "camera", center: () => userNodes[CurrentModel].Position);
            CameraNode.Radius = 4.0f;

            scene.AddChild(CameraNode);
            Camera = CameraNode.CreateComponent<Camera>();
            SetPanelMode(PanelMode.Material);
            // Viewport
            //Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));

            //String filename = ResourceCache.GetResourceFileName("Scenes/Scene.xml");
            //Urho.IO.File file = new Urho.IO.File(Context,filename, Urho.IO.FileMode.ReadWrite);
            //scene.SaveXml(file, "\t");

            // Do actions
            //await model1Node.RunActionsAsync(new EaseBounceOut(new ScaleTo(duration: 1f, scale: 1)));
            //await boxNode.RunActionsAsync(new RepeatForever(new MoveBy(duration: 1, position: new Vector3(0.001f, 0.0f, 0.0f))));
            //await boxNode.RunActionsAsync(new RepeatForever(
            // new RotateBy(duration: 5, deltaAngleX: 0, deltaAngleY: 90, deltaAngleZ: 0)));
        }

        /// <summary>
        /// Move camera for 2D samples
        /// </summary>
        protected void SimpleMoveCamera2D(float timeStep)
        {
            // Do not move if the UI has a focused element (the console)
            if (UI.FocusElement != null)
                return;

            // Movement speed as world units per second
            const float moveSpeed = 4.0f;

            // Read WASD keys and move the camera scene node to the corresponding direction if they are pressed
            if (Input.GetKeyDown(Key.W)) CameraNode.Translate(Vector3.UnitY * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.S)) CameraNode.Translate(-Vector3.UnitY * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.A)) CameraNode.Translate(-Vector3.UnitX * moveSpeed * timeStep);
            if (Input.GetKeyDown(Key.D)) CameraNode.Translate(Vector3.UnitX * moveSpeed * timeStep);

            if (Input.GetKeyDown(Key.PageUp))
            {
                Camera camera = CameraNode.GetComponent<Camera>();
                camera.Zoom = camera.Zoom * 1.01f;
            }

            if (Input.GetKeyDown(Key.PageDown))
            {
                Camera camera = CameraNode.GetComponent<Camera>();
                camera.Zoom = camera.Zoom * 0.99f;
            }

            CameraNode.LookAt(CameraNode.CenterPosition(), Vector3.UnitY, TransformSpace.World);

            //NOTE:
            //using this function is a bit of a hack, because the CameraNode is really a LookSphereNode,
            //and is locked to the sphere around CameraNode.CenterPosition.
        }


        protected void MoveCameraByTouches(float timeStep)
        {
            if (UI.FocusElement != null)
                return;

            if (!TouchEnabled || CameraNode == null)
                return;
            //if(Input.NumTouches == 0)
            //{
            //    if (Pitch > 0) Pitch = MathHelper.Clamp(Pitch - (0.01f * timeStep), 0, 100);
            //    if (Yaw > 0) Yaw = MathHelper.Clamp(Yaw - (0.01f * timeStep), 0, 100);
            //}

            var input = Input;
            for (uint i = 0, num = input.NumTouches; i < num; ++i)
            {
                TouchState state = input.GetTouch(i);
                if (state.TouchedElement != null)
                    continue;
                if (state.Position.Y < this.CurrentPanel.Height) continue;
                if (state.Pressure > 0.0f)
                {
                    var camera = CameraNode.GetComponent<Camera>();
                    if (camera == null)
                        return;

                    var graphics = Graphics;
                    float X = (float)state.Position.X - (graphics.Width / 2);
                    float Y = (float)state.Position.Y - this.CurrentPanel.Height - ((graphics.Height - this.CurrentPanel.Height) / 2);
                    //Yaw += TouchSensitivity * camera.Fov / graphics.Height * state.Delta.X;
                    //Pitch += TouchSensitivity * camera.Fov / graphics.Height * state.Delta.Y;
                    float value = 0.01f;
                    //bool A = state.Position.X > state.Position.Y;
                    //bool B = state.Position.X + state.Position.Y < (graphics.Width + graphics.Height) / 2;
                    CameraNode.Translate(Vector3.UnitX * X * value * timeStep);
                    CameraNode.Translate(Vector3.UnitY * -Y * value * timeStep);

                    //if (A && B)
                    //CameraNode.Translate(Vector3.UnitY * value * timeStep);
                    ////else if( !A && !B )
                    //    CameraNode.Translate(-Vector3.UnitY * value * timeStep);
                    ////else if (A && !B)
                    //    CameraNode.Translate(Vector3.UnitX * value * timeStep);
                    //else if (!A && B)
                    //    CameraNode.Translate(-Vector3.UnitX * value * timeStep);


                    //CameraNode.Rotation = new Quaternion(Pitch, Yaw, 0);
                    //CameraNode.Phi -= Pitch * timeStep;
                    //CameraNode.Theta -= Yaw * timeStep;
                }
                else
                {
                    var cursor = UI.Cursor;
                    if (cursor != null && cursor.Visible)
                    {
                        IntVector2 delta = cursor.Position - state.Position;
                        cursor.Position = state.Position;
                        CameraNode.Phi -= delta.X * 0.01f;
                        CameraNode.Theta -= delta.Y * 0.01f;
                    }
                }

            }
            CameraNode.LookAt(CameraNode.CenterPosition(), Vector3.UnitY, TransformSpace.World);
        }



        protected void SimpleCreateInstructions(string text = "")
        {
            var textElement = new Text()
            {
                Value = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            textElement.SetFont(ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"), 25);
            UI.Root.AddChild(textElement);
        }




        void SetWindowAndTitleIcon()
        {

            var icon = ResourceCache.GetImage("Textures/UrhoIcon.png");
            Graphics.SetWindowIcon(icon);
            Graphics.WindowTitle = "UrhoSharp Sample";
        }

        void CreateConsoleAndDebugHud()
        {
            var xml = ResourceCache.GetXmlFile("UI/DefaultStyle.xml");
            Console = Engine.CreateConsole();
            Console.DefaultStyle = xml;
            Console.Background.Opacity = 0.8f;

            DebugHud = Engine.CreateDebugHud();
            DebugHud.DefaultStyle = xml;
        }

        void HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Esc:
                    Exit();
                    return;
                case Key.F1:
                    Console.Toggle();
                    return;
                case Key.F2:
                    DebugHud.ToggleAll();
                    return;
                case Key.Up:
                    PreviousPanelMode();
                    return;
                case Key.Down:
                    SetPanelMode(PanelMode.Node);
                    return;

            }

            var renderer = Renderer;
            switch (e.Key)
            {
                //    case Key.N1:
                //        var quality = renderer.TextureQuality;
                //        ++quality;
                //        if (quality > 2)
                //            quality = 0;
                //        renderer.TextureQuality = quality;
                //        break;

                //    case Key.N2:
                //        var mquality = renderer.MaterialQuality;
                //        ++mquality;
                //        if (mquality > 2)
                //            mquality = 0;
                //        renderer.MaterialQuality = mquality;
                //        break;

                //    case Key.N3:
                //        renderer.SpecularLighting = !renderer.SpecularLighting;
                //        break;

                //    case Key.N4:
                //        renderer.DrawShadows = !renderer.DrawShadows;
                //        break;

                //    case Key.N5:
                //        var shadowMapSize = renderer.ShadowMapSize;
                //        shadowMapSize *= 2;
                //        if (shadowMapSize > 2048)
                //            shadowMapSize = 512;
                //        renderer.ShadowMapSize = shadowMapSize;
                //        break;

                //    // shadow depth and filtering quality
                //    case Key.N6:
                //        var q = (int)renderer.ShadowQuality++;
                //        if (q > 3)
                //            q = 0;
                //        renderer.ShadowQuality = (ShadowQuality)q;
                //        break;

                //    // occlusion culling
                //    case Key.N7:
                //        var o = !(renderer.MaxOccluderTriangles > 0);
                //        renderer.MaxOccluderTriangles = o ? 5000 : 0;
                //        break;

                //    // instancing
                //    case Key.N8:
                //        renderer.DynamicInstancing = !renderer.DynamicInstancing;
                //        break;

                //    case Key.N9:
                //        Image screenshot = new Image();
                //        Graphics.TakeScreenShot(screenshot);
                //        screenshot.SavePNG(FileSystem.ProgramDir + $"Data/Screenshot_{GetType().Name}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)}.png");
                //        break;
            }
        }

        void InitTouchInput()
        {
            TouchEnabled = true;
            var layout = ResourceCache.GetXmlFile("UI/ScreenJoystick_Samples.xml");
            if (!string.IsNullOrEmpty(JoystickLayoutPatch))
            {
                XmlFile patchXmlFile = new XmlFile();
                patchXmlFile.FromString(JoystickLayoutPatch);
                layout.Patch(patchXmlFile);
            }
            var screenJoystickIndex = Input.AddScreenJoystick(layout, ResourceCache.GetXmlFile("UI/DefaultStyle.xml"));
            Input.SetScreenJoystickVisible(screenJoystickIndex, true);
        }



    }
}
