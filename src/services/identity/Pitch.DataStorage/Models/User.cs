using Microsoft.EntityFrameworkCore;
using Pitch.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pitch.DataStorage.Models
{
    /// <summary>
    /// A human user of the pitch site
    /// </summary>
    [Table("Users")]
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public long Xp { get; set; }

        /// <summary>
        /// Move to domain model
        /// </summary>
        public int Level => Convert.ToInt32(Constants.XpToLevelMultiplier * Math.Sqrt(Xp));

        public double Money { get; set; }

        public Squad ActiveSquad { get; set; } = new Squad();
    }

    /// <summary>
    /// Squad
    /// </summary>
    [Owned]
    public class Squad
    {
        public Squad() => CreatedOn = DateTime.UtcNow;
        //public IList<Slot> Positions { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// Links a Card to a Position
    /// </summary>
    [Owned]
    public class Slot
    {
        public Position Position { get; set; }
        public Card Card { get; set; }
    }

    public enum Position
    {
        GK,
        CB,
        LB,
        RB,
        CM,
        LM,
        RM,
        ST
    }
}
