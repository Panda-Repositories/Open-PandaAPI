using System;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace PandaModuleAPI
{
    public class WeArePanda
    {
        public static string API_Current_Version = "v1.1";
        public static string exploitdllname = "Panda.dll";
        public static string luapipebased = "https://raw.githubusercontent.com/SkieAdmin/Panda-Respiratory/main/API/CurrentPipe";
        public static string puppy = "RBLXInjector.exe"; //Named of the Custom Injector you want

        

        public async static void Inject(bool isPuppyMilk = false, bool OpenPandaDiscord = true)
        {
            WebClient llo = new WebClient();
            string api_version = llo.DownloadString("https://raw.githubusercontent.com/SkieAdmin/Panda-Respiratory/main/API/API_Version");
            if (!api_version.Contains(WeArePanda.API_Current_Version))
            {
                MessageBox.Show("A Newer Version of Panda-API has been Detected, Please Redownload Open-Panda's Technology API");
                Process.Start("https://github.com/Panda-Respiratory/Open-PandaAPI");
                return;
            }
            try
            {
                if (NamedPipeExist(llo.DownloadString(WeArePanda.luapipebased)))//check if the pipe exist
                {
                    MessageBox.Show("Already injected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);//if the pipe exist that's mean that we don't need to inject
                    return;
                }
                WebClient oc = new WebClient();
                string sourceFile = oc.DownloadString("https://raw.githubusercontent.com/SkieAdmin/Panda-Respiratory/main/API/ExploitDownloadLink");
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                string destFile = path + exploitdllname;
                oc.DownloadFile(sourceFile, destFile);
                if (isPuppyMilk == true)
                {
                    /*It would use Puppy Injector in this case*/
                    if (!File.Exists(WeArePanda.puppy))
                    {
                        oc.DownloadFile("https://cdn.discordapp.com/attachments/784597168887300106/785011977973268510/PuppyMilkV3.exe", WeArePanda.puppy);
                    }
                    string filearg = AppDomain.CurrentDomain.BaseDirectory + WeArePanda.exploitdllname;
                    Process.Start(WeArePanda.puppy, filearg);
                    return;
                }
                else if (!NamedPipeExist(llo.DownloadString(WeArePanda.luapipebased)))//check if the pipe don't exist
                {
                    switch (Injector.DllInjector.GetInstance.Inject("RobloxPlayerBeta", AppDomain.CurrentDomain.BaseDirectory + WeArePanda.exploitdllname))//Process name and dll directory
                    {
                        case Injector.DllInjectionResult.DllNotFound://if can't find the dll
                            MessageBox.Show($"Couldn't find {WeArePanda.exploitdllname}", "Dll was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);//display messagebox to tell that dll was not found
                            return;
                        case Injector.DllInjectionResult.GameProcessNotFound://if can't find the process
                            MessageBox.Show("Couldn't find RobloxPlayerBeta.exe!", "Target process was not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);//display messagebox to tell that proccess was not found
                            return;
                        case Injector.DllInjectionResult.InjectionFailed://if injection fails(this don't work or only on special cases)
                            MessageBox.Show("Injection Failed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);//display messagebox to tell that injection failed
                            return;
                    }
                }
                await Task.Delay(5000);
                {
                    if (NamedPipeExist(llo.DownloadString(WeArePanda.luapipebased)))
                    {
                        if (OpenPandaDiscord == true)
                        {
                            Process.Start(llo.DownloadString("https://raw.githubusercontent.com/SkieAdmin/Panda-Respiratory/main/API/Panda_Discord"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oh Now!, A Roadblock on Injecting your Executor may had encounter some error.\n\nError Details: " + ex.ToString());
                return;
            }
        }
        public void Execute(string script)
        {
            try
            {
                WebClient llo = new WebClient();
                if (!NamedPipeExist(llo.DownloadString(WeArePanda.luapipebased)))//check if the pipe don't exist
                {
                    MessageBox.Show("Please Inject Panda-Module.");
                    return;
                }
                LuaPipe(script);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oh Now!, A Roadblock on Executing your Script may had encounter some error.\n\nError Details: " + ex.ToString());
                return;
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WaitNamedPipe(string name, int timeout);
        //function to check if the pipe exist
        public static bool NamedPipeExist(string pipeName)
        {
            try
            {
                if (!WaitNamedPipe($"\\\\.\\pipe\\{pipeName}", 0))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error == 0)
                    {
                        return false;
                    }
                    if (lastWin32Error == 2)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //lua pipe function
        public static void LuaPipe(string script)
        {
            WebClient llo = new WebClient();
            if (NamedPipeExist(llo.DownloadString(WeArePanda.luapipebased)))
            {
                new Thread(() =>//lets run this in another thread so if roblox crash the ui/gui don't freeze or something
                {
                    try
                    {
                        using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", llo.DownloadString(WeArePanda.luapipebased), PipeDirection.Out))
                        {
                            namedPipeClientStream.Connect();
                            using (StreamWriter streamWriter = new StreamWriter(namedPipeClientStream, System.Text.Encoding.Default, 999999))//changed buffer to max 1mb since default buffer is 1kb
                            {
                                streamWriter.Write(script);
                                streamWriter.Dispose();
                            }
                            namedPipeClientStream.Dispose();
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error occured connecting to the pipe.", "Connection Failed!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Please Inject the Exploit First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
    class Injector
    {
        public enum DllInjectionResult
        {
            DllNotFound,
            GameProcessNotFound,
            InjectionFailed,
            Success
        }

        public sealed class DllInjector
        {
            static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern int CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress,
                IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

            static DllInjector _instance;

            public static DllInjector GetInstance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new DllInjector();
                    }
                    return _instance;
                }
            }

            DllInjector() { }

            public DllInjectionResult Inject(string sProcName, string sDllPath)
            {
                if (!File.Exists(sDllPath))
                {
                    return DllInjectionResult.DllNotFound;
                }

                uint _procId = 0;

                Process[] _procs = Process.GetProcesses();
                for (int i = 0; i < _procs.Length; i++)
                {
                    if (_procs[i].ProcessName == sProcName)
                    {
                        _procId = (uint)_procs[i].Id;
                        break;
                    }
                }

                if (_procId == 0)
                {
                    return DllInjectionResult.GameProcessNotFound;
                }

                if (!bInject(_procId, sDllPath))
                {
                    return DllInjectionResult.InjectionFailed;
                }

                return DllInjectionResult.Success;
            }

            bool bInject(uint pToBeInjected, string sDllPath)
            {
                IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, pToBeInjected);

                if (hndProc == INTPTR_ZERO)
                {
                    return false;
                }

                IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

                if (lpLLAddress == INTPTR_ZERO)
                {
                    return false;
                }

                IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)sDllPath.Length, (0x1000 | 0x2000), 0X40);

                if (lpAddress == INTPTR_ZERO)
                {
                    return false;
                }

                byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);

                if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
                {
                    return false;
                }

                if (CreateRemoteThread(hndProc, (IntPtr)null, INTPTR_ZERO, lpLLAddress, lpAddress, 0, (IntPtr)null) == INTPTR_ZERO)
                {
                    return false;
                }

                CloseHandle(hndProc);

                return true;
            }
        }
    }
}
