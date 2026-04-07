using Microsoft.Win32;

namespace HabitGhost.Additionals
{
    public static class MachineGuid
    {
        public static string GetMachineGuid()
        {
            string key = @"SOFTWARE\Microsoft\Cryptography";
            using (var localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var rk = localMachineX64View.OpenSubKey(key))
            {
                return rk?.GetValue("MachineGuid")?.ToString();
            }
        }
    }
}
