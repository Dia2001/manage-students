namespace QLSV.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [Table("GiangVien")]
    public partial class GiangVien
    {
        [Key]
        [StringLength(50)]
        public string MaGV { get; set; }

        [StringLength(50)]
        public string HoTenGV { get; set; }

        public bool? GioiTinh { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string SDT { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string TrinhDo { get; set; }

        [Required]
        [StringLength(50)]
        public string MaKhoa { get; set; }
        [StringLength(50)]
        public string HinhAnh { get; set; }
        public int? TrangThai { get; set; }
        public virtual Khoa Khoa { get; set; }
    }
}
