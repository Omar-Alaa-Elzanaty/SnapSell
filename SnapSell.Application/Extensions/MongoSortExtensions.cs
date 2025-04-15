using MongoDB.Driver;

namespace SnapSell.Application.Extensions
{
    public static class MongoSortExtensions
    {
        /// <summary>
        /// Converts a sort string to MongoDB SortDefinition
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="sortBy">Sort string (e.g., "name asc, createdAt desc")</param>
        /// <returns>Configured SortDefinition</returns>
        public static SortDefinition<TColumn> ToSortDefinition<TColumn>(this string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return Builders<TColumn>.Sort.Ascending("_id");
            }

            var sortBuilder = Builders<TColumn>.Sort;
            var sortDefinitions = new List<SortDefinition<TColumn>>();

            foreach (var field in sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var (key, ascending) = ParseSortField(field.Trim());
                sortDefinitions.Add(ascending
                    ? sortBuilder.Ascending(key)
                    : sortBuilder.Descending(key));
            }

            return sortBuilder.Combine(sortDefinitions);
        }

        private static (string key, bool ascending) ParseSortField(string field)
        {
            bool ascending = true;
            int spaceIndex = field.IndexOf(' ');
            string key = field;

            if (spaceIndex >= 0)
            {
                key = field[..spaceIndex];
                string direction = field[(spaceIndex + 1)..].Trim().ToLower();

                ascending = direction switch
                {
                    "asc" => true,
                    "desc" => false,
                    _ => throw new ArgumentException($"Invalid sorting direction: {direction}")
                };
            }

            return (key, ascending);
        }
    }
}