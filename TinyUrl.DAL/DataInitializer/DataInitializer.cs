using Microsoft.EntityFrameworkCore;
using TinyUrl.DAL;

namespace TinyUrl.EF.DataInitializer
{
    public class DataInitializer
    {
        public static void RecreateDatabase(TinyUrlContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
           //      context.SaveChanges();
        }

        public static void ClearData(TinyUrlContext context)
        {
            ExecuteDeleteSql(context, "Users");
            ExecuteDeleteSql(context, "Urls");
        }

        private static void ExecuteDeleteSql(TinyUrlContext context, string tableName)
        {
            string command = $"Delete * from dbo.{tableName}";
            context.Database.ExecuteSqlRaw(command);
        }
    }
}