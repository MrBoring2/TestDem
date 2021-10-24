namespace TestProducts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterialToProduct")]
    public partial class MaterialToProduct
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string MaterialName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AmountOfMaterial { get; set; }

        public virtual Materials Materials { get; set; }

        public virtual Products Products { get; set; }
    }
}
