using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility.Input
{
    public enum MouseButton { Left, Right }
    public enum Trigger { Left, Right }

    public interface IPressable
    {
        bool IsBeingPressed();
    }

    public interface IHeldDownable
    {
        bool IsDown();
    }

    public interface IReleaseable
    {
        bool IsReleased();
    }

    public class InputKey : IPressable, IHeldDownable, IReleaseable
    {
        private Keys Key;
        public InputKey(Keys key)
        {
            Key = key;
        }

        public bool IsBeingPressed()
        {
            return InputHandler.KeyPressed(Key);
        }

        public bool IsDown()
        {
            return InputHandler.KeyDown(Key);
        }

        public bool IsReleased()
        {
            return InputHandler.KeyReleased(Key);
        }
    }
    
    public class InputMouseButton : IPressable, IHeldDownable, IReleaseable
    {
        private MouseButton Button;
        public InputMouseButton(MouseButton button)
        {
            Button = button;
        }

        public bool IsBeingPressed()
        {
            if (Button == MouseButton.Left)
            {
                return InputHandler.MouseLeftPressed();
            }
            else
            {
                return InputHandler.MouseRightPressed();
            }
        }

        public bool IsDown()
        {
            if (Button == MouseButton.Left)
            {
                return InputHandler.MouseLeftDown();
            }
            else
            {
                return InputHandler.MouseRightDown();
            }
        }

        public bool IsReleased()
        {
            if (Button == MouseButton.Left)
            {
                return InputHandler.MouseLeftReleased();
            }
            else
            {
                return InputHandler.MouseRightReleased();
            }
        }
    }

    public class InputTrigger : IPressable, IHeldDownable, IReleaseable
    {
        private Trigger Trigger;
        private PlayerIndex PlayerIndex;

        public InputTrigger(Trigger trigger, PlayerIndex playerIndex)
        {
            Trigger = trigger;
            PlayerIndex = playerIndex;
        }

        public bool IsBeingPressed()
        {
            if (Trigger == Trigger.Left)
            {
                return InputHandler.LeftTriggerPressed(PlayerIndex);
            }
            else
            {
                return InputHandler.RightTriggerPressed(PlayerIndex);
            }
        }

        public bool IsDown()
        {
            if (Trigger == Trigger.Left)
            {
                return InputHandler.LeftTriggerDown(PlayerIndex);
            }
            else
            {
                return InputHandler.RightTriggerDown(PlayerIndex);
            }
        }

        public bool IsReleased()
        {
            if (Trigger == Trigger.Left)
            {
                return InputHandler.LeftTriggerReleased(PlayerIndex);
            }
            else
            {
                return InputHandler.RightTriggerReleased(PlayerIndex);
            }
        }
    }

    public class InputGamepadButton : IPressable, IHeldDownable, IReleaseable
    {
        private Buttons Button;
        private PlayerIndex PlayerIndex;

        public InputGamepadButton(Buttons button, PlayerIndex playerIndex)
        {
            Button = button;
            PlayerIndex = playerIndex;
        }

        public bool IsBeingPressed()
        {
            return InputHandler.ButtonPressed(Button, PlayerIndex);
        }

        public bool IsDown()
        {
            return InputHandler.ButtonDown(Button, PlayerIndex);
        }

        public bool IsReleased()
        {
            return InputHandler.ButtonReleased(Button, PlayerIndex);
        }
    }
}
