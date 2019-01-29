using MicwayTech.Domain.Helper;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace MicwayTech.Domain
{
    /// <summary>
    /// Driver Class 
    /// </summary>
    public class Driver : IDriver
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [EmailAddress(ErrorMessage = "Email Address not valid.")]
        [MaxLength(100, ErrorMessage = "Last Name cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

    }
}
