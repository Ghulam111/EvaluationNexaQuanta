using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using EvaluationNexaQuanta.Models;

namespace EvaluationNexaQuanta.Repository
{
    public class UsersRepository
    {
        private readonly IDbConnection _db;

        public UsersRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<UserModel?> ValidateUserAsync(string username, string password)
        {

            const string sql = "SELECT * FROM Users WHERE Username = @Username";
            var user = await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Username = username });

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null;
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            const string sql = "SELECT * FROM Users WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Username = username });
        }

        public async Task AddUserAsync(UserModel user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            const string sql = @"INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

            await _db.ExecuteAsync(sql, new
            {
                Username = user.Username,
                Password = hashedPassword
            });
        }

       public static async Task SeedAdminUser(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<UsersRepository>();

            var existing = await repo.GetByUsernameAsync("admin");
            if (existing == null)
            {
                await repo.AddUserAsync(new UserModel
                {
                    Username = "admin",
                    Password ="admin@123"
                });
            }
        }

    }
}

