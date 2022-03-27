using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace DELTation.LeoEcsExtensions.Tests.EditMode
{
    internal class DebugTestAttribute : NUnitAttribute, IApplyToTest
    {
        public void ApplyToTest(Test test)
        {
#if !DEBUG
            test.RunState = RunState.Ignored;
            test.Properties.Set("_SKIPREASON", "Not running in DEBUG mode.");
#endif
        }
    }
}