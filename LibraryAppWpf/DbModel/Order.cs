﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace LibraryAppWpf.DbModel
{
    public partial class Order
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int StatusId { get; set; }
        public int StaffId { get; set; }
        public DateTime TakingDate { get; set; }
        public int Length { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual User Staff { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual Student Student { get; set; }
    }
}