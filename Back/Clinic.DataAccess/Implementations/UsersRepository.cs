﻿namespace Clinic.DataAccess.Implementations
{
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;
    using Domain;
    using Repositories;

    public class UsersRepository : IUsersRepository
    {
        private readonly IDbConnection connection;

        public UsersRepository(IDbConnection connection)
        {
            this.connection = connection;
        }
        
        public async Task<User> GetAsync(string username)
        {
            return await this.connection.QueryFirstOrDefaultAsync<User>(@"select * from users where ""Username"" = @username limit 1", new {username});
        }

        public async Task CreateAsync(User user)
        {
            await this.connection.ExecuteAsync(@"
                insert into users (""Username"", ""PasswordHash"", ""FirstName"", ""SecondName"", ""ThirdName"", ""UserPermission"")
                    values (@Username, @PasswordHash, @FirstName, @SecondName, @ThirdName, @UserPermission)",
                user);
        }

        public async Task UpdateAsync(User user)
        {
            await this.connection.ExecuteAsync(@"
                update users set 
                    ""PasswordHash"" = @PasswordHash, 
                    ""FirstName"" = @FirstName, 
                    ""SecondName"" = @SecondName, 
                    ""ThirdName"" = @ThirdName, 
                    ""UserPermission"" = @UserPermission
                    where ""Username"" = @Username",
                user);
        }
        
        public async Task DeleteAsync(string username)
        {
            await this.connection.ExecuteAsync(@"delete from services where ""Username"" = @username", new { username });
        }
    }
}
