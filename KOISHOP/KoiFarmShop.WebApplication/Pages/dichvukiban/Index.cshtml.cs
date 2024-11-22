using KoiFarmShop.Repositories.Entities;
using KoiFarmShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoiFarmShop.WebApplication.Pages.dichvukiban
{
	public class IndexModel : PageModel
	{
		private readonly IConsignmentRequestService _consignmentRequestService;

		public IndexModel(IConsignmentRequestService consignmentRequestService)
		{
			_consignmentRequestService = consignmentRequestService;
		}

		public IList<ConsignmentRequest> ConsignmentRequest { get; set; }

		public async Task OnGetAsync()
		{
			ConsignmentRequest = await _consignmentRequestService.GetAllConsignmentRequestsAsync();
		}
	}
}
