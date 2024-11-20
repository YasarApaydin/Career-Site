using Kariyer.Helpers;
using Kariyer.Models;
using Kariyer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace kariyer.Controllers
{
    public class IsVerenController : Controller
    {
        private readonly KariyerContext context;
        private readonly IWebHostEnvironment env;
        public IsVerenController(KariyerContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }

        [HttpGet]
        public async Task<IActionResult> Is_Veren_Kayit(string kullaniciTur)
        {
            var sehirler = await context.Sehirs
                .OrderBy(x => x.Ad)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Ad
                }).ToListAsync();

            ViewBag.KullaniciTur = kullaniciTur;
            var viewModel = new IsVerenViewModel
            {
                MevcutPersonelSayi = await context.MevcutPersonelSayis.OrderBy(x => x.Sira).ToListAsync(),
                Sehirler = sehirler
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Is_Veren_Kayit(IsVerenViewModel viewModel)
        {
            if (!PasswordHelper.ValidatePassword(viewModel.Parola!))
            {
                ModelState.AddModelError("Parola", "Şifre en az 8 karakter uzunluğunda olmalı ve bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");
                await LoadFormData(viewModel);
                return View(viewModel);
            }
            if (!ModelState.IsValid)
            {
                await LoadFormData(viewModel);

                return View(viewModel);
            }

            if (viewModel.Parola != viewModel.ParolaTekrar)
            {
                ModelState.AddModelError("ParolaTekrar", "Şifreler eşleşmiyor.");
                await LoadFormData(viewModel);

                return View(viewModel);
            }

            var kullanici = await context.Kullanicis.FirstOrDefaultAsync(k => k.Id == viewModel.KullaniciId);
            if (kullanici == null)
            {
                var kullaniciTur = await context.KullaniciTurs.FirstOrDefaultAsync(k => k.Ad == viewModel.KullaniciTur);
                if (kullaniciTur == null)
                {
                    ModelState.AddModelError("", "Geçersiz kullanıcı türü.");
                    await LoadFormData(viewModel);
                    return View(viewModel);
                }
                kullanici = new Kullanici
                {
                    Eposta = viewModel.Eposta,
                    KullaniciTurId = kullaniciTur.Id,
                    TelefonNo = viewModel.Telefon,
                    Parola = PasswordHelper.HashPassword(viewModel.Parola!),
                    Sil = false
                };
                context.Kullanicis.Add(kullanici);
                await context.SaveChangesAsync();
            }
            else
            {
                kullanici.Parola = PasswordHelper.HashPassword(viewModel.Parola!);
                context.Kullanicis.Update(kullanici);
                await context.SaveChangesAsync();

            }

            var isVeren = new IsVeren
            {
                KullaniciId = kullanici!.Id,
                AdSoyad = viewModel.AdSoyad,
                Adres = viewModel.Adres,
                VergiDairesi = viewModel.VergiDairesi,
                VergiNo = viewModel.VergiNo,
                MevcutPersonelSayiId = viewModel.MevcutPersonelSayiId,
                Foto = viewModel.Foto,
                IlceId = viewModel.IlceId,
                FirmaUnvan = viewModel.FirmaUnvan,
                FirmaWebAdres = viewModel.FirmaWebAdres,
                Sil = false
            };

            if (viewModel.FotoDosya != null && viewModel.FotoDosya.Length > 0)
            {
                var fileName = Path.GetFileName(viewModel.FotoDosya.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.FotoDosya.CopyToAsync(stream);
                }
                isVeren.Foto = fileName;
            }
            context.IsVerens.Add(isVeren);
            await context.SaveChangesAsync();

            return RedirectToAction("Giris", "Home");
        }

        [HttpGet]
        public IActionResult Havuz()
        {
            return View();
        }

        private async Task LoadFormData(IsVerenViewModel viewModel)
        {
            viewModel.MevcutPersonelSayi = await context.MevcutPersonelSayis.OrderBy(x => x.Sira).ToListAsync();
            viewModel.Sehirler = await context.Sehirs.OrderBy(x => x.Ad).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Ad }).ToListAsync();
        }

        [Authorize(Roles = "IsVeren")]
        public async Task<IActionResult> Ilan_Basvurular(int ilanNo)
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

            var ilan = await context.IsIlanlars
           .Include(i => i.IsBasvurus)
               .ThenInclude(b => b.IsArayan) 
               .ThenInclude(a => a!.Meslek)
           .FirstOrDefaultAsync(i => i.IlanNo == ilanNo);

            if (ilan == null)
            {
                return NotFound();
            }

            var basvurular = ilan.IsBasvurus
                .Select(b => new IlanBasvuruViewModel
                {
                    AdSoyad = b.IsArayan?.AdSoyad,
                    Eposta = b.IsArayan?.Kullanici?.Eposta,
                    Foto = b.IsArayan?.Foto,
                    Meslek = b.IsArayan?.Meslek?.Ad,
                    KullaniciIdEncrypted = EncryptionHelper.Encrypt(b.IsArayan!.Id.ToString()),
                    BasvuruTarihi = DateOnly.FromDateTime(DateTime.Now)
                }).ToList();

            var model = new IlanBasvuruViewModel
            {
                kullaniciTür = kullanici.KullaniciTurId!.Value,
                AdSoyad = kullanici.IsArayans.FirstOrDefault()?.AdSoyad ?? kullanici.IsVerens.FirstOrDefault()?.AdSoyad,
                Eposta = kullanici.Eposta,
                Foto = kullanici.IsArayans.FirstOrDefault()?.Foto ?? kullanici.IsVerens.FirstOrDefault()?.Foto,
                IsArayan = kullanici.IsArayans.Any(),
                IsVeren = kullanici.IsVerens.Any(),
                IlanBasvuru = basvurular
            };

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetIlceler(Guid sehirId)
        {
            var ilceler = await context.Ilces
                .Where(ilce => ilce.SehirId == sehirId)
                .OrderBy(x => x.Ad)
                .Select(ilce => new { ilce.Id, ilce.Ad })
                .ToListAsync();
            return Json(ilceler);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Yeni_Is_Ilani()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {

                return RedirectToAction("Error", "Home");

            }

            if (Guid.TryParse(userId, out Guid userGuid))
            {
                var isVeren = await context.IsVerens.FirstOrDefaultAsync(i => i.KullaniciId == userGuid);
                if (isVeren == null)
                {

                    return RedirectToAction("Error", "Home");
                }
                var kullanici = await context.Kullanicis.FirstOrDefaultAsync(i => i.Id == userGuid);

                if (kullanici == null)
                {

                    return RedirectToAction("Error", "Home");
                }

                var viewModel = new IsIlaniViewModel
                {
                    Cinsiyetler = await context.Cinsiyets.OrderBy(i => i.Id).ToListAsync(),
                    IsVerenler = isVeren,
                    Egitimler = await context.Egitims.OrderBy(x => x.Sira).ToListAsync(),
                    Pozisyonlar = await context.Pozisyons.OrderBy(x => x.Sira).ToListAsync(),
                    Deneyimler = await context.Deneyims.OrderBy(x => x.Sira).ToListAsync(),
                    CalismaSekilleri = await context.CalismaSeklis.OrderBy(x => x.Sira).ToListAsync(),
                    Kullanici = kullanici,
                    Eposta = kullanici?.Eposta
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PozisyonAra(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new List<SelectListItem>()); // Boş arama terimi için boş liste döndür
            }

            var pozsiyon = await context.Pozisyons
                .Where(m => m.Ad.ToLower().Contains(searchTerm.ToLower()) && !m.Sil) // Silinmiş meslekleri hariç tut
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Ad
                })
                .ToListAsync();

            return Json(pozsiyon);
        }

        [HttpPost]
        [Authorize(Roles = "IsVeren")]
        public async Task<IActionResult> Yeni_Is_Ilani(IsIlaniViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadForm(viewModel);
                return View(viewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userGuid))
            {
                return RedirectToAction("Error", "Home");
            }

            var isVeren = await context.IsVerens.FirstOrDefaultAsync(i => i.KullaniciId == userGuid);
            if (isVeren == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var maxIlanNo = await context.IsIlanlars.MaxAsync(i => (int?)i.IlanNo) ?? 0;
            var yeniIlanNo = maxIlanNo + 1;
            var isIlan = new IsIlanlar
            {

                CalismaSekliId = viewModel.CalismaSekliId,
                IsVerenId = isVeren.Id,
                DeneyimId = viewModel.DeneyimId,
                EgitimId = viewModel.EgitimId,
                PosizyonId = viewModel.PozisyonId,
                IlanBaslik = viewModel.IlanBaslik,
                Maas = viewModel.Maas,
                CalismaKonumu = viewModel.CalismaKonumu,
                YasAralık = viewModel.YasAralık,
                IlanAciklama = viewModel.IlanAciklama,
                BasTarih = DateTime.Now,
                AlinacakPersonelSayi = viewModel.AlinacakPersonelSayi,
                IlanNo = yeniIlanNo,
                CinsiyetId = viewModel.CinsiyetId,
                Sil = false
            };

            context.IsIlanlars.Add(isIlan);
            await context.SaveChangesAsync();
            var nitelikler = viewModel.NitelikAd!.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var nitelikAd in nitelikler)
            {
                int maxNitelik = await context.Niteliks.CountAsync(n => !n.Sil);
                var nitelik = new Nitelik
                {
                    Ad = nitelikAd.Trim(),
                    Sira = maxNitelik + 1,
                    Sil = false
                };
                context.Niteliks.Add(nitelik);
                await context.SaveChangesAsync();
                int maxSira = await context.IsIlanlarNiteliks
                    .Where(x => x.IsIlanlarId == viewModel.IsIlanlarId)
                    .MaxAsync(x => (int?)x.Sira) ?? 0;
                var ilanNitelik = new IsIlanlarNitelik
                {
                    IsIlanlarId = isIlan.Id,
                    NitelikId = nitelik.Id,
                    Sira = maxSira + 1,
                    Sil = false
                };
                context.IsIlanlarNiteliks.Add(ilanNitelik);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Firma_Ilanlar", "IsVeren");
        }

        private async Task LoadForm(IsIlaniViewModel viewModel)
        {
            viewModel.Cinsiyetler = await context.Cinsiyets.OrderBy(x => x.Id).ToListAsync();
            viewModel.Egitimler = await context.Egitims.OrderBy(x => x.Sira).ToListAsync();
            viewModel.Pozisyonlar = await context.Pozisyons.OrderBy(x => x.Sira).ToListAsync();
            viewModel.Deneyimler = await context.Deneyims.OrderBy(x => x.Sira).ToListAsync();
            viewModel.CalismaSekilleri = await context.CalismaSeklis.OrderBy(x => x.Sira).ToListAsync();
        }





        [Authorize(Roles = "IsVeren")]
        public async Task<IActionResult> Firma_Ilanlar(int pageNumber = 1, int pageSize = 5)
        {
            var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var kullanici = await context.Kullanicis
                .Include(k => k.IsArayans)
                .Include(k => k.IsVerens)
                .Include(k => k.Profils)
                .FirstOrDefaultAsync(k => k.Id == kullaniciId);
            var isveren = kullanici!.IsVerens.FirstOrDefault();

            if (isveren == null)
            {
                return NotFound();
            }
            var toplamSayfaSayisi = await context.IsIlanlars.CountAsync(x => x.IsVerenId == isveren.Id);
            var ilanlar = await context.IsIlanlars.Include(x => x.Posizyon)
        .Where(x => x.IsVerenId == isveren!.Id).OrderBy(x => x.BasTarih).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(k => new IsIlaniViewModel
        {
            Goruntulenme = k.Goruntulenme,
            IlanBaslik = k.IlanBaslik,
            IlanNo = k.IlanNo,
            BasTarih = k.BasTarih,
            PozisyonAd = k.Posizyon!.Ad,
            Maas = k.Maas,
            YasAralık = k.YasAralık,
            basvuruSay = k.IsBasvurus.Count(),
            IlanIdEncrypted = EncryptionHelper.Encrypt(k.Id.ToString())
        }).ToListAsync();

            var model = new IsIlaniViewModel
            {
                kullaniciTür = kullanici!.KullaniciTurId!.Value,
                AdSoyad = kullanici.IsArayans.FirstOrDefault()?.AdSoyad ?? kullanici.IsVerens.FirstOrDefault()?.AdSoyad,
                Eposta = kullanici.Eposta,
                ToplamSayfa = (int)Math.Ceiling((double)toplamSayfaSayisi / pageSize),
                MevcutSayfa = pageNumber,
                Foto = kullanici.IsArayans.FirstOrDefault()?.Foto ?? kullanici.IsVerens.FirstOrDefault()?.Foto,
                IsArayan = kullanici.IsArayans.Any(),
                IsVeren = kullanici.IsVerens.Any(),
                IsIlanlari = ilanlar
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "IsVeren")]
        public IActionResult DeleteIlan(int IlanNo)
        {
            var ilan = context.IsIlanlars
                                .Include(i => i.IsIlanlarNiteliks) // Nitelikler
                                .ThenInclude(i => i.Nitelik)
                                .Include(i => i.IsBasvurus) // Başvurular
                                .Include(i => i.Begenis) // Beğeniler
                                .ThenInclude(i => i.Kullanici)
                                .FirstOrDefault(i => i.IlanNo == IlanNo);

            if (ilan != null)
            {
                foreach (var nitelik in ilan.IsIlanlarNiteliks.ToList())
                {
                    context.IsIlanlarNiteliks.Remove(nitelik);
                }
                foreach (var basvuru in ilan.IsBasvurus.ToList())
                {
                    context.IsBasvurus.Remove(basvuru);
                }

                foreach (var begeni in ilan.Begenis.ToList())
                {
                    context.Begenis.Remove(begeni);
                }
               
                context.IsIlanlars.Remove(ilan);
               
                context.SaveChanges();
            }

            return RedirectToAction("Profil", "Hesap");
        }

    }
}
