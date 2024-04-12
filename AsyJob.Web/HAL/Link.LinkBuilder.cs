namespace AsyJob.Web.HAL
{

    public partial class Link
    {
        public class LinkBuilder(string uri, bool? templated = null) 
        {
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

            public Link Build()
                => _link;
        }
    }
}