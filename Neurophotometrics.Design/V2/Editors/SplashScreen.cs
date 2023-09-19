using System;
using System.Threading;
using System.Windows.Forms;

namespace Neurophotometrics.Design.V2.Editors
{
    public partial class SplashScreen : Form
    {
        private static Thread _splashThread;
        private static SplashScreen _splashScreen;

        public SplashScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show the Splash Screen (Loading...)
        /// </summary>
        public static void ShowSplash()
        {
            try
            {
                if (_splashThread == null)
                {
                    // Show the form in a new thread
                    _splashThread = new Thread(new ThreadStart(DoShowSplash))
                    {
                        IsBackground = true
                    };
                    _splashThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: ShowSplash\nMessage: {ex.Message}");
            }
        }

        /// <summary>
        /// Called by _splashThread
        /// </summary>
        private static void DoShowSplash()
        {
            try
            {
                if (_splashScreen == null)
                    _splashScreen = new SplashScreen();

                // Create a new message pump on this thread (started from ShowSplash)
                Application.Run(_splashScreen);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: DoShowSplash\nMessage: {ex.Message}");
            }
        }

        /// <summary>
        /// Close the splash (Loading...) screen
        /// </summary>
        public static void CloseSplash()
        {
            try
            {
                // need to call on the thread that launched this splash
                if (_splashScreen.InvokeRequired)
                    _splashScreen.Invoke(new MethodInvoker(CloseSplash));
                else
                {
                    Application.ExitThread();
                    if (!_splashScreen.IsDisposed)
                    {
                        _splashScreen.Dispose();
                        _splashScreen = null;
                    }
                    if (_splashThread != null)
                        _splashThread = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: CloseSplash\nMessage: {ex.Message}");
            }
        }
    }
}