using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface IDialogService
    {
        Task ShowErrorDialogAsync(string title, string message);
    }
}
