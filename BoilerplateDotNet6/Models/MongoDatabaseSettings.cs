namespace BoilerplateDotNet6.Models
{
    /// <summary>
    /// Retrieves the database parameters and make is available
    /// </summary>
    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string UsersCollectionName { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
    }

    public interface IMongoDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
