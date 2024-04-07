namespace AsyJob.Web.HAL
{
    public readonly struct Embed(HalDocument document, string? name)
    {
        public HalDocument Document { get; } = document;
        public string? Name { get; } = name;
    }
}
