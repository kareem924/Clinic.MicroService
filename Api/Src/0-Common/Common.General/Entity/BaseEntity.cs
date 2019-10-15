using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.General.Interfaces;

namespace Common.General.Entity
{
    public class BaseEntity<T>:IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public T Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
