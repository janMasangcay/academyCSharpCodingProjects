namespace Quote_codeFirst_
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quote
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(60)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string DateOfBirth { get; set; }

        public int CarYear { get; set; }

        [Required]
        [StringLength(50)]
        public string CarMake { get; set; }

        [Required]
        [StringLength(50)]
        public string CarModel { get; set; }

        [Required]
        [StringLength(10)]
        public string DUI { get; set; }

        public int SpeedingTicket { get; set; }

        [Required]
        [StringLength(20)]
        public string Coverage { get; set; }

        public double Amount { get; set; }
    }
}
