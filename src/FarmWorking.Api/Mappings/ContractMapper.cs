using C = FarmWorking.Shared;
using D = FarmWorking.Domain.Entities;
using E = FarmWorking.Domain.Enums;

namespace FarmWorking.Api.Mappings;

internal static class ContractMapper
{
    public static C.Farm ToContract(this D.Farm x) => new() { Id = x.Id, Name = x.Name, Location = x.Location, Latitude = x.Latitude, Longitude = x.Longitude, Area = x.Area, AreaUnit = x.AreaUnit, Crop = x.Crop, Description = x.Description, CreatedAt = x.CreatedAt };
    public static D.Farm ToDomain(this C.Farm x) => new() { Id = x.Id, Name = x.Name, Location = x.Location, Latitude = x.Latitude, Longitude = x.Longitude, Area = x.Area, AreaUnit = x.AreaUnit, Crop = x.Crop, Description = x.Description, CreatedAt = x.CreatedAt };
    public static C.WorkNote ToContract(this D.WorkNote x) => new() { Id = x.Id, FarmId = x.FarmId, WorkDate = x.WorkDate, Title = x.Title, Content = x.Content, Worker = x.Worker };
    public static D.WorkNote ToDomain(this C.WorkNote x) => new() { Id = x.Id, FarmId = x.FarmId, WorkDate = x.WorkDate, Title = x.Title, Content = x.Content, Worker = x.Worker };
    public static C.FinanceTag ToContract(this D.FinanceTag x) => new() { Id = x.Id, Name = x.Name, Type = (C.TransactionType)x.Type };
    public static D.FinanceTag ToDomain(this C.FinanceTag x) => new() { Id = x.Id, Name = x.Name, Type = (E.TransactionType)x.Type };
    public static C.FarmTransaction ToContract(this D.FarmTransaction x) => new() { Id = x.Id, FarmId = x.FarmId, Type = (C.TransactionType)x.Type, Amount = x.Amount, TransactionDate = x.TransactionDate, Tag = x.Tag, Description = x.Description };
    public static D.FarmTransaction ToDomain(this C.FarmTransaction x) => new() { Id = x.Id, FarmId = x.FarmId, Type = (E.TransactionType)x.Type, Amount = x.Amount, TransactionDate = x.TransactionDate, Tag = x.Tag, Description = x.Description };
    public static C.ProcessStep ToContract(this D.ProcessStep x) => new() { Id = x.Id, Order = x.Order, Title = x.Title, Description = x.Description, DayOffset = x.DayOffset };
    public static D.ProcessStep ToDomain(this C.ProcessStep x) => new() { Id = x.Id, Order = x.Order, Title = x.Title, Description = x.Description, DayOffset = x.DayOffset };
    public static C.FarmProcessExtraStep ToContract(this D.FarmProcessExtraStep x) => new() { Id = x.Id, FarmId = x.FarmId, WorkProcessId = x.WorkProcessId, Order = x.Order, Title = x.Title, Description = x.Description, DayOffset = x.DayOffset };
    public static D.FarmProcessExtraStep ToDomain(this C.FarmProcessExtraStep x) => new() { Id = x.Id, FarmId = x.FarmId, WorkProcessId = x.WorkProcessId, Order = x.Order, Title = x.Title, Description = x.Description, DayOffset = x.DayOffset };
    public static C.FarmProcessRunStep ToContract(this D.FarmProcessRunStep x) => new() { Id=x.Id, Order=x.Order, Title=x.Title, Description=x.Description, DayOffset=x.DayOffset, IsFarmExtra=x.IsFarmExtra, CompletedAt=x.CompletedAt };
    public static C.FarmProcessRun ToContract(this D.FarmProcessRun x) => new() { Id=x.Id, FarmId=x.FarmId, WorkProcessId=x.WorkProcessId, ProcessName=x.ProcessName, SeasonName=x.SeasonName, StartDate=x.StartDate, CompletedAt=x.CompletedAt, Steps=x.Steps.OrderBy(s=>s.Order).Select(ToContract).ToList() };
    public static C.WorkProcess ToContract(this D.WorkProcess x) => new() { Id = x.Id, Name = x.Name, CropType = x.CropType, FarmIds = x.Farms.Select(f => f.Id).ToList(), Description = x.Description, IsDeleted = x.IsDeleted, DeletedAt = x.DeletedAt, Steps = x.Steps.Select(ToContract).ToList() };
    public static D.WorkProcess ToDomain(this C.WorkProcess x) => new() { Id = x.Id, Name = x.Name, CropType = x.CropType, Description = x.Description, IsDeleted = x.IsDeleted, DeletedAt = x.DeletedAt, Steps = x.Steps.Select(ToDomain).ToList() };
    public static C.Supply ToContract(this D.Supply x) => new() { Id = x.Id, Type = (C.SupplyType)x.Type, Name = x.Name, Brand = x.Brand, ImageUrl = x.ImageUrl, Quantity = x.Quantity, Unit = x.Unit, MinimumStock = x.MinimumStock, ExpiryDate = x.ExpiryDate, Usage = x.Usage, Notes = x.Notes };
    public static D.Supply ToDomain(this C.Supply x) => new() { Id = x.Id, Type = (E.SupplyType)x.Type, Name = x.Name, Brand = x.Brand, ImageUrl = x.ImageUrl, Quantity = x.Quantity, Unit = x.Unit, MinimumStock = x.MinimumStock, ExpiryDate = x.ExpiryDate, Usage = x.Usage, Notes = x.Notes };
    public static C.Worker ToContract(this D.Worker x) => new() { Id = x.Id, FarmId = x.FarmId, FullName = x.FullName, Phone = x.Phone, Address = x.Address, Role = x.Role, DailyWage = x.DailyWage, StartDate = x.StartDate, IsActive = x.IsActive, Notes = x.Notes };
    public static D.Worker ToDomain(this C.Worker x) => new() { Id = x.Id, FarmId = x.FarmId, FullName = x.FullName, Phone = x.Phone, Address = x.Address, Role = x.Role, DailyWage = x.DailyWage, StartDate = x.StartDate, IsActive = x.IsActive, Notes = x.Notes };
}
