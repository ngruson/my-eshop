using Microsoft.AspNetCore.Components;

namespace eShop.AdminApp.Application.Services;

internal class ProgressService<T>
{
    public Action<T> OnProgressChanged;

    public void ReportProgress(T progress)
    {
        OnProgressChanged?.Invoke(progress);
    }
}
