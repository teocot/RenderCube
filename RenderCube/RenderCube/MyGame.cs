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

//Stage 1 Created by thinkyfish at freelancer for teodor5


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
    public class MyGame : Application
    {

        Scene scene;

        UrhoConsole console;
        DebugHud debugHud;
        Sprite logoSprite;
        UI ui;
        Dictionary<String, String> materials = new Dictionary<String, String>
        {
            {"SphereMaterial", "Materials/SphereMaterial.xml" },
            {"BoxMaterial", "Materials/BoxMaterial.xml" },
            {"TeapotMaterial", "Materials/TeapotMaterial.xml" }
        };

        protected const float TouchSensitivity = 2;
        protected float Yaw { get; set; }
        protected float Pitch { get; set; }
        protected bool TouchEnabled { get; set; }
        protected LookSphereNode CameraNode { get; set; }
        protected MonoDebugHud MonoDebugHud { get; set; }

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
        protected string JoystickLayoutPatch => JoystickLayoutPatches.WithZoomInAndOut;

        protected override void OnUpdate(float timeStep)
        {

            //MoveCameraByTouches(timeStep);
            base.OnUpdate(timeStep);

            SimpleMoveCamera2D(timeStep);

        }


        void CreateScene()
        {

            // 3D scene with Octree
            scene = new Scene(Context);
            scene.CreateComponent<Octree>();


            //Button b = CreateButton(10, 10, 100, 100, "test");
            // Box	
            Node model1Node = scene.CreateChild(name: "Box node");
            model1Node.Position = new Vector3(x: 0, y: 0, z: 0.8f);
            model1Node.SetScale(1f);
            model1Node.Rotation = new Quaternion(x: 0, y: 90, z: 0);

            Node model2Node = scene.CreateChild(name: "Sphere node");
            model2Node.Position = new Vector3(x: 0, y: 0, z: -0.8f);
            model2Node.SetScale(1f);
            model2Node.Rotation = new Quaternion(x: 0, y: 0, z: 0);

            //The box material is set up as 50% texture and 50% cubemap
            Material BoxMaterial = ResourceCache.GetMaterial("Materials/BoxMaterial.xml");
            StaticModel model1 = model1Node.CreateComponent<StaticModel>();
            model1.Model = ResourceCache.GetModel("Models/Teapot.mdl");
            model1.SetMaterial(BoxMaterial);
            //BoxMaterial.SetShaderParameter("MatEnvMapColor", new Vector3(1.0f, 1.0f, 1.0f));
            //BoxMaterial.SetShaderParameter("MatDiffColor", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));


            //The shere material is set up exactly the same as the box, 50/50.
            Material SphereMaterial = ResourceCache.GetMaterial("Materials/SphereMaterial.xml");
            StaticModel model2 = model2Node.CreateComponent<StaticModel>();
            model2.Model = ResourceCache.GetModel("Models/Sphere.mdl");
            model2.SetMaterial(SphereMaterial);

            //Set up test material panel
            MaterialPanel mp = new MaterialPanel(UI.Root, SphereMaterial, ResourceCache);
            mp.Visible = true;

             

            //yes, we can change things up on the fly, giving the sphere 100% refract color, and no texture/cubemap
            SphereMaterial.SetShaderParameter("RefractIndex", 0.66f);
            SphereMaterial.SetShaderParameter("RefractColor", new Vector3(1.0f, 1.0f, 1.0f));
            SphereMaterial.SetShaderParameter("MatEnvMapColor", new Vector3(0.0f, 0.0f, 0.0f));
            SphereMaterial.SetShaderParameter("MatDiffColor", new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

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
            CameraNode = new LookSphereNode(name: "camera", center: () => origin);
            CameraNode.Radius = 4.0f;
            //CameraNode.Phi = 3.14f/2.0f;
            scene.AddChild(CameraNode);
            Camera camera = CameraNode.CreateComponent<Camera>();

            // Viewport
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
            
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

        void InitWindow()
        {
        //    // Create the Window and add it to the UI's root node
        //    window = new Window();
        //    uiRoot.AddChild(window);

        //    // Set Window size and layout settings
        //    window.SetMinSize(384, 192);
        //    window.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
        //    window.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
        //    window.Name = "Window";

        //    // Create Window 'titlebar' container
        //    UIElement titleBar = new UIElement();
        //    titleBar.SetMinSize(0, 24);
        //    titleBar.VerticalAlignment = VerticalAlignment.Top;
        //    titleBar.LayoutMode = LayoutMode.Horizontal;

        //    // Create the Window title Text
        //    var windowTitle = new Text();
        //    windowTitle.Name = "WindowTitle";
        //    windowTitle.Value = "Hello GUI!";

        //    // Create the Window's close button
        //    Button buttonClose = new Button();
        //    buttonClose.Name = "CloseButton";

        //    // Add the controls to the title bar
        //    titleBar.AddChild(windowTitle);
        //    titleBar.AddChild(buttonClose);

        //    // Add the title bar to the Window
        //    window.AddChild(titleBar);

        //    // Apply styles
        //    window.SetStyleAuto(null);
        //    windowTitle.SetStyleAuto(null);
        //    buttonClose.SetStyle("CloseButton", null);

        //    buttonClose.SubscribeToReleased(_ => Exit());

        //    // Subscribe also to all UI mouse clicks just to see where we have clicked
        //    UI.SubscribeToUIMouseClick(HandleControlClicked);
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

                if (state.Delta.X != 0 || state.Delta.Y != 0)
                {
                    var camera = CameraNode.GetComponent<Camera>();
                    if (camera == null)
                        return;

                    var graphics = Graphics;

                    //Yaw += TouchSensitivity * camera.Fov / graphics.Height * state.Delta.X;
                    //Pitch += TouchSensitivity * camera.Fov / graphics.Height * state.Delta.Y;
                    Pitch = (graphics.Height / 2) - state.Position.Y;
                    Yaw = (graphics.Width / 2) - state.Position.X;
                    //CameraNode.Rotation = new Quaternion(Pitch, Yaw, 0);
                    CameraNode.Phi -= Pitch * timeStep * 0.01f;
                    CameraNode.Theta -= Yaw * timeStep * 0.01f;
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

        Slider CreateSlider(int x, int y, int xSize, int ySize, string text)
        {
            UIElement root = UI.Root;
            ResourceCache cache = ResourceCache;
            Font font = cache.GetFont("Fonts/Font.ttf");
            // Create text and slider below it
            Text sliderText = new Text();
            root.AddChild(sliderText);
            sliderText.SetPosition(x, y);
            sliderText.SetFont(font, 12);
            sliderText.Value = text;

            Slider slider = new Slider();
            root.AddChild(slider);
            slider.SetStyleAuto(null);
            slider.SetPosition(x, y + 20);
            slider.SetSize(xSize, ySize);
            // Use 0-1 range for controlling sound/music master volume
            slider.Range = 1.0f;

            return slider;
        }
        Button CreateButton(int x, int y, int xSize, int ySize, string text)
        {
            UIElement root = UI.Root;
            var cache = ResourceCache;
            Font font = cache.GetFont("Fonts/Font.ttf");

            // Create the button and center the text onto it
            Button button = new Button();
            root.AddChild(button);
            button.SetStyleAuto(null);
            button.SetPosition(x, y);
            button.SetSize(xSize, ySize);

            Text buttonText = new Text();
            button.AddChild(buttonText);
            buttonText.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            buttonText.SetFont(font, 20);
            buttonText.Value = text;

            return button;
        }


        //void CreateLogo()
        //{

        //    var logoTexture = ResourceCache.GetTexture2D("Textures/LogoLarge.png");

        //    if (logoTexture == null)
        //        return;

        //    ui = UI;
        //    logoSprite = ui.Root.CreateSprite();
        //    logoSprite.Texture = logoTexture;
        //    int w = logoTexture.Width;
        //    int h = logoTexture.Height;
        //    logoSprite.SetScale(256.0f / w);
        //    logoSprite.SetSize(w, h);
        //    logoSprite.SetHotSpot(0, h);
        //    logoSprite.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
        //    logoSprite.Opacity = 0.75f;
        //    logoSprite.Priority = -100;
        //}

        void SetWindowAndTitleIcon()
        {

            var icon = ResourceCache.GetImage("Textures/UrhoIcon.png");
            Graphics.SetWindowIcon(icon);
            Graphics.WindowTitle = "UrhoSharp Sample";
        }

        void CreateConsoleAndDebugHud()
        {
            var xml = ResourceCache.GetXmlFile("UI/DefaultStyle.xml");
            console = Engine.CreateConsole();
            console.DefaultStyle = xml;
            console.Background.Opacity = 0.8f;

            debugHud = Engine.CreateDebugHud();
            debugHud.DefaultStyle = xml;
        }

        void HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Esc:
                    Exit();
                    return;
                case Key.F1:
                    console.Toggle();
                    return;
                case Key.F2:
                    debugHud.ToggleAll();
                    return;
                case Key.Up:
                    return;
              
            }

            var renderer = Renderer;
            switch (e.Key)
            {
                case Key.N1:
                    var quality = renderer.TextureQuality;
                    ++quality;
                    if (quality > 2)
                        quality = 0;
                    renderer.TextureQuality = quality;
                    break;

                case Key.N2:
                    var mquality = renderer.MaterialQuality;
                    ++mquality;
                    if (mquality > 2)
                        mquality = 0;
                    renderer.MaterialQuality = mquality;
                    break;

                case Key.N3:
                    renderer.SpecularLighting = !renderer.SpecularLighting;
                    break;

                case Key.N4:
                    renderer.DrawShadows = !renderer.DrawShadows;
                    break;

                case Key.N5:
                    var shadowMapSize = renderer.ShadowMapSize;
                    shadowMapSize *= 2;
                    if (shadowMapSize > 2048)
                        shadowMapSize = 512;
                    renderer.ShadowMapSize = shadowMapSize;
                    break;

                // shadow depth and filtering quality
                case Key.N6:
                    var q = (int)renderer.ShadowQuality++;
                    if (q > 3)
                        q = 0;
                    renderer.ShadowQuality = (ShadowQuality)q;
                    break;

                // occlusion culling
                case Key.N7:
                    var o = !(renderer.MaxOccluderTriangles > 0);
                    renderer.MaxOccluderTriangles = o ? 5000 : 0;
                    break;

                // instancing
                case Key.N8:
                    renderer.DynamicInstancing = !renderer.DynamicInstancing;
                    break;

                case Key.N9:
                    Image screenshot = new Image();
                    Graphics.TakeScreenShot(screenshot);
                    screenshot.SavePNG(FileSystem.ProgramDir + $"Data/Screenshot_{GetType().Name}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)}.png");
                    break;
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
