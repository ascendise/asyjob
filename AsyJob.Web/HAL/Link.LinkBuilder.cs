namespace AsyJob.Web.HAL
{

    public partial class Link
    {
        public class LinkBuilder
        {
            private Link _link;

            private LinkBuilder(string uri = "/", bool? templated = null)
            {
                _link = new Link(uri, templated);
            }

            public static LinkBuilder New(string uri = "/", bool? templated = null)
                => new(uri, templated);

            public LinkBuilder SetHref(string href)
            {
                _link.Href = href;
                return this;
            }

            public LinkBuilder SetTemplated(bool templated)
            {
                _link.Templated = templated;
                return this;
            }

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