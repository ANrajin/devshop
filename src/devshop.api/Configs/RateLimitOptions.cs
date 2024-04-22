namespace devshop.api.Configs
{
    public sealed class RateLimitOptions
    {
        public const string SectionName = "RateLimitConfigs";

        public int PermitLimit { get; set; }

        public int QueueLimit { get; set; }

        public int TimeWindow { get; set; }
    }
}
