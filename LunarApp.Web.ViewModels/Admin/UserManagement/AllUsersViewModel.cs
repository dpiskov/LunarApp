namespace LunarApp.Web.ViewModels.Admin.UserManagement
{
    public class AllUsersViewModel
    {
        public required string Id { get; set; }
        public string? Email { get; set; }
        public required IEnumerable<string> Roles {get; set; }
    }
}
