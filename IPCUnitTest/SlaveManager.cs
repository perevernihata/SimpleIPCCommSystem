using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace IPCUnitTest {

    /// <summary>
    /// 
    /// </summary>
    public class SlaveManager : IDisposable {

        private static SlaveManager _instance = null;
        public static SlaveManager Instance() {
            if (_instance == null) {
                _instance = new SlaveManager();
            }
            return _instance;
        }

        private Process slaveProc;

        private void OnProcessExit(object sender, EventArgs e) {
            Dispose();
        }

        private SlaveManager() {
            // subscribe to be able kill and free resources before exit
            AppDomain.CurrentDomain.DomainUnload += OnProcessExit;
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
            slaveStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            slaveStartInfo.CreateNoWindow = true;
            slaveProc = Process.Start(slaveStartInfo);
            return slaveProc.Id;
        }

        public void Dispose() {
            slaveProc.Kill();
            slaveProc.Dispose();
        }
    }
}
