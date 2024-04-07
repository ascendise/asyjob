namespace AsyJob.Web.HAL
{
    public readonly struct Embed(HALDocument document, string? name)
    {
        public HALDocument Document { get; } = document;
        public string? Name { get; } = name;
    }
}
