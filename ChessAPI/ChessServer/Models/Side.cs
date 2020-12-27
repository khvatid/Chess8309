namespace ChessServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Side
    {
        [Key]
        public int ID_side { get; set; }

        public int Game_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Player_ID { get; set; }

        [Required]
        [StringLength(6)]
        public string Color { get; set; }

        public bool Result { get; set; }

        public virtual Game Game { get; set; }

        public virtual PlayerProfile PlayerProfile { get; set; }
    }
}
