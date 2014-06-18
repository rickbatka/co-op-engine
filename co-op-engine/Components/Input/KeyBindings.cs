using co_op_engine.Utility;
using co_op_engine.Utility.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    public static class KeyBindings
    {
        // definitions
        public static readonly InputKey KEYBOARD_MOVE_UP = new InputKey(Keys.W);
        public static readonly InputKey KEYBOARD_MOVE_DOWN = new InputKey(Keys.S);
        public static readonly InputKey KEYBOARD_MOVE_LEFT = new InputKey(Keys.A);
        public static readonly InputKey KEYBOARD_MOVE_RIGHT = new InputKey(Keys.D);
        public static readonly InputKey KEYBOARD_ATTACK = new InputKey(Keys.Space);
        public static readonly InputKey KEYBOARD_ATTACK_ALT = new InputKey(Keys.D7);
        public static readonly InputKey KEYBOARD_RAGE = new InputKey(Keys.D8);

        public static readonly InputMouseButton MOUSE_ATTACK = new InputMouseButton(MouseButton.Left);
        public static readonly InputTrigger TRIGGER_ATTACK = new InputTrigger(Trigger.Right, PlayerIndex.One);
        public static readonly InputGamepadButton GAMEPAD_ATTACK = new InputGamepadButton(Buttons.B, PlayerIndex.One);

        // Bindings
        public static readonly IHeldDownable[] KeyboardMovementKeys = { KEYBOARD_MOVE_UP, KEYBOARD_MOVE_DOWN, KEYBOARD_MOVE_LEFT, KEYBOARD_MOVE_RIGHT };
        public static readonly IPressable[] AttackInputs = { KEYBOARD_ATTACK, KEYBOARD_ATTACK_ALT, MOUSE_ATTACK, TRIGGER_ATTACK, GAMEPAD_ATTACK };
        public static readonly IPressable[] RageInputs = { KEYBOARD_RAGE };
    }
}
