using System.ComponentModel.DataAnnotations;

namespace Common.Code.Entity
{
    public interface IEntity<T>
    {
        [Key]
        T Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
