using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.RawInput;
using SlimDX.Multimedia;
using System.Windows.Forms;
using Common;

namespace Pigment.Engine.Input
{
    public class Input
    {
        public Int2 MousePos { get; set; }
        public int MouseWheelDelta { get; set; }
        public Dictionary<System.Windows.Forms.Keys, bool> Pressed;

        public Input()
        {
            
            Device.RegisterDevice(UsagePage.Generic, UsageId.Keyboard, DeviceFlags.None);
            Device.KeyboardInput += new EventHandler<KeyboardInputEventArgs>(keyboardInput);
            Device.RegisterDevice(UsagePage.Generic, UsageId.Mouse, DeviceFlags.None);
            Device.MouseInput += new EventHandler<MouseInputEventArgs>(mouseInput);
            MouseWheelDelta = 0;
            MousePos = new Int2(0, 0);
            Pressed = new Dictionary<Keys, bool>();
        }

        public delegate void MouseDelegate(object sender, EventArgs e);
        
        public event MouseDelegate OnLeftMouseDown;
        public event MouseDelegate OnLeftMouseUp;

        public event MouseDelegate OnMiddleMouseDown;
        public event MouseDelegate OnMiddleMouseUp;

        public event MouseDelegate OnRightMouseDown;
        public event MouseDelegate OnRightMouseUp;

        public event MouseDelegate OnButton4MouseDown;
        public event MouseDelegate OnButton4MouseUp;

        public event MouseDelegate OnButton5MouseDown;
        public event MouseDelegate OnButton5MouseUp;

        private void keyboardInput(object sender, KeyboardInputEventArgs e)
        {
            if (e.State.HasFlag(KeyState.Pressed))
            {
                if (Pressed.ContainsKey(e.Key))
                {
                    Pressed[e.Key] = true;
                }
                else
                {
                    Pressed.Add(e.Key, true);
                }
            }
            else if (e.State.HasFlag(KeyState.Released))
            {
                if (Pressed.ContainsKey(e.Key))
                {
                    Pressed[e.Key] = false;
                }
                else
                {
                    Pressed.Add(e.Key, false);
                }
            }
        }

        private void mouseInput(object sender, MouseInputEventArgs e)
        {
            MousePos = new Int2(e.X, e.Y);
            MouseWheelDelta += e.WheelDelta;
            EventArgs args = new EventArgs();

            if (e.ButtonFlags.HasFlag(MouseButtonFlags.LeftDown))
            {
                if (OnLeftMouseDown != null)
                {
                    OnLeftMouseDown(this, args);
                }
            }
            if (e.ButtonFlags.HasFlag(MouseButtonFlags.LeftUp))
            {
                if (OnLeftMouseUp != null)
                {
                    OnLeftMouseUp(this, args);
                }
            }

            if (e.ButtonFlags.HasFlag(MouseButtonFlags.MiddleDown))
            {
                if (OnMiddleMouseDown != null)
                {
                    OnMiddleMouseDown(this, args);
                }
            }
            if (e.ButtonFlags.HasFlag(MouseButtonFlags.MiddleUp))
            {
                if (OnMiddleMouseUp != null)
                {
                    OnMiddleMouseUp(this, args);
                }
            }

            if (e.ButtonFlags.HasFlag(MouseButtonFlags.RightDown))
            {
                if (OnRightMouseDown != null)
                {
                    OnRightMouseDown(this, args);
                }
            }
            if (e.ButtonFlags.HasFlag(MouseButtonFlags.RightUp))
            {
                if (OnRightMouseUp != null)
                {
                    OnRightMouseUp(this, args);
                }
            }

            if (e.ButtonFlags.HasFlag(MouseButtonFlags.Button4Down))
            {
                if (OnButton4MouseDown != null)
                {
                    OnButton4MouseDown(this, args);
                }
            }
            if (e.ButtonFlags.HasFlag(MouseButtonFlags.Button4Up))
            {
                if (OnButton4MouseUp != null)
                {
                    OnButton4MouseUp(this, args);
                }
            }

            if (e.ButtonFlags.HasFlag(MouseButtonFlags.Button5Down))
            {
                if (OnButton5MouseDown != null)
                {
                    OnButton5MouseDown(this, args);
                }
            }
            if (e.ButtonFlags.HasFlag(MouseButtonFlags.Button5Up))
            {
                if (OnButton5MouseUp != null)
                {
                    OnButton5MouseUp(this, args);
                }
            }
        }
    }
}
