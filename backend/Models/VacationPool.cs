using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models 
{
    public class VacationPool : IEntity
    {
        [Column("VacationPoolId")]
        public int Id {get;set;}
        public DateTime StartDate {get;set;}
        public int Hours { get; set; }
        public bool IsActive {get;set;}
        public DateTime LastModifiedOn {get;set;} = DateTime.UtcNow;
        public string LastModifiedBy { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}