//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PBL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class chitietketqua
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public chitietketqua()
        {
            this.giaodethis = new HashSet<giaodethi>();
        }
    
        public int ID { get; set; }
        public int maketqua { get; set; }
        public int macauhoi { get; set; }
        public int dapanchon { get; set; }
    
        public virtual cauhoi cauhoi { get; set; }
        public virtual traloi traloi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<giaodethi> giaodethis { get; set; }
    }
}
