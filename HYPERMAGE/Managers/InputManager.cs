using HYPERMAGE.Spells;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HYPERMAGE.Managers
{
    public static class InputManager

    {
        private static MouseState oldMouse;
        public static bool Clicked { get; private set; }
        public static bool RightClicked { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static bool LeftMouseDown { get; private set; }
        public static bool RightMouseDown { get; private set; }

        private static Vector2 direction;
        public static Vector2 lastDirection = new(0, 1);
        public static Vector2 Direction => direction;
        public static bool Moving => direction != Vector2.Zero;

        public static bool buttonClicked;
        public static bool buttonClickedHold;

        public static bool dashing;
        public static bool slow;

        public static float dashCooldown = 100;

        public static Keys up = Keys.W;
        public static Keys down = Keys.S;
        public static Keys left = Keys.A;
        public static Keys right = Keys.D;

        public static Keys dash = Keys.Space;
        public static Keys sneak = Keys.LeftShift;

        public static void Update()
        {
            var ms = Mouse.GetState();

            LeftMouseDown = ms.LeftButton == ButtonState.Pressed;
            RightMouseDown = ms.RightButton == ButtonState.Pressed;

            Clicked = oldMouse.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed;
            RightClicked = oldMouse.RightButton == ButtonState.Released && ms.RightButton == ButtonState.Pressed;
            MousePosition = new Vector2(ms.Position.X, ms.Position.Y) / new Vector2(1920 / 320, 1080 / 180);

            oldMouse = ms;

            direction = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.GetPressedKeyCount() > 0)
            {
                if (keyboardState.IsKeyDown(left)) direction.X--;
                if (keyboardState.IsKeyDown(right)) direction.X++;
                if (keyboardState.IsKeyDown(up)) direction.Y--;
                if (keyboardState.IsKeyDown(down)) direction.Y++;

                if (direction != Vector2.Zero)
                {
                    lastDirection = direction;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Space) && dashCooldown > 100f)
            {
                dashCooldown = 0;
                dashing = true;
            }

            if (keyboardState.IsKeyDown(sneak))
            {
                slow = true;
            }

            else
            {
                slow = false;
            }

            if (buttonClicked && LeftMouseDown)
            {
                buttonClickedHold = true;
            }

            if (buttonClickedHold && LeftMouseDown)
            {
                buttonClicked = true;
            }

            else
            {
                buttonClicked = false;
                buttonClickedHold = false;
            }

            dashCooldown++;
        }
    }
}