extern alias monoFrameworkAlias;
using System;
using System.Threading;
using monoFrameworkAlias.Microsoft.Xna.Framework.Graphics;

namespace DevTools.GraphicsControls.Boiler
{
    public class MonoGraphicsDeviceService : monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.IGraphicsDeviceService
    {
        private static readonly MonoGraphicsDeviceService instance = new MonoGraphicsDeviceService();
        private static int refCount;

        private monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.PresentationParameters parameters;
        public monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        private MonoGraphicsDeviceService() { }

        private void CreateDevice(IntPtr windowHandle, int width, int height)
        {
            parameters = new monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.PresentationParameters();
            parameters.BackBufferWidth = Math.Max(width, 1);
            parameters.BackBufferHeight = Math.Max(height, 1);
            parameters.BackBufferFormat = SurfaceFormat.Color;
            parameters.DepthStencilFormat = DepthFormat.Depth24;
            parameters.DeviceWindowHandle = windowHandle;
            parameters.PresentationInterval = PresentInterval.Immediate;
            parameters.IsFullScreen = false;

            this.GraphicsDevice = new monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice(
                monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter,
                monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsProfile.Reach,
                parameters);

            if (DeviceCreated != null)
            {
                DeviceCreated(this, EventArgs.Empty);
            }
        }

        public static MonoGraphicsDeviceService AddRef(IntPtr handle, int width, int height)
        {
            if (Interlocked.Increment(ref refCount) == 1)
            {
                instance.CreateDevice(handle, width, height);
            }

            return instance;
        }

        public void Release(bool disposing)
        {
            if (Interlocked.Decrement(ref refCount) == 0)
            {
                if (disposing)
                {
                    if (DeviceDisposing != null)
                        DeviceDisposing(this, EventArgs.Empty);

                    this.GraphicsDevice.Dispose();
                }

                this.GraphicsDevice = null;
            }
        }

        public void ResetDevice(int width, int height)
        {
            if (DeviceResetting != null)
            {
                DeviceResetting(this, EventArgs.Empty);
            }

            parameters.BackBufferWidth = Math.Max(parameters.BackBufferWidth, width);
            parameters.BackBufferHeight = Math.Max(parameters.BackBufferHeight, height);

            // hope this isnt important lol
            //this.GraphicsDevice.Reset(parameters);
            if (DeviceReset != null)
            {
                DeviceReset(this, EventArgs.Empty);
            }
        }
    }

    public class MonoGraphicsDeviceEventArgs : EventArgs
    {
        public monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice GraphicsDevice { get; private set; }

        public MonoGraphicsDeviceEventArgs(monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
        {
            GraphicsDevice = device;
        }
    }
}
