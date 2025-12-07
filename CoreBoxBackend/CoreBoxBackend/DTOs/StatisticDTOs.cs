namespace CoreBoxBackend.DTOs
{
    public sealed class StatisticDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }

    public sealed class StartSessionDto
    {
        public int ModuleId { get; set; }
    }

    public sealed class EndSessionDto
    {
        public int SessionId { get; set; }
    }
}