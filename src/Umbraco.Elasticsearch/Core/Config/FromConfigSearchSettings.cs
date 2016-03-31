using System;

namespace Umbraco.Elasticsearch.Core.Config
{
    public class FromConfigSearchSettings : ISearchSettings
    {
        private const string Prefix = "SearchSettings:";

        public string Host => nameof(Host).FromAppSettingsWithPrefix(Prefix, "http://localhost:9200");

        public string IndexEnvironmentPrefix
        {
            get
            {
                var value = nameof(IndexEnvironmentPrefix).FromAppSettingsWithPrefix(Prefix, string.Empty);
                return Environment.ExpandEnvironmentVariables(value).ToLowerInvariant();
            }
        }

        public string IndexName => nameof(IndexName).FromAppSettingsWithPrefix(Prefix);
    }
}