using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeactivisionInfiniteWordTrayarko
{
    public class ProcessListener
    {
        private static readonly TimeSpan CheckSpan = new TimeSpan(0, 0, 2);
        private List<string> CodList { get; set; }

        public delegate void Launched(object sender, Process process);
        public event Launched CodLaunched;

        public delegate void Exited(object sender, Process process);
        public event Exited CodExited;

        private bool WasCodLaunchedLastTime { get; set; }
        private Process FoundProcess { get; set; }

        public ProcessListener()
        {
            CodList = new List<string>();
        }

        public void AddProcess(string processName)
        {
            CodList.Add(processName);
        }

        private void CheckCods()
        {
            var processes = Process.GetProcesses();
            bool didntFoundCod = true;
            foreach (var cod in CodList)
            {
                var pr = processes.FirstOrDefault(p => p.ProcessName == cod);
                if (pr == default(Process))
                    continue;

                didntFoundCod = false;

                if (WasCodLaunchedLastTime)
                    continue;

                WasCodLaunchedLastTime = true;
                CodLaunched(this, pr);
                FoundProcess = pr;

                Console.WriteLine("COD");
            }

            if (didntFoundCod)
            {
                WasCodLaunchedLastTime = false;

                if (WasCodLaunchedLastTime)
                    CodExited(this, FoundProcess);
            }
        }

        public async Task ObserveForever()
        {
            while (true)
            {
                CheckCods();

                await Task.Delay(CheckSpan);
            }
        }
    }
}
