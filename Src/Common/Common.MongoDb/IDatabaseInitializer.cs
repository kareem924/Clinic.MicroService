using System.Threading.Tasks;

namespace Common.MongoDb
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
