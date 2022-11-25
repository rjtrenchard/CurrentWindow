using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CurrentWindow
{
    class Program
    {
        const int POLLRATE = 500;
        
        static void Main(string[] args)
        {
            int counter = 0;
            string activeWindow, lastWindow = "";

            while (true) {
                activeWindow = GetActiveWindowTitle();
                if (activeWindow != null)
                {
                    if (!activeWindow.Equals(lastWindow))
                    {
                        lastWindow = activeWindow;
                        System.Console.WriteLine(activeWindow);
                    }
                } else {
                    if (lastWindow != null) {
                        // set up a counter so every time we switch windows we don't display "no handle" for the split second there is no handle.
                        counter++;
                        if (counter > 2 ) {
                            lastWindow = null;
                            counter = 0;
                            System.Console.WriteLine("No Window Handle");
                        }
                        
                    }
                }
                Thread.Sleep(POLLRATE);
            }
        }


        // solution provided from https://stackoverflow.com/a/115905

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }
}
