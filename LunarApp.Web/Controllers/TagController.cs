using LunarApp.Data;
using LunarApp.Data.Models;
using LunarApp.Web.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Web.Controllers
{
    [Authorize]
    public class TagController(ApplicationDbContext context) : Controller
    {
        
    }
}
