using DbLayer.Conext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataProcessor.AppSettings
{
    internal class AppConfiguration
    {
        private readonly ConfigurationBuilder _builder;
        private readonly IConfigurationRoot _configurationRoot;
        public AppConfiguration()
        {
            _builder = new ConfigurationBuilder();
            _builder.AddJsonFile("appsettings.json");
            var appSettingsPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).FullName + "\\AppSettings\\";
            _builder.SetBasePath(appSettingsPath);
            _configurationRoot = _builder.Build();
        }

        internal DbContextOptions<DbLayerContext> ConnectionOptions =>
            new DbContextOptionsBuilder<DbLayerContext>().UseSqlServer(_configurationRoot[key: "ConnectionStrings:DefaultConnection"]).Options;

        internal string ListeningPath => _configurationRoot[key: "ListeningPath:DefaultPath"];
    }
}