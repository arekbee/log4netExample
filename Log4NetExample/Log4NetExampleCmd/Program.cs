using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Log4NetExampleCmd
{
    class Program
    {
        private static readonly ILog gLogger =
                   LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Loggin app logs
        private static Timer gTimer;

        static Program()
        {
            gTimer = new Timer(10000);
            gTimer.Elapsed += LogAppStatusAsync;
            gTimer.Start();
        }
        public async static void LogAppStatusAsync(object sender, EventArgs eventArgs)
        {
            var gcTotalMemory = GC.GetTotalMemory(true);
            var WorkingSet64 = Process.GetCurrentProcess().WorkingSet64;
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000); // wait a second to get a valid reading
            var usage = cpuCounter.NextValue();

            gLogger.DebugFormat("APPSTAT MemGC:{0} WorkingSet:{1} Cpu:{2}", gcTotalMemory, WorkingSet64, usage);
        }

        #endregion


        static void Main(string[] args)
        {
            GlobalContext.Properties["args"] = string.Join(" ", args);


            for (int i = 0; i < int.MaxValue; i++)
            {
                gLogger.Debug("TEST i:" + i);
                int rand = new Random().Next(i * 100, 100 + i * 100);
                System.Threading.Thread.Sleep(rand);
                if (rand % i == 0)
                {
                    gLogger.Error("TEST ERROR");
                }
            }
        }
    }
}
