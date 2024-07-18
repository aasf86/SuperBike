using System.ComponentModel.DataAnnotations.Schema;

namespace SuperBike.Infrastructure.Repositories
{
    public class Helpers
    {
        public static class StrSql
        {
            private static string GetTableName<T>()
            {
                var type = typeof(T);
                var attrTable = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;
                return attrTable is null ? type.Name : attrTable.Name;
            }

            private static List<string> GetPropertiesColumn<T>()
            {
                return typeof(T)
                    .GetProperties()
                    .OrderBy(p => p.Name.ToLower())
                    .Where(x => x.Name.ToLower() != "id" && !x.GetCustomAttributes(true).Any(z => z is NotMappedAttribute))
                    .Select(x => x.Name)
                    .ToList();                
            }

            public static string CreateSqlSelect<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = " where id = @id ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    select *
                    from {tableName}
                    {internalFilter}
                ";
            }

            public static string CreateSqlInsert<T>()
            {
                var tableName = GetTableName<T>();
                var propertiesEntity = GetPropertiesColumn<T>();
                var sqlInsert = $@"
                    insert into {tableName} ({string.Join(", ", propertiesEntity)})
                    values ({string.Join(", ", propertiesEntity.Select(x => $"@{x}"))}) returning id
                ";
                return sqlInsert;
            }

            public static string CreateSqlUpdate<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var propertiesEntity = GetPropertiesColumn<T>();
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = " where id = @id ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                var sqlUpdate = $@"
                    update {tableName}
                    set {string.Join(", ", propertiesEntity.Select(x => $"{x} = @{x}"))}
                    {internalFilter}
                ";
                return sqlUpdate;
            }

            public static string CreateSqlDelete<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = " where id = @id ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    delete from {tableName} 
                    {internalFilter}
                ";
            }
        }
    }
}
