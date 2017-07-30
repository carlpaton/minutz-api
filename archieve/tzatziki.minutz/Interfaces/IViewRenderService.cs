using System.Threading.Tasks;

namespace tzatziki.minutz.Interfaces
{
	public interface IViewRenderService
	{
		Task<string> RenderToStringAsync(string viewName, object model);
	}
}
