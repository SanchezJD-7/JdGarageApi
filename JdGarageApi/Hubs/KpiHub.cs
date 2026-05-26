using JdGarageApi.Data;
using JdGarageApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JdGarageApi.Hubs;

public class KpiHub : Hub
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _db;

    public KpiHub(UserManager<AppUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public override async Task OnConnectedAsync()
    {
        var totalUsers = await _userManager.Users.CountAsync();
        var totalBikes = await _db.Vehicles.OfType<Bike>().CountAsync();
        var totalCars = await _db.Vehicles.OfType<Car>().CountAsync();
        var totalVehicles = totalBikes + totalCars;

        await Clients.Caller.SendAsync("KpiUpdated", new Kpi
        {
            Users = totalUsers,
            Vehicles = totalVehicles,
            Bikes = totalBikes,
            Cars = totalCars
        });
        await base.OnConnectedAsync();
    }
}
