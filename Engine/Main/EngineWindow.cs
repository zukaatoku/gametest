﻿using Engine.Events;
using Engine.Graphics;
using Engine.Shared;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Engine.Main
{
    public class EngineWindow : GameWindow
    {
        private Renderer _renderer;
        public event FrameRender OnFrameRender;
        public event FrameUpdate OnFrameUpdate;

        public EngineWindow()
            : base(Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT, GraphicsMode.Default, Constants.WINDOW_TITLE)
        {
            InitializeRenderer();

            RenderFrame += EngineWindow_RenderFrame;
            UpdateFrame += EngineWindow_UpdateFrame;
            Resize += EngineWindow_Resize;
        }

        private void EngineWindow_Resize(object sender, EventArgs e)
        {
            GL.Viewport(new Rectangle(0, 0, (sender as GameWindow).Width, (sender as GameWindow).Height));
        }

        private void EngineWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
            OnFrameUpdate?.Invoke(this, new Events.EventArgs.UpdateFrameEventArgs());
        }

        private double _timer = 0.0d;
        private int _fps = 0;

        private void EngineWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            if (Constants.CALC_FPS)
            {
                _timer += e.Time;
                _fps++;

                if (_timer > 1.0d)
                {
                    Console.WriteLine(_fps);
                    _fps = 0;
                    _timer = 0.0d;
                }
            }

            OnFrameRender?.Invoke(this, new Events.EventArgs.RenderFrameEventArgs(_renderer));
            _renderer.BeforeDraw();
            _renderer.DrawFrame();
            _renderer.AfterDraw();
            (sender as GameWindow).SwapBuffers();
        }

        public Renderer Renderer => _renderer;

        public void RunEngine()
        {
            //VSync = VSyncMode.Off;
            //WindowState = WindowState.Fullscreen;
            //Run();
            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine("SHADER: " + GL.GetString(StringName.ShadingLanguageVersion));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Run(Constants.UPDATES_PER_SECOND, Constants.FRAMES_PER_SECOND);
        }

        private void InitializeRenderer()
        {
            _renderer = new Renderer();
        }
    }
}