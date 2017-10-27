using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Pool
{
  public interface IEntity
  {
    int Id { get; set; }

    DateTime LastModified { get; set; }
  }

  public abstract class Entity : IEntity
  {
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "DateTime")]
    public DateTime LastModified { get; set; }
  }
}
