﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PBL3Entities7 : DbContext
    {
        public PBL3Entities7()
            : base("name=PBL3Entities7")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cauhoi> cauhois { get; set; }
        public virtual DbSet<chitietdethi> chitietdethis { get; set; }
        public virtual DbSet<chitietketqua> chitietketquas { get; set; }
        public virtual DbSet<chitietquyen> chitietquyens { get; set; }
        public virtual DbSet<chitietthongbao> chitietthongbaos { get; set; }
        public virtual DbSet<chuong> chuongs { get; set; }
        public virtual DbSet<danhmucchucnang> danhmucchucnangs { get; set; }
        public virtual DbSet<dethi> dethis { get; set; }
        public virtual DbSet<dethitudong> dethitudongs { get; set; }
        public virtual DbSet<dokho> dokhoes { get; set; }
        public virtual DbSet<giaodethi> giaodethis { get; set; }
        public virtual DbSet<monhoc> monhocs { get; set; }
        public virtual DbSet<nguoidung> nguoidungs { get; set; }
        public virtual DbSet<nhom> nhoms { get; set; }
        public virtual DbSet<nhomquyen> nhomquyens { get; set; }
        public virtual DbSet<phancong> phancongs { get; set; }
        public virtual DbSet<thongbao> thongbaos { get; set; }
        public virtual DbSet<traloi> tralois { get; set; }
        public virtual DbSet<chitietnhom> chitietnhoms { get; set; }
    }
}
