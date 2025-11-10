using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WUPHF.Web.Pages
{
 [IgnoreAntiforgeryToken] // Disable antiforgery for this page
 public class VulnerableEchoModel : PageModel
 {
 [BindProperty]
 public string UserInput { get; set; } = string.Empty;

 public void OnGet()
 {
 // No-op, just show the page
 }

 public void OnPost()
 {
 // UserInput is bound and will be reflected unsanitized in the view
 }
 }
}
