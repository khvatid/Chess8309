namespace ChessServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlayerProfile")]
    public partial class PlayerProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlayerProfile()
        {
            Sides = new HashSet<Side>();
        }

        [Key]
        [StringLength(50)]
        public string emailPlayer { get; set; }

        [Required]
        [StringLength(25)]
        public string namePlayer { get; set; }

        [Required]
        [StringLength(50)]
        public string passwordPlayer { get; set; }

        public int victory { get; set; }

        public int defeat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Side> Sides { get; set; }
    }
}
