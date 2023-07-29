using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace shoppingcomdraft5.Models
{
    public class AuditLog
    {
        [Key]
        public int Audit_ID { get; set; }

        //Logged in user performing the action
        [Display(Name = "Performed By")]
        public string Username { get; set; }

        //Time when the event occurred
        [Display(Name = "Date Time Stamp")]
        [DataType(DataType.DateTime)]
        public DateTime DateTimeStamp { get; set; }

        //Login Success/Failure/Logout, Create, Delete, View, Update
        [Display(Name = "Action Type")]
        public string ActionType { get; set; }

        //Table Name
        [Display(Name = "Table Name")]
        public string TableName { get; set; }

        //Table's ID of item that got affected
        [Display(Name = "Table's ID")]
        public int TableID { get; set; }

        //Item before the changes made
        [Display(Name = "Before Changes")]
        public string BeforeChange { get; set; }

        //Item after the changes made
        [Display(Name = "After Changes")]
        public string AfterChange { get; set; }
    }
}