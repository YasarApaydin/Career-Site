using Kariyer.Helpers;
using Kariyer.Models;
using Kariyer.Services;
using Kariyer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
public class HesapController : Controller
{
    private readonly KariyerContext context;
    private readonly IEmailSender emailSender;
    public HesapController(KariyerContext _context, IEmailSender _emailSender)
    {
        emailSender = _emailSender;
        context = _context;
    }

    [HttpGet]
    public IActionResult SifreYenile()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SifreYenile(string email)
    {
        var kullanici = context.Kullanicis.FirstOrDefault(u => u.Eposta == email);
        if (kullanici == null)
        {
            ModelState.AddModelError(string.Empty, "Bu e-posta adresiyle kayıtlı bir kullanıcı bulunamadı.");
            return RedirectToAction("Error", "Home");
        }
        var (token, expirationTime) = TokenHelper.GenerateToken();
        kullanici.ParolaSifirla = token;
        HttpContext.Session.SetString("Token", token);
        var expiration = DateTime.UtcNow.AddMinutes(15);
        HttpContext.Session.SetString("TokenExpiration", expiration.ToString());
        await context.SaveChangesAsync();
        var subject = "Şifre Sıfırla";
        var resetlink = Url.Action("SifreSifirlama", "Hesap", null, Request.Scheme);
        var message = $"Şifrenizi sıfırlamak için <a href='{resetlink}'>buraya tıklayın</a>.";
        await emailSender.SendEmailAsync(email, subject, message);

        return View("SifreSifirlaOnayi");
    }

