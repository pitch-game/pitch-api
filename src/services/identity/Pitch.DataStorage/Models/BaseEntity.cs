using System;
using System.ComponentModel.DataAnnotations;

namespace Pitch.DataStorage.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public string id { get; set; }
    }
}
