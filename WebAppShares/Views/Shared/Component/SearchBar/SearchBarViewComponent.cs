using Microsoft.AspNetCore.Mvc;

namespace WebAppShares.Views.Shared.Component.SearchBar
{
    public class SearchBarViewComponent : ViewComponent
    {
        public SearchBarViewComponent()
        {

        }

        public IViewComponentResult Invoke(Spager SearchPager)
        {
            return View("default", SearchPager);
        }

    }
}
