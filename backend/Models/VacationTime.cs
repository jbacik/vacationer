using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models 
{
    public class VacationTime : IEntity
    {
        [Column("VacationTimeId")]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Hours { get; set; }
        public bool IsPlanned { get; set; }
        public DateTime LastModifiedOn {get;set;} = DateTime.UtcNow;
        public string LastModifiedBy { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}