
// this code was pieced from another codebase, probably has an MIT attached, don't release

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Xna.Framework.Graphics;
using DevTools.GraphicsControls.Boiler;

namespace DevTools.GraphicsControls
{
    class GraphicsControlBase : HwndHost
    {
        //it's hilarious when under the hood windows primitives poke their head out
        private const string windowClass = "GraphicsControlBaseHostWindowClass";
        private IntPtr hWnd;

        private GraphicsDeviceService graphicsService;
        private ServiceContainer services = new ServiceContainer();


        private bool applicationHasFocus = false;
        private bool mouseInWindow = false;
        private HwndMouseState mouseState = new HwndMouseState();
        private bool isMouseCaptured = false;

        //one is screen, the other is window
        private int capturedMouseX;
        private int capturedMouseY;
        private int capturedMouseClientX;
        private int capturedMouseClientY;

        public event EventHandler<LoadContentArgs> LoadContent;
        public event EventHandler<GraphicsDeviceEventArgs> RenderXna;
        public event EventHandler<HwndMouseEventArgs> HwndLButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndLButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndLButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndRButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndMButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndX1ButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDown;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonUp;
        public event EventHandler<HwndMouseEventArgs> HwndX2ButtonDblClick;
        public event EventHandler<HwndMouseEventArgs> HwndMouseMove;
        public event EventHandler<HwndMouseEventArgs> HwndMouseEnter;
        public event EventHandler<HwndMouseEventArgs> HwndMouseLeave;

        public GraphicsControlBase()
        {
            Loaded += new RoutedEventHandler(XnaWindowHost_Loaded);

            SizeChanged += new SizeChangedEventHandler(XnaWindowHost_SizeChanged);
            Application.Current.Activated += new EventHandler(Current_Activated);
            Application.Current.Deactivated += new EventHandler(Current_Deactivated);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        protected override void Dispose(bool disposing)
        {
            if (graphicsService != null)
            {
                graphicsService.Release(disposing);
                graphicsService = null;
            }

            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            base.Dispose(disposing);
        }

        public new void CaptureMouse()
        {
            if (isMouseCaptured)
            {
                return;
            }

            NativeBinaries.ShowCursor(false);
            isMouseCaptured = true;

            NativeBinaries.POINT p = new NativeBinaries.POINT();
            NativeBinaries.GetCursorPos(ref p);
            capturedMouseX = p.X;
            capturedMouseY = p.Y;

            NativeBinaries.ScreenToClient(hWnd, ref p);
            capturedMouseClientX = p.X;
            capturedMouseClientY = p.Y;
        }

        public new void ReleaseMouseCapture()
        {
            if (!isMouseCaptured)
            {
                return;
            }

            NativeBinaries.ShowCursor(true);
            isMouseCaptured = false;
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (isMouseCaptured && (int)mouseState.Position.X != capturedMouseX && (int)mouseState.Position.Y != capturedMouseY)
            {
                NativeBinaries.SetCursorPos(capturedMouseX, capturedMouseY);
                mouseState.Position = mouseState.PreviousPosition = new Point(capturedMouseClientX, capturedMouseClientY);
            }

            if (graphicsService == null)
            {
                return;
            }

            int width = (int)ActualWidth;
            int height = (int)ActualHeight;

            if (width < 1 || height < 1)
            {
                return;
            }

            Viewport viewport = new Viewport(0, 0, width, height);
            graphicsService.GraphicsDevice.Viewport = viewport;

            if (RenderXna != null)
            {
                RenderXna(this, new GraphicsDeviceEventArgs(graphicsService.GraphicsDevice));
            }
            graphicsService.GraphicsDevice.Present(viewport.Bounds, null, hWnd);
        }

        void XnaWindowHost_Loaded(object sender, RoutedEventArgs e)
        {
            if (graphicsService == null)
            {
                graphicsService = GraphicsDeviceService.AddRef(hWnd, (int)ActualWidth, (int)ActualHeight);
                services.AddService<IGraphicsDeviceService>(graphicsService);

                if (LoadContent != null)
                {
                    LoadContent(this, new LoadContentArgs(graphicsService.GraphicsDevice, graphicsService, services));
                }
            }
        }

        void XnaWindowHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (graphicsService != null)
            {
                graphicsService.ResetDevice((int)ActualWidth, (int)ActualHeight);
            }
        }

        void Current_Activated(object sender, EventArgs e)
        {
            applicationHasFocus = true;
        }

        void Current_Deactivated(object sender, EventArgs e)
        {
            applicationHasFocus = false;
            ResetMouseState();

            if (mouseInWindow)
            {
                mouseInWindow = false;
                if (HwndMouseLeave != null)
                    HwndMouseLeave(this, new HwndMouseEventArgs(mouseState));
            }

            ReleaseMouseCapture();
        }

        private void ResetMouseState()
        {
            bool fireL = mouseState.LeftButton == MouseButtonState.Pressed;
            bool fireM = mouseState.MiddleButton == MouseButtonState.Pressed;
            bool fireR = mouseState.RightButton == MouseButtonState.Pressed;
            mouseState.LeftButton = MouseButtonState.Released;
            mouseState.MiddleButton = MouseButtonState.Released;
            mouseState.RightButton = MouseButtonState.Released;

            HwndMouseEventArgs args = new HwndMouseEventArgs(mouseState);
            if (fireL && HwndLButtonUp != null)
                HwndLButtonUp(this, args);
            if (fireM && HwndMButtonUp != null)
                HwndMButtonUp(this, args);
            if (fireR && HwndRButtonUp != null)
                HwndRButtonUp(this, args);

            mouseInWindow = false;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            hWnd = CreateHostWindow(hwndParent.Handle);
            return new HandleRef(this, hWnd);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeBinaries.DestroyWindow(hwnd.Handle);
            hWnd = IntPtr.Zero;
        }

