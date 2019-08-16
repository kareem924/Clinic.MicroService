using System.ComponentModel.DataAnnotations;
using Common.General.Interfaces;

namespace Common.General.Entity
{
    public class BaseEntity<T>:IEntity
    {
        [Key]
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
