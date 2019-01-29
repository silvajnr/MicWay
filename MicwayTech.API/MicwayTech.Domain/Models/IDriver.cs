using System;

namespace MicwayTech.Domain
{
    /// <summary>
    /// Driver Class Interface
    /// </summary>
    public interface IDriver
    {
        string Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime DOB { get; set; }
        string Email { get; set; }
    }
}