        private IntPtr CreateHostWindow(IntPtr hWndParent)
        {
            RegisterWindowClass();

            return NativeBinaries.CreateWindowEx(0, windowClass, "",
               NativeBinaries.WS_CHILD | NativeBinaries.WS_VISIBLE,
               0, 0, (int)Width, (int)Height, hWndParent, IntPtr.Zero, IntPtr.Zero, 0);
        }

        private void RegisterWindowClass()
        {
            NativeBinaries.WNDCLASSEX wndClass = new NativeBinaries.WNDCLASSEX();
            wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
            wndClass.hInstance = NativeBinaries.GetModuleHandle(null);
            wndClass.lpfnWndProc = NativeBinaries.DefaultWindowProc;
            wndClass.lpszClassName = windowClass;
            wndClass.hCursor = NativeBinaries.LoadCursor(IntPtr.Zero, NativeBinaries.IDC_ARROW);

            NativeBinaries.RegisterClassEx(ref wndClass);
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //don't hate me ^_^
            switch (msg)
            {
                case NativeBinaries.WM_LBUTTONDOWN:
                    {
                        mouseState.LeftButton = MouseButtonState.Pressed;
                        if (HwndLButtonDown != null)
                        {
                            HwndLButtonDown(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_LBUTTONUP:
                    {
                        mouseState.LeftButton = MouseButtonState.Released;
                        if (HwndLButtonUp != null)
                        {
                            HwndLButtonUp(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_LBUTTONDBLCLK:
                    {
                        if (HwndLButtonDblClick != null)
                        {
                            HwndLButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Left));
                        }
                    } break;
                case NativeBinaries.WM_RBUTTONDOWN:
                    {
                        mouseState.RightButton = MouseButtonState.Pressed;
                        if (HwndRButtonDown != null)
                        {
                            HwndRButtonDown(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_RBUTTONUP:
                    {
                        mouseState.RightButton = MouseButtonState.Released;
                        if (HwndRButtonUp != null)
                        {
                            HwndRButtonUp(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_RBUTTONDBLCLK:
                    {
                        if (HwndRButtonDblClick != null)
                        {
                            HwndRButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Right));
                        }
                    } break;
                case NativeBinaries.WM_MBUTTONDOWN:
                    {
                        mouseState.MiddleButton = MouseButtonState.Pressed;
                        if (HwndMButtonDown != null)
                        {
                            HwndMButtonDown(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_MBUTTONUP:
                    {
                        mouseState.MiddleButton = MouseButtonState.Released;
                        if (HwndMButtonUp != null)
                        {
                            HwndMButtonUp(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
                case NativeBinaries.WM_MBUTTONDBLCLK:
                    {
                        if (HwndMButtonDblClick != null)
                        {
                            HwndMButtonDblClick(this, new HwndMouseEventArgs(mouseState, MouseButton.Middle));
                        }
                    } break;
                case NativeBinaries.WM_MOUSEMOVE:
                    {
                        if (!applicationHasFocus)
                        {
                            break;
                        }

                        mouseState.PreviousPosition = mouseState.Position;
                        mouseState.Position = new Point(
                            NativeBinaries.GetXLParam((int)lParam),
                            NativeBinaries.GetYLParam((int)lParam));

                        if (!mouseInWindow)
                        {
                            mouseInWindow = true;
                            mouseState.PreviousPosition = mouseState.Position;

                            if (HwndMouseEnter != null)
                            {
                                HwndMouseEnter(this, new HwndMouseEventArgs(mouseState));
                            }

                            NativeBinaries.TRACKMOUSEEVENT tme = new NativeBinaries.TRACKMOUSEEVENT();
                            tme.cbSize = Marshal.SizeOf(typeof(NativeBinaries.TRACKMOUSEEVENT));
                            tme.dwFlags = NativeBinaries.TME_LEAVE;
                            tme.hWnd = hwnd;
                            NativeBinaries.TrackMouseEvent(ref tme);
                        }

                        if (mouseState.Position != mouseState.PreviousPosition)
                        {
                            if (HwndMouseMove != null)
                            {
                                HwndMouseMove(this, new HwndMouseEventArgs(mouseState));
                            }
                        }

                    } break;
                case NativeBinaries.WM_MOUSELEAVE:
                    {
                        if (isMouseCaptured)
                        {
                            break;
                        }

                        ResetMouseState();

                        if (HwndMouseLeave != null)
                        {
                            HwndMouseLeave(this, new HwndMouseEventArgs(mouseState));
                        }
                    } break;
            }

            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }
    }

    public class LoadContentArgs : EventArgs
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public GraphicsDeviceService GraphicsService { get; private set; }
        public ServiceContainer Services;

        public LoadContentArgs(GraphicsDevice device, GraphicsDeviceService graphicsService, ServiceContainer services)
        {
            GraphicsDevice = device;
            GraphicsService = graphicsService;
            Services = services;
        }
    }
}
