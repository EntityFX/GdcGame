namespace EntityFX.Gdcame.Manager.Contract.Common.WorkerManager
{
    public class WorkerStatus
    {
        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public long Ticks { get; set; }
    }
}