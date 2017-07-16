using System;
using System.Collections;
using HidLibrary;
using SpaceTradingGame.Engine.UI.Controls;

namespace SpaceTradingGame.Engine.UI.Interfaces
{
    public class CombatScreen : Interface
    {
        public CombatScreen(InterfaceManager manager) 
            : base(manager)
        {
            //Find gamepad and set the local variable
            var deviceList = HidDevices.Enumerate();
            foreach (var device in deviceList)
            {
                if (device.Description.Equals("HID-compliant game controller"))
                {
                    gamePad = device;
                    break;
                }
            }
        }
        
        public override void DrawFrame(GameTime gameTime)
        {
            GraphicConsole.Clear();

            HidDeviceData data = gamePad.Read();
            GraphicConsole.WriteLine(data.Status);
            GraphicConsole.Write("\n\n");

            for (int i = 34; i < 34 + 5; i++)
            {
                GraphicConsole.Write(toBitString(data.Data[i]) + " ");
                //if (i % 3 == 0)
                    GraphicConsole.Write("\n");
            }

            GraphicConsole.WriteLine("\n");
            GraphicConsole.WriteLine(string.Format("          ???: {0}", data.Data[0]));
            GraphicConsole.WriteLine(string.Format("   Left Thumb: {0} | {1}", data.Data[1], data.Data[2]));
            GraphicConsole.WriteLine(string.Format("  Right Thumb: {0} | {1}", data.Data[3], data.Data[4]));
            GraphicConsole.WriteLine(string.Format(" Face Buttons: {0}", toBitString(data.Data[5])));
            GraphicConsole.WriteLine(string.Format(" Shld Buttons: {0}", toBitString(data.Data[6])));
            GraphicConsole.WriteLine(string.Format("          ???: {0}", toBitString(data.Data[7])));
            GraphicConsole.WriteLine(string.Format(" Left Trigger: {0}", data.Data[8]));
            GraphicConsole.WriteLine(string.Format("Right Trigger: {0}", data.Data[9]));

            base.DrawFrame(gameTime);
        }

        private string toBitString(byte x)
        {
            BitArray bit = new BitArray(new byte[] { x });

            string str = string.Empty;
            foreach (bool b in bit)
            {
                str += (b) ? "1" : "0";
            }

            return str;
        }

        private HidDevice gamePad;
    }
}
