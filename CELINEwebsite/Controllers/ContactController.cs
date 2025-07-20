using CELINEwebsite.Models;
using CELINEwebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CELINEwebsite.Controllers
{
    public class ContactController : Controller
    {
        private readonly IStringLocalizer<ContactController> _localizer;
        private readonly EmailService _emailService;
        private readonly ILogger<ContactController> _logger;

        // Constructor المعدل
        public ContactController(
            IStringLocalizer<ContactController> localizer,
            EmailService emailService,
            ILogger<ContactController> logger)
        {
            _localizer = localizer;
            _emailService = emailService;
            _logger = logger;
        }

        // بقية الأكود (Actions) تبقى كما هي
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to send email...");
                    await _emailService.SendEmailAsync(model.Name, model.Email, model.Message);
                    TempData["Message"] = _localizer["Thank you for your message! We'll contact you soon"].ToString();
                    _logger.LogInformation("Email sent successfully.");
                    // إعادة نموذج فارغ بدلاً من التوجيه
                    ModelState.Clear();
                    return View(new ContactFormModel());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Email sending failed");
                    TempData["ErrorMessage"] = _localizer["There was an error sending your message. Please try again later"].ToString();
                    // يمكنك إضافة رسالة تفصيلية للأخطاء التقنية إذا لزم الأمر
                }

                ////استخدمى هذا السطر فى حال رغبتى بإعادة التوجيه إلى الصفحة الرئيسية/////// return RedirectToAction("Index", "Home");
              

                // إذا كان هناك أخطاء في النموذج، يعرض نفس الصفحة مع البيانات المدخلة
                return View(model);
            }
      
            return View(model);
        }
    }
}