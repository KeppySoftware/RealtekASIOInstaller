using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtekASIOInstaller
{
    static class Program
    {
        static bool Is64 = Environment.Is64BitOperatingSystem;
        static Process RegSvr32 = new Process();
        static Process RegSvr64 = new Process();

        static string AppDir = Path.GetDirectoryName(Application.ExecutablePath);

        static string RASIODir32 = String.Format(@"{0}\RTCOM", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
        static string RASIODir64 = String.Format(@"{0}\RTCOM", Environment.GetFolderPath(Environment.SpecialFolder.System));

        static string RASIO32 = "RTHDASIO.DLL";
        static string RASIO64 = "RTHDASIO64.DLL";

        static string Final32 = String.Format(@"{0}\{1}", RASIODir32, RASIO32);
        static string Final64 = String.Format(@"{0}\{1}", RASIODir64, RASIO64);

        static string Arg = "/r";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] Arguments)
        {
            Arg = (Arguments.Length == 1) ? Arguments[0].ToLowerInvariant() : "/r";

            switch (Arg)
            {
                case "/u":
                    Unregister(false);
                    break;
                case "/r":
                default:
                    Register();
                    break;
            }
        }

        static void Register()
        {
            try
            {
                Unregister(true);

                if (!Directory.Exists(RASIODir32))
                    Directory.CreateDirectory(RASIODir32);

                File.Copy(String.Format(@"{0}\{1}", AppDir, RASIO32), Final32, true);

                if (Is64)
                {
                    if (!Directory.Exists(RASIODir64))
                        Directory.CreateDirectory(RASIODir64);

                    File.Copy(String.Format(@"{0}\{1}", AppDir, RASIO64), Final64, true);
                }

                RegSvr32.StartInfo.FileName = String.Format(@"{0}\regsvr32.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
                RegSvr32.StartInfo.Arguments = String.Format(@"/s {0}", Final32);

                RegSvr32.Start();
                RegSvr32.WaitForExit();

                if (RegSvr32.ExitCode != 0)
                {
                    MessageBox.Show("Realtek ASIO x86 failed to register!\n\nPress OK to exit.", "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Unregister(true);
                    return;
                }

                if (Is64)
                {
                    RegSvr64.StartInfo.FileName = String.Format(@"{0}\regsvr32.exe", Environment.GetFolderPath(Environment.SpecialFolder.System));
                    RegSvr64.StartInfo.Arguments = String.Format(@"/s {0}", Final64);

                    RegSvr64.Start();
                    RegSvr64.WaitForExit();

                    if (RegSvr32.ExitCode != 0)
                    {
                        MessageBox.Show("Realtek ASIO x64 failed to register!\n\nPress OK to exit.", "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Unregister(true);
                        return;
                    }
                }

                MessageBox.Show("Realtek ASIO is now registered!", "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("An error has occurred while registering the ASIO driver.\n\nError:\n{0}", ex.ToString()), "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Unregister(bool Silent)
        {
            bool ShowMsg = false;

            try
            {
                if (File.Exists(Final32))
                {
                    RegSvr32.StartInfo.FileName = String.Format(@"{0}\regsvr32.exe", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
                    RegSvr32.StartInfo.Arguments = String.Format(@"/u /s {0}", Final32);

                    RegSvr32.Start();
                    RegSvr32.WaitForExit();

                    File.Delete(Final32);
                    Directory.Delete(RASIODir32);

                    ShowMsg = true;
                }

                if (Is64)
                {
                    if (File.Exists(Final64))
                    {
                        RegSvr64.StartInfo.FileName = String.Format(@"{0}\regsvr32.exe", Environment.GetFolderPath(Environment.SpecialFolder.System));
                        RegSvr64.StartInfo.Arguments = String.Format(@"/u /s {0}", Final64);

                        RegSvr64.Start();
                        RegSvr64.WaitForExit();

                        File.Delete(Final64);
                        Directory.Delete(RASIODir64);

                        ShowMsg = true;
                    }
                }

                if (!Silent) MessageBox.Show("Realtek ASIO is now unregistered!", "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("An error has occurred while unregistering the ASIO driver.\n\nError:\n{0}", ex.ToString()), "Realtek ASIO Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
