using MegaCrit.Sts2.Core.Logging;

namespace DeadBranch.Scripts.Logging;

public static class DeadBranchLog
{
    public static void Info(string text, int skipFrame = 2)
    {
        Log.Info("[DeadBranch]" + text, skipFrame);
    }
}