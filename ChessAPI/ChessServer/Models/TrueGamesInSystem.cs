namespace ChessServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TrueGamesInSystem")]
    public partial class TrueGamesInSystem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Player_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        public string namePlayer { get; set; }

        public int? CountAllGames { get; set; }
    }
}
