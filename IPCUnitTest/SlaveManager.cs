using System;
using System.Diagnostics;
using System.IO;

namespace IPCUnitTest {

    public class SlaveManager : IDisposable {

        private Process slaveProc;
        private bool _isDisposed = false;

        private void OnProcessExit(object sender, EventArgs e) {
            Dispose();
        }

        public SlaveManager() {
            // subscribe to be able kill and free resources before exit
            AppDomain.CurrentDomain.DomainUnload += OnProcessExit;
        }

        ~SlaveManager() {
            Dispose(false);
        }

        public string GetSlavePath() {
            string slaveDir = Environment.CurrentDirectory;
            // \ParentFolder\TestResults\%_Mashine ID_% %_Date_%\Out
            for (int i = 0; i < 3; i++) {
                slaveDir = Directory.GetParent(slaveDir).FullName;
            }
            slaveDir = Path.Combine(slaveDir, "ICPTestSlave\\bin\\Debug\\ICPTestSlave.exe");
            return slaveDir;
        }

        public int LaunchSlave() {
            if (slaveProc != null && !slaveProc.HasExited) {
                return slaveProc.Id;
            }
            // Prepare the slave process to run
            ProcessStartInfo slaveStartInfo = new ProcessStartInfo();
            slaveStartInfo.FileName = GetSlavePath();
            if (!File.Exists(slaveStartInfo.FileName)) {
                throw new Exception("Can't find the slave binaries!");
            }
#if DEBUG
            slaveStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            slaveStartInfo.CreateNoWindow = true;            
#endif
            slaveProc = Process.Start(slaveStartInfo);
            return slaveProc.Id;
        }

        private void Dispose(bool disposing) {
            if (!_isDisposed) {
                try {
                    slaveProc.Kill();
                } catch (InvalidOperationException) {
                    // keep silence if process has been crashed   
                }
                slaveProc.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
