using System.Threading.Tasks;

namespace Gear.Manager.Core.BackgroundProcesses.Infrastructure
{
    public interface IScopeBase<TScope>
    {
        Task ExecuteProcess();
    }
}
