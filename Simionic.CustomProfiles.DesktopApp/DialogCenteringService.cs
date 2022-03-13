using System.Runtime.InteropServices;
using System.Text;

// this class adapted from Stackoverflow answer - original copyright acknowledged and used under general 
// some naming here is Win32-style because P/Invoke

public class DialogCenteringService : IDisposable
{
    private const int WH_CALLWNDPROCRET = 12;
    
    private readonly IWin32Window _owner;
    private readonly HookProc _hook;
    private readonly IntPtr _hookPtr = IntPtr.Zero;

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

    [DllImport("kernel32.dll")]
    static extern int GetCurrentThreadId();

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

    [DllImport("user32.dll")]
    private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("User32.dll")]
    public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

    [DllImport("User32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

    [DllImport("user32.dll")]
    public static extern int UnhookWindowsHookEx(IntPtr idHook);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

    [DllImport("user32.dll")]
    public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

    public static Rectangle? GetWindowRect(IntPtr hWnd)
    {
        Rectangle rectangle = new Rectangle(0, 0, 0, 0);
        bool success = GetWindowRect(hWnd, ref rectangle);

        if (!success)
        {
            return null;
        }

        return rectangle;
    }

    public static Rectangle GetCenterRectangle(Rectangle parent, Rectangle child)
    {
        int width = child.Width - child.X;
        int height = child.Height - child.Y;

        Point center = new Point(0, 0);
        center.X = parent.X + ((parent.Width - parent.X) / 2);
        center.Y = parent.Y + ((parent.Height - parent.Y) / 2);

        Point start = new Point(0, 0);
        start.X = (center.X - (width / 2));
        start.Y = (center.Y - (height / 2));

        // get centered rectangle
        Rectangle centeredRectangle = new Rectangle(start.X, start.Y, width, height);

        // fit the window to the screen
        Screen parentScreen = Screen.FromRectangle(parent);
        Rectangle workingArea = parentScreen.WorkingArea;

        // various collision checks
        if (workingArea.X > centeredRectangle.X)
        {
            centeredRectangle = new Rectangle(workingArea.X, centeredRectangle.Y, centeredRectangle.Width, centeredRectangle.Height);
        }
        if (workingArea.Y > centeredRectangle.Y)
        {
            centeredRectangle = new Rectangle(centeredRectangle.X, workingArea.Y, centeredRectangle.Width, centeredRectangle.Height);
        }
        if (workingArea.Right < centeredRectangle.Right)
        {
            centeredRectangle = new Rectangle(workingArea.Right - centeredRectangle.Width, centeredRectangle.Y, centeredRectangle.Width, centeredRectangle.Height);
        }
        if (workingArea.Bottom < centeredRectangle.Bottom)
        {
            centeredRectangle = new Rectangle(centeredRectangle.X, workingArea.Bottom - centeredRectangle.Height, centeredRectangle.Width, centeredRectangle.Height);
        }

        return centeredRectangle;
    }

    public static void CenterWindow(IntPtr childWindow, Rectangle parent)
    {
        Rectangle? child = GetWindowRect(childWindow);

        if (child == null)
        {
            return;
        }

        Rectangle centerRectangle = GetCenterRectangle(parent, child.Value);

        Task.Factory.StartNew(() => SetWindowPos(childWindow, (IntPtr)0, centerRectangle.X, centerRectangle.Y, centerRectangle.Width, centerRectangle.Height, SetWindowPosFlags.SWP_ASYNCWINDOWPOS | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOOWNERZORDER | SetWindowPosFlags.SWP_NOZORDER));
    }

    public void Dispose()
    {
        UnhookWindowsHookEx(_hookPtr);
    }

    private IntPtr DialogHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode < 0)
        {
            return CallNextHookEx(_hookPtr, nCode, wParam, lParam);
        }

        CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
        IntPtr hook = _hookPtr;

        if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
        {
            try
            {
                CenterWindow(msg.hwnd);
            }
            finally
            {
                UnhookWindowsHookEx(_hookPtr);
            }
        }

        return CallNextHookEx(hook, nCode, wParam, lParam);
    }

    private void CenterWindow(IntPtr hChildWnd)
    {
        Rectangle? recParent = GetWindowRect(_owner.Handle);

        if (recParent == null)
        {
            return;
        }

        CenterWindow(hChildWnd, recParent.Value);
    }

    public DialogCenteringService(IWin32Window owner)
    {
        if (owner == null) throw new ArgumentNullException("owner");

        _owner = owner;
        _hook = DialogHookProc;

        _hookPtr = SetWindowsHookEx(WH_CALLWNDPROCRET, _hook, IntPtr.Zero, GetCurrentThreadId());
    }

    public enum CbtHookAction : int
    {
        HCBT_MOVESIZE = 0,
        HCBT_MINMAX = 1,
        HCBT_QS = 2,
        HCBT_CREATEWND = 3,
        HCBT_DESTROYWND = 4,
        HCBT_ACTIVATE = 5,
        HCBT_CLICKSKIPPED = 6,
        HCBT_KEYSKIPPED = 7,
        HCBT_SYSCOMMAND = 8,
        HCBT_SETFOCUS = 9
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        public IntPtr lResult;
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    };

    [Flags]
    public enum SetWindowPosFlags : uint
    {
        /// <summary>
        ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        SWP_ASYNCWINDOWPOS = 0x4000,

        /// <summary>
        ///     Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        SWP_DEFERERASE = 0x2000,

        /// <summary>
        ///     Draws a frame (defined in the window's class description) around the window.
        /// </summary>
        SWP_DRAWFRAME = 0x0020,

        /// <summary>
        ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        SWP_FRAMECHANGED = 0x0020,

        /// <summary>
        ///     Hides the window.
        /// </summary>
        SWP_HIDEWINDOW = 0x0080,

        /// <summary>
        ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        SWP_NOACTIVATE = 0x0010,

        /// <summary>
        ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        SWP_NOCOPYBITS = 0x0100,

        /// <summary>
        ///     Retains the current position (ignores X and Y parameters).
        /// </summary>
        SWP_NOMOVE = 0x0002,

        /// <summary>
        ///     Does not change the owner window's position in the Z order.
        /// </summary>
        SWP_NOOWNERZORDER = 0x0200,

        /// <summary>
        ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        SWP_NOREDRAW = 0x0008,

        /// <summary>
        ///     Same as the SWP_NOOWNERZORDER flag.
        /// </summary>
        SWP_NOREPOSITION = 0x0200,

        /// <summary>
        ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        SWP_NOSENDCHANGING = 0x0400,

        /// <summary>
        ///     Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        SWP_NOSIZE = 0x0001,

        /// <summary>
        ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        SWP_NOZORDER = 0x0004,

        /// <summary>
        ///     Displays the window.
        /// </summary>
        SWP_SHOWWINDOW = 0x0040,
    }
}