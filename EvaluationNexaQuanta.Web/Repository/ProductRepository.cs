using Dapper;
using EvaluationNexaQuanta.Models;
using System.Data;
using Microsoft.Data.SqlClient;

public class ProductRepository
{
    private readonly string _connectionString;
    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var db = Connection;
        return await db.QueryAsync<Product>("sp_GetAllProducts", commandType: CommandType.StoredProcedure);
    }

    public async Task AddAsync(Product product)
    {
        using var db = Connection;
        var parameters = new
        {
            product.Name,
            product.Price,
            product.Quantity
        };
        await db.ExecuteAsync("sp_InsertProduct", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Product product)
    {
        using var db = Connection;
        var parameters = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Quantity
        };
        await db.ExecuteAsync("sp_UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var db = Connection;
        await db.ExecuteAsync("sp_DeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var db = Connection;
        var parameters = new { Id = id };
        return await db.QueryFirstOrDefaultAsync<Product>(
            "sp_GetProductById",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }



}
