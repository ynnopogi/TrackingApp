using System;
using System.ComponentModel.DataAnnotations;
using Tracking.Entities.Models;

namespace Tracking.Common.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Active?")]
        public bool Active { get; set; }

        [Display(Name = "Clock In")]
        public DateTime? ClockIn { get; set; }

        [Display(Name = "Clock Out")]
        public DateTime? ClockOut { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public EmployeeViewModel Map(Employee employee) => new EmployeeViewModel
        {
            Id = employee.Id,
            Active = employee.Active,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            ClockIn = employee.ClockIn,
            ClockOut = employee.ClockOut
        };
    }
}