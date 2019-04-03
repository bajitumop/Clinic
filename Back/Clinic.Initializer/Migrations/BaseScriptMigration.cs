namespace Clinic.Initializer.Migrations
{
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;

    public abstract class BaseScriptMigration : Migration
    {
        public override async Task ExecuteAsync(IDbConnection connection)
        {
            await connection.ExecuteAsync(this.Script);
        }
        
        protected abstract string Script { get; }
    }
}
