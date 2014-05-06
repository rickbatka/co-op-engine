using System;
using System.Windows.Input;
using System.Windows;

namespace DevTools.GraphicsControls.Boiler
{
    class HwndMouseState
    {
        public MouseButtonState LeftButton;
        public MouseButtonState RightButton;
        public MouseButtonState MiddleButton;
        public Point Position;
        public Point PreviousPosition;
    }

    class HwndMouseEventArgs : EventArgs
    {
        public MouseButtonState LeftButton { get; private set; }
        public MouseButtonState RightButton { get; private set; }
        public MouseButtonState MiddleButton { get; private set; }
        public MouseButton? DoubleClickButton { get; private set; }
        public int VerticalScrollDelta { get; private set; }
        public int HorizontalScrollDelta { get; private set; }
        public Point Position { get; private set; }
        public Point PreviousPosition { get; private set; }
        
        public HwndMouseEventArgs(HwndMouseState state)
        {
            LeftButton = state.LeftButton;
            RightButton = state.RightButton;
            MiddleButton = state.MiddleButton;
            Position = state.Position;
            PreviousPosition = state.PreviousPosition;
        }

        public HwndMouseEventArgs(HwndMouseState state, int mouseWheelDelta, int mouseHWheelDelta)
            : this(state)
        {
            VerticalScrollDelta = mouseWheelDelta;
            HorizontalScrollDelta = mouseHWheelDelta;
        }

        public HwndMouseEventArgs(HwndMouseState state, MouseButton doubleClickButton)
            : this(state)
        {
            DoubleClickButton = doubleClickButton;
        }
    }
}
