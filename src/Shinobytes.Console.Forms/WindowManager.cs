﻿using System.Collections.Generic;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    /// <summary>
    /// Manager class to help making sure that the right control gets keyboard focus
    /// </summary>
    internal class InputManager
    {

    }

    /// <summary>
    /// Manager class to keep all active windows updated and rendered
    /// </summary>
    internal class WindowManager
    {
        private static readonly List<Window> windows = new List<Window>();
        private static readonly object mutex = new object();

        public static void Register(Window window)
        {
            lock (mutex) windows.Add(window);
        }

        public static void Draw(ConsoleGraphics graphics, AppTime appTime)
        {
            lock (mutex) windows
                 .Where(x => x.Visible)
                .ForEach(x => x.Draw(graphics, appTime));
        }

        public static void Update(AppTime appTime)
        {
            lock (mutex) windows
                 .Where(x => x.Enabled)
                .ForEach(x => x.Update(appTime));
        }

        public static void OnKeyDown(KeyInfo keyInfo)
        {
            lock (mutex) windows
                    .Where(x => x.HasKeyboardFocus && !x.EventBlocked())
                    .DoWhile(x => x.OnKeyDown(keyInfo));
        }
        
        public static void Unregister(Window window)
        {
            lock (mutex) windows.Remove(window);
        }
    }
}