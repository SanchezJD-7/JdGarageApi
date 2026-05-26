using JdGarageApi.Data;
using JdGarageApi.Hubs;
using JdGarageApi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace JdGarageApi.Services;

public class KpiService : IKpiService
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<KpiHub> _hubContext;

    public KpiService(ApplicationDbContext db, IHubContext<KpiHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }

    public async Task BroadcastAsync()
    {
        var totalUsers = await _db.Set<AppUser>().CountAsync();
        var totalBikes = await _db.Set<Bike>().CountAsync();
        var totalCars = await _db.Set<Car>().CountAsync();

        await _hubContext.Clients.All.SendAsync("KpiUpdated", new Kpi
        {
            Users = totalUsers,
            Vehicles = totalBikes + totalCars,
            Bikes = totalBikes,
            Cars = totalCars
        });
    }
}
