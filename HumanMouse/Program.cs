using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;

    private static bool isRunning = true;

    static void Main(string[] args)
    {
        Console.WriteLine("Press Esc to stop.");

        // Start a separate thread for mouse movement simulation
        Thread mouseThread = new Thread(MouseMovementThread);
        mouseThread.Start();

        // Wait for Esc key press
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    isRunning = false;
                    break;
                }
            }
        }

        // Wait for the mouse movement thread to finish
        mouseThread.Join();
    }

    static void MouseMovementThread()
    {
        int screenWidth = Console.WindowWidth;
        int screenHeight = Console.WindowHeight;

        Random random = new Random();

        while (isRunning)
        {
            // Generate random coordinates for the mouse cursor
            int x = random.Next(screenWidth);
            int y = random.Next(screenHeight);

            // Move the cursor to the generated coordinates
            SetCursorPos(x, y);

            // Simulate right mouse button click with a 50% probability
            if (random.NextDouble() < 0.5)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }

            Thread.Sleep(10000); // Delay for 10 seconds
        }
    }
}