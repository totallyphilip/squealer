using System.Runtime.InteropServices;

namespace SquealerConsoleCSharp
{
    internal class KeepAlive
    {
        [Flags]
        public enum ExecutionState : uint
        {
            AwayModeRequired = 0x00000040,
            Continuous       = 0x80000000,
            DisplayRequired  = 0x00000002,
            SystemRequired   = 0x00000001,
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        public void KeepMonitorActive()
        {
            SetThreadExecutionState(ExecutionState.DisplayRequired | ExecutionState.Continuous);
        }

        public void RestoreMonitorSettings()
        {
            SetThreadExecutionState(ExecutionState.Continuous);
        }
    }
}
