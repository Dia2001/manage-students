namespace QLSV.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("Lop")]
    public partial class Lop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lop()
        {
            SinhViens = new HashSet<SinhVien>();
        }

        [Key]
        [StringLength(50)]
        public string MaLop { get; set; }

        [StringLength(50)]
        public string TenLop { get; set; }

        [Required]
        [StringLength(50)]
        public string MaNganh { get; set; }
        public int? TrangThai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<SinhVien> SinhViens { get; set; }
        public virtual Nganh Nganh { get; set; }
    }
}
