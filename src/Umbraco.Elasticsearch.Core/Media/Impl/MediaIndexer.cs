using System;
using Umbraco.Core.Logging;

namespace Umbraco.Elasticsearch.Core.Media.Impl
{
    public class MediaIndexer : IEntityIndexer
    {
        public void Build(string indexName)
        {
            var indexExists = UmbracoSearchFactory.Client.IndexExists(indexName)?.Exists ?? false;
            if (!indexExists) throw new InvalidOperationException($"'{indexName}' not available, please ensure you have created an index with this name");
            using (BusyStateManager.Start($"Building media for {indexName}", indexName))
            {
                LogHelper.Info<MediaIndexer>($"Started building index [{indexName}]");
                foreach (var indexService in UmbracoSearchFactory.GetMediaIndexServices())
                {
                    try
                    {
                        LogHelper.Info<MediaIndexer>($"Started to index media for {indexService.DocumentTypeName}");
                        BusyStateManager.UpdateMessage($"Indexing {indexService.DocumentTypeName}");
                        indexService.Build(indexName);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error<MediaIndexer>($"Failed to index media for {indexService.DocumentTypeName}", ex);
                    }
                }
                LogHelper.Info<MediaIndexer>(
                    $"Finished building index [{indexName}] : elapsed {BusyStateManager.Elapsed:g}");
            }
        }
    }
}