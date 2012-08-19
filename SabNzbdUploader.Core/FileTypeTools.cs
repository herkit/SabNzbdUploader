using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Arasoft.SabNzdbUploader.Core
{
    public static class FileTypeTools
    {
        public static void SetAssociation(string extension, string keyName, string openWith, string fileDescription)
        {
            RegistryKey BaseKey;
            RegistryKey OpenMethod;
            RegistryKey Shell;
            RegistryKey CurrentUser;

            BaseKey = Registry.ClassesRoot.CreateSubKey(extension);
            BaseKey.SetValue("", keyName);

            OpenMethod = Registry.ClassesRoot.CreateSubKey(keyName);
            OpenMethod.SetValue("", fileDescription);
            OpenMethod.CreateSubKey("DefaultIcon").SetValue("", String.Format("\"{0}\",0", openWith));
            Shell = OpenMethod.CreateSubKey("Shell");
            Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", String.Format("\"{0}\" \"%1\"", openWith));
            BaseKey.Close();
            OpenMethod.Close();
            Shell.Close();

            // Delete the key instead of trying to change it
            CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension, true);
            CurrentUser.DeleteSubKey("UserChoice", false);
            CurrentUser.Close();

            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool IsAssociated(string extension, string keyName, string openWith)
        {
            var BaseKey = Registry.ClassesRoot.OpenSubKey(extension);

            if (BaseKey == null)
                return false;

            if ((string)BaseKey.GetValue("") != keyName)
                return false;

            var OpenMethod = Registry.ClassesRoot.OpenSubKey(keyName);
            var Shell = OpenMethod.OpenSubKey("Shell");
            var currentOpenWith = (string)(Shell.OpenSubKey("open").OpenSubKey("command").GetValue(""));

            return (string)currentOpenWith == String.Format("\"{0}\" \"%1\"", openWith);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
