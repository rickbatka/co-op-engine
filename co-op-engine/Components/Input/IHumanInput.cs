using Microsoft.Xna.Framework;
using System;
namespace co_op_engine.Components.Input
{
    interface IHumanInput
    {
        Vector2 GetMovement();
        bool IsPressingRunButton();
    }
}
