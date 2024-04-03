namespace AsyJob.Web.HAL
{

    public class Link
    {
        private Link(string href) { Href = href; }

        public string Href { get; private set; }
        public bool? Templated { get; private set; }
        public string? Type { get; private set; }
        public string? Deprecation { get; private set; }
        public string? Name { get; private set; }
        public string? Profile { get; private set; }
        public string? Title { get; private set; }
        public string? HrefLang { get; private set; }

        public class LinkBuilder(HALBuilder halBuilder, string uri, bool? templated = null)
        {
            private readonly HALBuilder _halBuilder = halBuilder;
            private readonly Link _link = new(uri)
            {
                Templated = templated
            };

            public LinkBuilder SetType(string mediaType) 
            {
                _link.Type = mediaType;
                return this;
            }

            public LinkBuilder IsDeprecated(string infoUri) 
            {
                _link.Deprecation = infoUri;
                return this;
            }

            public LinkBuilder SetName(string name) 
            {
                _link.Name = name;
                return this;
            }

            public LinkBuilder SetProfile(string profileUri) 
            {
                _link.Profile = profileUri;
                return this;
            }

            public LinkBuilder SetTitle(string title) 
            {
                _link.Title = title;
                return this;
            }

            public LinkBuilder SetHrefLang(string language) 
            {
                _link.HrefLang = language;
                return this;
            }

            public HALBuilder Build()
            {
                _halBuilder.AddLink(_link);
                return _halBuilder;
            }
        }
    }

}
