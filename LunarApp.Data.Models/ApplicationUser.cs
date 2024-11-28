using Microsoft.AspNetCore.Identity;

namespace LunarApp.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.Id = Guid.NewGuid();
        }

        //TODO: Add more properties to user

        //public virtual ICollection<ApplicationUserMovie> ApplicationUserMovies { get; set; }
        //    = new HashSet<ApplicationUserMovie>();

        //public virtual ICollection<Ticket> Tickets { get; set; }
        //    = new HashSet<Ticket>();
    }
}


