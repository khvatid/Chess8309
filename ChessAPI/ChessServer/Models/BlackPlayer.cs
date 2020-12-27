namespace ChessServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BlackPlayer
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_game { get; set; }

        [Key]
        [Column("blackPlayer", Order = 1)]
        [StringLength(50)]
        public string blackPlayer1 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(6)]
        public string Color { get; set; }
    }
}
