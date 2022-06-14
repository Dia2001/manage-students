namespace QLSV.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("MonHoc")]
    public partial class MonHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MonHoc()
        {
            Diems = new HashSet<Diem>();
        }

        [Key]
        [StringLength(50)]
        public string MaMH { get; set; }

        [Required]
        [StringLength(50)]
        public string TenMH { get; set; }

        public int? STC { get; set; }

        public int? SoTietLT { get; set; }

        public int? SoTietTH { get; set; }

        [Required]
        [StringLength(50)]
        public string MaKhoa { get; set; }

        public int? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        // Lỗi được fix json vì json ko hỗ trợ khóa ngoại
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Diem> Diems { get; set; }

        public virtual Khoa Khoa { get; set; }
    }
}
