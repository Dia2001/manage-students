namespace QLSV.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("Nganh")]
    public partial class Nganh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nganh()
        {
            Lops = new HashSet<Lop>();
        }

        [Key]
        [StringLength(50)]
        public string MaNganh { get; set; }

        [StringLength(50)]
        public string TenNganh { get; set; }

        [Required]
        [StringLength(50)]
        public string MaKhoa { get; set; }

        [StringLength(50)]
        public string TongSTC { get; set; }

        public int? TrangThai { get; set; }

        public virtual Khoa Khoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // Lỗi được fix json vì json ko hỗ trợ khóa ngoại
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Lop> Lops { get; set; }
    }
}
