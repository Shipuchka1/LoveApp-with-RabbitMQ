namespace Dashboard.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [Key]
        public int user_id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        [StringLength(100)]
        public string phone { get; set; }

        [StringLength(100)]
        public string email { get; set; }
    }
}