    [Authorize]
    [HttpGet]
    public IActionResult SifreSifirlaOnayi()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public IActionResult SifreSifirlama()
    {
        var sessionToken = HttpContext.Session.GetString("Token");
        var expirationTimeStr = HttpContext.Session.GetString("TokenExpiration");
        if (string.IsNullOrEmpty(sessionToken) || string.IsNullOrEmpty(expirationTimeStr))
        {
            return RedirectToAction("Error", "Home");
        }
        var expirationTime = DateTime.Parse(expirationTimeStr);
        if (!TokenHelper.ValidateToken(sessionToken, sessionToken, expirationTime))
        {
            return RedirectToAction("Error", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SifreSifirlama(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Lütfen gerekli alanları doğru doldurduğunuzdan emin olun.");
            return View(model);
        }
        var sessionToken = HttpContext.Session.GetString("Token");
        var kullanici = context.Kullanicis.FirstOrDefault(x => x.ParolaSifirla == sessionToken);
        if (kullanici == null)
        {
            return RedirectToAction("Error", "Home");
        }
        kullanici.Parola = PasswordHelper.HashPassword(model.NewPassword!);
        kullanici.ParolaSifirla = null;
        await context.SaveChangesAsync();
        return RedirectToAction("SifreOnayi", "Hesap");
    }

    [HttpGet]
    public IActionResult SifreOnayi()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Profil(ProfilViewModel model)
    {
        if (!ModelState.IsValid)
        {

            return RedirectToAction("Error", "Home");
        }
        var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isVeren = await context.IsVerens.FirstOrDefaultAsync(i => i.KullaniciId == kullaniciId);
        if (isVeren == null)
        {
            return RedirectToAction("Error", "Home");
        }

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Profil(int id)
    {
        var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
       
        var kullanici = await context.Kullanicis
            .Include(k => k.IsArayans)
            .Include(k => k.IsVerens)
            .Include(k => k.Profils)
            .FirstOrDefaultAsync(k => k.Id == kullaniciId);
        if (kullanici == null)
        {
            return NotFound();
        }
        var profil = await context.Profils
            .Include(k => k.SosyalBaglanti)
            .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId);
        string meslekAd = "İş Unvanı";
        if (profil?.MeslekId != null)
        {
            var meslek = await context.Mesleks.FindAsync(profil.MeslekId);
            meslekAd = meslek?.Ad ?? meslekAd;
        }
        else
        {
            var isArayan = kullanici.IsArayans.FirstOrDefault();
            if (isArayan != null && isArayan.MeslekId != null)
            {
                var meslek = await context.Mesleks.FindAsync(isArayan.MeslekId);
                meslekAd = meslek?.Ad ?? meslekAd;
            }
        }
        Guid? ilceId = profil?.IlceId;
        if (ilceId == null)
        {
            var isArayan = kullanici.IsArayans.FirstOrDefault();
            ilceId = isArayan?.İlceId;
        }
        var ilceler = await context.Ilces
            .Where(i => i.Sehir.Ad == "Balıkesir" && !i.Sil)
            .Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Ad
            })
            .ToListAsync();
        var isArayan1 = kullanici.IsArayans.FirstOrDefault();
        string deneyimAd = "Deneyim Bilgisi";
        if (profil?.Deneyim == null)
        {
            var isArayan = kullanici.IsArayans.FirstOrDefault();
            if (isArayan != null && isArayan.DeneyimId.HasValue)
            {
                var deneyim = await context.Deneyims.FirstOrDefaultAsync(d => d.Id == isArayan.DeneyimId);
                deneyimAd = deneyim?.Ad ?? deneyimAd;
            }
        }
        else
        {
            deneyimAd = profil.Deneyim;
        }
        var model = new ProfilViewModel
        {
         
            kullaniciTür = kullanici.KullaniciTurId!.Value,
            AdSoyad = kullanici.IsArayans.FirstOrDefault()?.AdSoyad ?? kullanici.IsVerens.FirstOrDefault()?.AdSoyad,
            Eposta = kullanici.Eposta,
            TelefonNo = kullanici.TelefonNo,
            FirmaUnvan = kullanici.IsVerens.FirstOrDefault()?.FirmaUnvan,
            Meslek = meslekAd,
            Meslekler = await context.Mesleks
                .Where(m => !m.Sil)
                .OrderBy(x => x.Ad)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Ad
                }).ToListAsync(),
            IlceId = ilceId,
            Ilceler = ilceler ?? new List<SelectListItem>(),
            Foto = kullanici.IsArayans.FirstOrDefault()?.Foto ?? kullanici.IsVerens.FirstOrDefault()?.Foto,
            IsArayan = kullanici.IsArayans.Any(),
            IsVeren = kullanici.IsVerens.Any(),
            yas = profil?.Yas ?? 0,
            Deneyim = profil?.Deneyim ?? deneyimAd,
            SosyalBaglantiId = profil?.SosyalBaglantiId,
            Facebook = profil?.SosyalBaglanti?.Facebook,
            Twitter = profil?.SosyalBaglanti?.Twitter,
            Linkedin = profil?.SosyalBaglanti?.Linkedin,
            Github = profil?.SosyalBaglanti?.Github
        };
        if (profil != null && profil!.Yas == null && isArayan1?.DogumTar != null)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var birthDate = isArayan1.DogumTar.Value;
            model.yas = currentDate.Year - birthDate.Year;
            if (currentDate < birthDate.AddYears(model.yas))
            {
                model.yas--;
            }
        }

        return View(model);
    }






    [HttpGet]
    public async Task<IActionResult> Ozgecmis()
    {

       var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ozgecmisDurumu = await context.Ozgecmis.FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

      
        var cvDurumu = await context.Cvs.FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

        var model = new ProfilViewModel
        {
           cvVarMi = (ozgecmisDurumu != null || cvDurumu != null)
        };

        return Json(model);

    }



    [HttpPost]
    [Authorize(Roles = "IsVeren")]
    public async Task<IActionResult> GoruntuleKullanici(string userId)
    {
        
        var decryptedUserId = EncryptionHelper.Decrypt(userId);

      
        if (!Guid.TryParse(decryptedUserId, out Guid kullaniciId))
        {
            return BadRequest("Geçersiz kullanıcı ID'si.");
        }


        var cv = await context.Cvs.FirstOrDefaultAsync(c => c.KullaniciId == kullaniciId);
        var ozgecmis = await context.Ozgecmis.FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

        if (cv == null && ozgecmis == null)
        {
            return NotFound("CV ve Özgeçmiş bulunamadı.");
        }

        int cvSay = cv?.OzgecmisSay ?? 0;
        int ozgecmisSay = ozgecmis?.OzgecmisSay ?? 0;



        if (cvSay > ozgecmisSay)
        {

            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CV", cv.Cvpdf);
            if (System.IO.File.Exists(pdfPath))
            {

                var pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);

                Response.Headers.Add("Content-Disposition", "inline; filename=KendiYukledigiOzgecmis.pdf");

                return File(pdfBytes, "application/pdf");


            }
            else
            {
                return NotFound("Kendi yüklediği özgeçmiş bulunamadı.");
            }
        }
        else
        {

            return NotFound();
        }
    }






    [HttpGet]
    public async Task<IActionResult> OzgecmisGoruntule()
    {

        var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
      
        var cv = await context.Cvs.FirstOrDefaultAsync(c => c.KullaniciId == kullaniciId);
        var ozgecmis = await context.Ozgecmis.FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

        if (cv == null && ozgecmis == null)
        {
            return NotFound("CV ve Özgeçmiş bulunamadı.");
        }

        
        int cvSay = cv?.OzgecmisSay ?? 0; 
        int ozgecmisSay = ozgecmis?.OzgecmisSay ?? 0; 

        if (cvSay > ozgecmisSay)
        {
            
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CV", cv.Cvpdf);
            if (System.IO.File.Exists(pdfPath))
            {
               
                var pdfBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);

                Response.Headers.Add("Content-Disposition", "inline; filename=KendiYukledigiOzgecmis.pdf");

                return File(pdfBytes, "application/pdf");

               
            }
            else
            {
                return NotFound("Kendi yüklediği özgeçmiş bulunamadı.");
            }
        }
        else
        {
          
            return RedirectToAction("Detay", "IsArayan");
        }
    }





    [HttpPost]
    public async Task<IActionResult> Yukle(IFormFile pdfFile)
    {
        var kulllaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if(pdfFile == null || pdfFile.Length == 0)
        {
            return BadRequest("Lütfen bir PDF dosyası yükleyin.");
        }

        if(pdfFile.Length > 10 * 1024 * 1024)
        {
            return BadRequest("Dosya boyutu 10 MB'dan büyük olamaz.");
        }

        var eklenti = Path.GetExtension(pdfFile.FileName);
        if(eklenti.ToLower() != ".pdf")
        {
            return BadRequest("Yalnızca PDF dosyaları yüklenebilir.");
        }


        var mevcutCv = await context.Cvs.FirstOrDefaultAsync(c => c.KullaniciId == kulllaniciId );

        if (mevcutCv != null)
        {
            // Mevcut dosyanın fiziksel yolunu alın ve silin
            var mevcutDosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CV", mevcutCv.Cvpdf!);

            try
            {
                if (System.IO.File.Exists(mevcutDosyaYolu))
                {
               
                    System.IO.File.Delete(mevcutDosyaYolu);
                }
            }
            catch (Exception ex)
            {
          
                return StatusCode(500, $"Dosya silinirken bir hata oluştu: {ex.Message}");
            }

 
            context.Cvs.Remove(mevcutCv);
        }

        var mevcutOzgecmis = await context.Ozgecmis.FirstOrDefaultAsync(o => o.KullaniciId == kulllaniciId);

        
        int ozgecmisSay = mevcutOzgecmis?.OzgecmisSay ?? 0;

    
        int yeniOzgecmisSira = ozgecmisSay + 1;



        var benzersizAd = Guid.NewGuid().ToString() + eklenti;
        var yüklenenklasör = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CV");
        var dosyaYolu = Path.Combine( yüklenenklasör,benzersizAd);


        using (var filestream = new FileStream(dosyaYolu, FileMode.Create))
        {
            await pdfFile.CopyToAsync(filestream);
        }



        var Cv = new Cv
        {
            KullaniciId = kulllaniciId,
            Cvpdf = benzersizAd,
            OzgecmisSay =yeniOzgecmisSira,
            OlusturmaTarihi = DateTime.Now,
            Sil = false,
        };
        context.Cvs.Add(Cv);
        await context.SaveChangesAsync();

        return Ok("PDF başarıyla yüklendi.");



    }







    [HttpGet]
    public async Task<IActionResult> MeslekAra(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return Json(new List<SelectListItem>());
        }
        var meslekler = await context.Mesleks
            .Where(m => m.Ad.ToLower().Contains(searchTerm.ToLower()) && !m.Sil)
            .Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Ad
            })
            .ToListAsync();

        return Json(meslekler);
    }

    [HttpGet]
    public IActionResult SilOnay()
    {
        var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var kullanici = context.Kullanicis
            .Include(k => k.IsArayans)
            .Include(k => k.IsVerens)
            .FirstOrDefault(k => k.Id == kullaniciId);
        if (kullanici == null)
        {
            return NotFound();
        }
        string adSoyad = string.Empty;
        if (kullanici.IsArayans.Any())
        {

            adSoyad = kullanici.IsArayans.FirstOrDefault()?.AdSoyad!;
        }
        else if (kullanici.IsVerens.Any())
        {

            adSoyad = kullanici.IsVerens.FirstOrDefault()?.AdSoyad!;
        }
        var viewModel = new ProfilViewModel
        {
            KullaiciId = kullanici.Id.ToString(),
            AdSoyad = adSoyad,
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult SilOnay(ProfilViewModel model)
    {
        var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var kullanici = context.Kullanicis
            .Include(k => k.IsArayans)
            .Include(k => k.IsVerens)
            .ThenInclude(k => k.IsIlanlars)
            .ThenInclude(k => k.IsIlanlarNiteliks)
            .Include(k => k.Profils)
            .FirstOrDefault(k => k.Id == kullaniciId);
        if (kullanici == null)
        {
            return RedirectToAction("Error", "Home");
        }
        if (kullanici.IsArayans.Any())
        {
            context.IsArayans.RemoveRange(kullanici.IsArayans);
        }
        if (kullanici.IsVerens.Any())
        {
            foreach (var isVeren in kullanici.IsVerens)
            {
                if (isVeren.IsIlanlars.Any())
                {
                    foreach (var ilan in isVeren.IsIlanlars)
                    {
                        if (ilan.IsIlanlarNiteliks.Any())
                        {
                            context.IsIlanlarNiteliks.RemoveRange(ilan.IsIlanlarNiteliks);
                        }
                        var begeniler = context.Begenis.Where(b => b.IlanId == ilan.Id);
                        context.Begenis.RemoveRange(begeniler);
                        var basvurular = context.IsBasvurus.Where(b => b.IsIlanlarId == ilan.Id);
                        context.IsBasvurus.RemoveRange(basvurular);
                    }
                    context.IsIlanlars.RemoveRange(isVeren.IsIlanlars);
                }
                context.IsVerens.Remove(isVeren);
            }
        }
        if (kullanici.Profils.Any())
        {
            context.Profils.RemoveRange(kullanici.Profils);
        }
        context.Kullanicis.Remove(kullanici);
        context.SaveChanges();
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

        return RedirectToAction("Giris", "Home");
    }

   
    
    
    [Authorize(Roles = "IsVeren")]
    public IActionResult Ilanlarim()
    {
        return View();
    }

  
    
    
  
    
    
    public IActionResult mesajj()
    {
        return View();
    }
}