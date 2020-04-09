using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MathClientSharp
{
    
    class Program
    {
        #region Win32 APIs
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);
        #endregion
        #region delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr NicksIntFunction(IntPtr numPointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr NicksStringFunction();
        #endregion

        const string dllName = "WiltedBeetroot.dll";
        const string intFunctionName = "calculate_cube";
        const string stringFunctionName = "get_library_name";

        static void Main(string[] args)
        {
            string dllLoadPath="";
            IntPtr hModule = IntPtr.Zero;
            IntPtr funcaddr = IntPtr.Zero;
            try
            {
                dllLoadPath = $"{Directory.GetCurrentDirectory()}\\{dllName}";
                Console.WriteLine($"{dllLoadPath}");

                hModule = LoadLibrary(dllLoadPath);
                if (hModule == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to load library {dllLoadPath} (ErrorCode: {errorCode})");
                }
                Console.WriteLine($"{DateTime.Now.ToString()} library {dllLoadPath} was loaded sucessfully. hModule={hModule}");

                funcaddr = GetProcAddress(hModule, intFunctionName);
                if (funcaddr == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to find function {intFunctionName} (ErrorCode: {errorCode})");
                }
                Console.WriteLine($"{DateTime.Now.ToString()} function {intFunctionName} found in library {dllLoadPath} address={funcaddr}");
                Int32 number = 5;
                IntPtr numPointer = new IntPtr(number);
                NicksIntFunction intFunction = Marshal.GetDelegateForFunctionPointer<NicksIntFunction>(funcaddr) as NicksIntFunction;
                IntPtr intResultPtr = intFunction(numPointer);
                Int32 intResult = intResultPtr.ToInt32();
                Console.WriteLine($"{DateTime.Now.ToString()} function {intFunctionName} returned \"{intResult}\"");

                funcaddr = GetProcAddress(hModule, stringFunctionName);
                if (funcaddr == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Exception($"Failed to find function {stringFunctionName} (ErrorCode: {errorCode})");
                }
                Console.WriteLine($"{DateTime.Now.ToString()} function {stringFunctionName} found in library {dllLoadPath} address={funcaddr}");

                NicksStringFunction stringFunction = Marshal.GetDelegateForFunctionPointer<NicksStringFunction>(funcaddr) as NicksStringFunction;
                IntPtr stringResultPtr = stringFunction();
                string stringResult = Marshal.PtrToStringBSTR(stringResultPtr);
                Console.WriteLine($"{DateTime.Now.ToString()} function {stringFunctionName} returned \"{stringResult}\"");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            finally 
            {
                if (hModule != IntPtr.Zero)
                {
                    FreeLibrary(hModule);
                    Console.WriteLine($"{DateTime.Now.ToString()} library {dllLoadPath} was unloaded");
                };
            }
        }
    }
}
