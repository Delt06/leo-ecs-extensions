using Runner.Levels;

namespace Runner._Shared
{
    public class RuntimeData
    {
        public Level Level { get; private set; }

        public void OnSpawnedLevel(Level level)
        {
            Level = level;
        }
    }
}