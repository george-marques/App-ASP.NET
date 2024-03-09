using Learning.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Learning.App.Extensions
{
    [ViewComponent(Name = "Summary")]
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notifier;

        public SummaryViewComponent(INotificador notifier)
        {
            _notifier = notifier;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await Task.FromResult(_notifier.ObterNotificacoes());

            notifications.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Mensagem));

            return View();
        }
    }
}
