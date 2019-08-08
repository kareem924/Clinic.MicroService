using System.ComponentModel.DataAnnotations;
using Common.General.Interfaces;

namespace Common.General.Entity
{
    public class BaseEntity<T>:IEntity
    {
        [Key]
        T Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
