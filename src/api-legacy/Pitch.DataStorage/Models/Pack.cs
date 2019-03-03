using System;

namespace Pitch.DataStorage.Models
{
    /// <summary>
    /// A Pack is opened to create a Card.
    /// </summary>
    public class Pack : BaseEntity
    {
        public Guid PlayerId { get; set; }
        public PackType Type { get; set; }
        public bool Opened { get; set; }
        public DateTime CreatedOn { get; set; }

        //Curious?
        public Guid GeneratedCard { get; set; }
    }

    public enum PackType
    {
        Default
    }
}
