﻿using Cashify.Application.DTOs.Dashboard;
using Cashify.Application.DTOs.User;
using MudBlazor;

namespace Cashify.Components.Pages.Dashboard;

public partial class TransactionDetails
{
    protected override async Task OnInitializedAsync()
    {
        await GetDashboardCount();
    }

    #region User Details
    private GetUserDetailsDto UserDetails { get; set; } = new();
    
    private async Task GetUserDetails()
    {
        UserDetails = await ProfileService.GetUserDetails();
    }
    #endregion
    
    #region Dashboard Count
    private GetDashboardCount DashboardCount { get; set; } = new GetDashboardCount();

    private async Task GetDashboardCount()
    {
        try
        {
            DashboardCount = await DashboardService.GetDashboardCount();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }

    #endregion
}