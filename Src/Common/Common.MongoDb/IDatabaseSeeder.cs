using System.Threading.Tasks;

namespace Common.MongoDb
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}
