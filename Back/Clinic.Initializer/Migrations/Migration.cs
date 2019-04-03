namespace Clinic.Initializer.Migrations
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Threading.Tasks;

    public abstract class Migration
    {
        public DateTime MigrationDateTime => DateTime.ParseExact(
            this.GetType().Name.Substring(1, 14),
            "yyyyMMddHHmmss",
            CultureInfo.InvariantCulture);

        public abstract Task ExecuteAsync(IDbConnection connection);
    }
}
