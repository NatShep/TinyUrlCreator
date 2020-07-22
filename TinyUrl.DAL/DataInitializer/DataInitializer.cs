using Microsoft.EntityFrameworkCore;
using TinyUrl.DAL;

namespace TinyUrl.EF.DataInitializer
{
    public class DataInitializer
    {
        public static void RecreateDatabase(UrlContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();

        }

        public static void ClearData(UrlContext context)
        {
            ExecuteDeleteSql(context, "Users");
            ExecuteDeleteSql(context, "Urls");
        }

        private static void ExecuteDeleteSql(UrlContext context, string tableName)
        {
            string command = $"Delete * from dbo.{tableName}";
            context.Database.ExecuteSqlRaw(command);
        }
    }
}