using Kariyer.Helpers;
using Kariyer.Models;
using Kariyer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
namespace kariyer.Controllers
{
    public class HomeController : Controller
    {
        private readonly KariyerContext context;
        public HomeController(KariyerContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Giris(string? kullaniciTur)
        {
            ViewBag.KullaniciTur = kullaniciTur;
            return View();


        }

        [HttpPost]
        public async Task<IActionResult> Giris(Kullanici model, string kullaniciTur)
        {
            var user = await context.Kullanicis.FirstOrDefaultAsync(a => a.Eposta == model.Eposta);
            if (user != null && PasswordHelper.VerifyPassword(model.Parola!, user.Parola!))
            {
                var adSoyad = await context.IsArayans
                          .Where(i => i.KullaniciId == user.Id)
                          .Select(i => i.AdSoyad)
                          .FirstOrDefaultAsync() ?? await context.IsVerens
                          .Where(i => i.KullaniciId == user.Id)
                          .Select(i => i.AdSoyad)
                          .FirstOrDefaultAsync();
                var kullaniciTurAdi = await context.KullaniciTurs
                    .Where(t => t.Id == user.KullaniciTurId)
                    .Select(t => t.Ad)
                    .FirstOrDefaultAsync();
                var claims = new List<Claim>
                {  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                      new Claim(ClaimTypes.Name, user.Eposta!),
                      new Claim("AdSoyad", adSoyad ?? "Bilgi Yok"),
                      new Claim(ClaimTypes.Role, kullaniciTurAdi ?? "Bilgi Yok")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                ViewBag.KullaniciAd = adSoyad ?? "Bilgi Yok";
                return RedirectToAction("Anasayfa", "Home");
            }
            ModelState.AddModelError("", "Geçersiz giriş denemesi.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Ilan_Detay()
        {
            var ilanId = HttpContext.Session.GetString("CurrentIlanId");
            if (string.IsNullOrEmpty(ilanId))
            {
                return RedirectToAction("Error", "Home");
            }

            try
            {
                var ilanIdDecrypted = EncryptionHelper.Decrypt(ilanId);
                var ilanIdGuid = new Guid(ilanIdDecrypted);
                var kullaniciClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(kullaniciClaim, out Guid kullaniciId);
                var isArayan = await context.IsArayans.FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);
                var isVeren = await context.IsVerens.FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);
                if (isVeren != null)
                {
                    ViewBag.isVeren = true;
                }
                if (isArayan != null)
                {
                    var basvuruVarMi = await context.IsBasvurus
                        .AnyAsync(b => b.IsArayanId == isArayan.Id && b.IsIlanlarId == ilanIdGuid);
                    ViewBag.BasvuruYapildiMi = basvuruVarMi;
                }
                else
                {
                    ViewBag.BasvuruYapildiMi = false;
                }
                var ilan = await context.IsIlanlars.FindAsync(ilanIdGuid);
                if (ilan == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                if (ilan.Goruntulenme == null)
                {
                    ilan.Goruntulenme = 0;
                }
                ilan.Goruntulenme++;
                context.IsIlanlars.Update(ilan);
                await context.SaveChangesAsync();
                var ilanDetay = await context.IsIlanlars
           .Where(i => i.Id == ilanIdGuid)
           .Select(x => new
           {
               x.Id,
               x.IlanBaslik,
               x.Maas,
               x.CalismaKonumu,
               x.YasAralık,
               x.IlanAciklama,
               x.IlanNo,
               x.AlinacakPersonelSayi,
               x.BasTarih,
               x.IsVerenId,
               Cinsiyet = context.Cinsiyets.Where(a => a.Id == x.CinsiyetId).Select(t => t.Ad).FirstOrDefault(),
               FirmaAd = context.IsVerens.Where(a => a.Id == x.IsVerenId).Select(t => t.FirmaUnvan).FirstOrDefault(),
               Logo = context.IsVerens.Where(a => a.Id == x.IsVerenId).Select(t => t.Foto).FirstOrDefault(),
               WebAdresi = context.IsVerens.Where(a => a.Id == x.IsVerenId).Select(t => t.FirmaWebAdres).FirstOrDefault(),
               Pozisyon = context.Pozisyons.Where(a => a.Id == x.PosizyonId).Select(t => t.Ad).FirstOrDefault(),
               Deneyim = context.Deneyims.Where(a => a.Id == x.DeneyimId).Select(t => t.Ad).FirstOrDefault(),
               Egitim = context.Egitims.Where(a => a.Id == x.EgitimId).Select(t => t.Ad).FirstOrDefault(),
               Sektor = context.SektorPozisyons.Where(a => a.PozisyonId == x.PosizyonId).Select(a => a.Sektor!.Ad).FirstOrDefault(),
               Nitelikler = context.IsIlanlarNiteliks.Where(a => a.IsIlanlarId == x.Id).Select(n => n.Nitelik!.Ad).ToList(),
               CalismaSekli = context.CalismaSeklis.Where(a => a.Id == x.CalismaSekliId).Select(t => t.Ad).FirstOrDefault(),
               IlanIdEncrypted = EncryptionHelper.Encrypt(x.Id.ToString())
           }).FirstOrDefaultAsync();
                if (ilanDetay == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                var digerIsIlanlari = await context.IsIlanlars
                    .Where(i => i.IlanNo != ilanDetay.IlanNo)
                    .Select(x => new DigerIsIlan
                    {
                        IlanIdEncrypted = EncryptionHelper.Encrypt(x.Id.ToString()),
                        FirmaAd = x.IsVeren != null ? x.IsVeren.FirmaUnvan : "Bilinmiyor",
                        IlanBaslik = x.IlanBaslik,
                        IlanNo = x.IlanNo,
                        CalismaSekli = context.CalismaSeklis.Where(a => a.Id == x.CalismaSekliId).Select(t => t.Ad).FirstOrDefault(),
                        Pozisyon = context.Pozisyons.Where(a => a.Id == x.PosizyonId).Select(t => t.Ad).FirstOrDefault(),
                        Logo = context.IsVerens.Where(a => a.Id == x.IsVerenId).Select(t => t.Foto).FirstOrDefault(),
                        CalismaKonumu = x.CalismaKonumu,
                        Maas = x.Maas,
                        BasTarih = x.BasTarih
                    })
                    .ToListAsync();
                var FirmaDigerIsIlanlari = await context.IsIlanlars
                   .Where(i => i.IsVerenId == ilanDetay.IsVerenId && i.IlanNo != ilanDetay.IlanNo)
                   .Select(x => new DigerIsIlan
                   {
                       IlanIdEncrypted = EncryptionHelper.Encrypt(x.Id.ToString()),
                       FirmaAd = x.IsVeren != null ? x.IsVeren.FirmaUnvan : "Bilinmiyor",
                       IlanBaslik = x.IlanBaslik,
                       IlanNo = x.IlanNo,
                       CalismaSekli = context.CalismaSeklis.Where(a => a.Id == x.CalismaSekliId).Select(t => t.Ad).FirstOrDefault(),
                       Pozisyon = context.Pozisyons.Where(a => a.Id == x.PosizyonId).Select(t => t.Ad).FirstOrDefault(),
                       Logo = context.IsVerens.Where(a => a.Id == x.IsVerenId).Select(t => t.Foto).FirstOrDefault(),
                       CalismaKonumu = x.CalismaKonumu,
                       Maas = x.Maas,
                       BasTarih = x.BasTarih
                   })
                   .ToListAsync();
                var viewModel = new IlanDetayViewModel
                {
                    IlanBaslik = ilanDetay.IlanBaslik,
                    IlanIdEncrypted = ilanDetay.IlanIdEncrypted,
                    Maas = ilanDetay.Maas,
                    CalismaKonumu = ilanDetay.CalismaKonumu,
                    YasAralık = ilanDetay.YasAralık,
                    IlanAciklama = ilanDetay.IlanAciklama,
                    IlanNo = ilanDetay.IlanNo ?? 0,
                    AlinacakPersonelSayi = ilanDetay.AlinacakPersonelSayi ?? 0,
                    Cinsiyet = ilanDetay.Cinsiyet,
                    BasTarih = ilanDetay.BasTarih ?? default(DateTime),
                    CalismaSekli = ilanDetay.CalismaSekli,
                    FirmaAd = ilanDetay.FirmaAd,
                    Logo = ilanDetay.Logo,
                    WebAdresi = ilanDetay.WebAdresi,
                    Pozisyon = ilanDetay.Pozisyon,
                    Deneyim = ilanDetay.Deneyim,
                    Egitim = ilanDetay.Egitim,
                    Sektor = ilanDetay.Sektor,
                    Nitelikler = ilanDetay.Nitelikler,
                    DigerIsIlanlari = digerIsIlanlari,
                    DigerIsIlanlari1 = FirmaDigerIsIlanlari
                };

                return View(viewModel);
            }
            catch (FormatException)
            {
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult FavoriEkle(string ilanId)
        {
            if (string.IsNullOrEmpty(ilanId))
            {
                return RedirectToAction("Error", "Home");
            }
            HttpContext.Session.SetString("CurrentIlanId", ilanId);

            return RedirectToAction("Ilan_Detay", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Ilan_Detay(string ilanIda)
        {
            if (string.IsNullOrEmpty(ilanIda))
            {
                return Content("Geçersiz İlan ID.");
            }

            try
            {
                var ilanIdDecrypted = EncryptionHelper.Decrypt(ilanIda);
                if (!Guid.TryParse(ilanIdDecrypted, out var ilanId) || ilanId == Guid.Empty)
                {
                    return Content("Geçersiz İlan ID.");
                }
                var kullaniciClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(kullaniciClaim) || !Guid.TryParse(kullaniciClaim, out Guid kullaniciId))
                {
                    return Content("Kullanıcı ID alınamadı.");
                }
                var isArayan = await context.IsArayans.FirstOrDefaultAsync(a => a.KullaniciId == kullaniciId);
                if (isArayan == null)
                {
                    return Content("İş arayan kaydı bulunamadı.");
                }
                var isVerenMi = await context.IsVerens.AnyAsync(v => v.KullaniciId == kullaniciId);
                if (isVerenMi)
                {
                    return Content("İşverenler başvuruda bulunamaz.");
                }
                var basvuruVarMi = await context.IsBasvurus
                    .AnyAsync(b => b.IsArayanId == isArayan.Id && b.IsIlanlarId == ilanId);
                if (basvuruVarMi)
                {
                    return Content("Bu ilana zaten başvuruda bulundunuz.");
                }
                var ilan = await context.IsIlanlars.FirstOrDefaultAsync(x => x.Id == ilanId);
                if (ilan == null)
                {
                    return Content("İlan bulunamadı.");
                }
                var yeniBasvuru = new IsBasvuru
                {
                    IsIlanlarId = ilanId,
                    IsArayanId = isArayan.Id,
                    IsVerenId = ilan.IsVerenId,
                    Sil = false
                };
                try
                {
                    context.IsBasvurus.Add(yeniBasvuru);
                    await context.SaveChangesAsync();
                    return Content("İşe başarıyla başvurdunuz!");
                }
                catch (DbUpdateException )
                {
                    return Content("Bir hata oluştu.");
                }
            }
            catch (Exception )
            {
                return Content("Geçersiz İlan ID.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Anasayfa(string? meslekAnahtarKelime, string? calismaSekliAd, string? kategoriAd)
        {
            var calismaSekliListesi = await context.CalismaSeklis.OrderBy(x => x.Sira)
                .Where(cs => !cs.Sil)
                .Select(cs => new SelectListItem
                {
                    Value = cs.Ad,
                    Text = cs.Ad
                }).ToListAsync();

            var kategoriListesi = await context.Kategoris.OrderBy(x => x.Sira)
                .Select(sk => new SelectListItem
                {
                    Value = sk.Ad ?? string.Empty,
                    Text = sk.Ad ?? string.Empty
                }).ToListAsync();

            var sorgu = context.IsIlanlars.AsQueryable();

            if (!string.IsNullOrEmpty(meslekAnahtarKelime))
            {
                sorgu = sorgu.Where(i => i.IlanBaslik!.Contains(meslekAnahtarKelime));
            }

            if (!string.IsNullOrEmpty(calismaSekliAd))
            {
                var calismaSekli = await context.CalismaSeklis
                    .Where(cs => cs.Ad == calismaSekliAd)
                    .Select(cs => cs.Id)
                    .FirstOrDefaultAsync();

                if (calismaSekli != Guid.Empty)
                {
                    sorgu = sorgu.Where(i => i.CalismaSekliId == calismaSekli);
                }
            }

            if (!string.IsNullOrEmpty(kategoriAd))
            {
                var kategoriId = await context.Kategoris
                    .Where(k => k.Ad == kategoriAd)
                    .Select(k => k.Id)
                    .FirstOrDefaultAsync();

                if (kategoriId != Guid.Empty)
                {
                    var sektorIds = await context.SektorKategoris
                        .Where(sk => sk.KategoriId == kategoriId)
                        .Select(sk => sk.SektorId)
                        .ToListAsync();

                    var pozisyonIds = await context.SektorPozisyons
                        .Where(sk => sektorIds.Contains(sk.SektorId))
                        .Select(sk => sk.PozisyonId)
                        .ToListAsync();

                    sorgu = sorgu.Where(i => pozisyonIds.Contains(i.PosizyonId!.Value));
                }
            }

            var isIlanlar = await sorgu.OrderByDescending(x => x.BasTarih)
                .Select(i => new IsIlaniViewModel
                {
                    IlanBaslik = i.IlanBaslik,
                    CalismaKonumu = i.CalismaKonumu,
                    IlanNo = i.IlanNo,
                    BasTarih = i.BasTarih,
                    FirmaAd = i.IsVeren!.FirmaUnvan,
                    Foto = i.IsVeren!.Foto,
                    CalismaSekli = i.CalismaSekli!.Ad,
                    IlanIdEncrypted = EncryptionHelper.Encrypt(i.Id.ToString())
                }).ToListAsync();

            var KayitliKullanici = context.Kullanicis.Count();
            var UyeFirma = context.IsVerens.Count();
            var AktifIsIlanlari = context.IsIlanlars.Count();
            var viewModel = new AnasayfaViewModel
            {
                KayitliKullanici = KayitliKullanici,
                UyeFirma = UyeFirma,
                AktifIsIlanlari = AktifIsIlanlari,
                CalismaSekliListesi = calismaSekliListesi,
                KategoriListesi = kategoriListesi,
                IsIlanlari = isIlanlar,
                MeslekAnahtarKelime = meslekAnahtarKelime,
                CalismaSekliAd = calismaSekliAd,
                KategoriAd = kategoriAd
            };

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Is_Ilanlari(string? meslekAnahtarKelime, string? calismaSekliAd, string? kategoriAd)
        {
            var kullaniciClaim = User.Identity!.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var kullaniciId = string.IsNullOrEmpty(kullaniciClaim) ? Guid.Empty : Guid.Parse(kullaniciClaim);
            var calismaSekliListesi = await context.CalismaSeklis.OrderBy(x => x.Sira)
              .Where(cs => !cs.Sil)
              .Select(cs => new SelectListItem
              {
                  Value = cs.Ad,
                  Text = cs.Ad
              }).ToListAsync();

            var kategoriListesi = await context.Kategoris.OrderBy(x => x.Sira)
                .Select(sk => new SelectListItem
                {
                    Value = sk.Ad ?? string.Empty,
                    Text = sk.Ad ?? string.Empty
                }).ToListAsync();

            var sorgu = context.IsIlanlars.AsQueryable();
            if (!string.IsNullOrEmpty(meslekAnahtarKelime))
            {
                sorgu = sorgu.Where(i => i.IlanBaslik!.Contains(meslekAnahtarKelime));
            }

            if (!string.IsNullOrEmpty(calismaSekliAd))
            {
                var calismaSekli = await context.CalismaSeklis
                    .Where(cs => cs.Ad == calismaSekliAd)
                    .Select(cs => cs.Id)
                    .FirstOrDefaultAsync();

                if (calismaSekli != Guid.Empty)
                {
                    sorgu = sorgu.Where(i => i.CalismaSekliId == calismaSekli);
                }
            }

            if (!string.IsNullOrEmpty(kategoriAd))
            {
                var kategoriId = await context.Kategoris
                    .Where(k => k.Ad == kategoriAd)
                    .Select(k => k.Id)
                    .FirstOrDefaultAsync();
                if (kategoriId != Guid.Empty)
                {
                    var sektorIds = await context.SektorKategoris
                        .Where(sk => sk.KategoriId == kategoriId)
                        .Select(sk => sk.SektorId)
                        .ToListAsync();
                    var pozisyonIds = await context.SektorPozisyons
                        .Where(sk => sektorIds.Contains(sk.SektorId))
                        .Select(sk => sk.PozisyonId)
                        .ToListAsync();
                    sorgu = sorgu.Where(i => pozisyonIds.Contains(i.PosizyonId!.Value));
                }
            }

            var ilan = await sorgu.OrderByDescending(x => x.BasTarih)
                .Select(i => new IsIlaniViewModel
                {
                    IlanBaslik = i.IlanBaslik,
                    IlanNo = i.IlanNo,
                    CalismaKonumu = i.CalismaKonumu,
                    BasTarih = i.BasTarih,
                    CalismaSekli = context.CalismaSeklis.Where(a => a.Id == i.CalismaSekliId)
                   .Select(t => t.Ad)
                   .FirstOrDefault(),
                    PozisyonAd = context.Pozisyons.Where(a => a.Id == i.PosizyonId)
                   .Select(t => t.Ad)
                   .FirstOrDefault(),
                    FirmaAd = context.IsVerens.Where(a => a.Id == i.IsVerenId)
                   .Select(t => t.FirmaUnvan)
                   .FirstOrDefault(),
                    Foto = context.IsVerens.Where(a => a.Id == i.IsVerenId)
                   .Select(t => t.Foto)
                   .FirstOrDefault(),
                    KategoriAd = context.SektorKategoris
                   .Where(a => context.SektorPozisyons
                   .Where(b => b.PozisyonId == i.PosizyonId)
                   .Select(b => b.SektorId)
                   .Contains(a.SektorId))
                .Select(sk => sk.Kategori!.Ad)
                .FirstOrDefault(),
                    IlanIdEncrypted = EncryptionHelper.Encrypt(i.Id.ToString()),
                    BegeniVarMi = kullaniciId == Guid.Empty ? false : context.Begenis
                     .Any(b => b.IlanId == i.Id && b.KullaniciId == kullaniciId)
                }).ToListAsync();

            var viewModel = new IsIlaniViewModel
            {
                CalismaSekliListesi = calismaSekliListesi,
                KategoriListesi = kategoriListesi,
                IsIlanlari = ilan,
                MeslekAnahtarKelime = meslekAnahtarKelime,
                CalismaSekliAd = calismaSekliAd,
                KategoriAd = kategoriAd
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Begeni_Ekle(int ilanNo)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Giris", "Home");

            }
            var kullaniciClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(kullaniciClaim) || !Guid.TryParse(kullaniciClaim, out Guid kullaniciId))
            {
                return RedirectToAction("Error", "Home");
            }
            var ilan = await context.IsIlanlars.FirstOrDefaultAsync(x => x.IlanNo == ilanNo);
            if (ilan == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var mevcutBegeni = await context.Begenis
        .FirstOrDefaultAsync(b => b.KullaniciId == kullaniciId && b.IlanId == ilan.Id);

            if (mevcutBegeni != null)
            {
                context.Begenis.Remove(mevcutBegeni);
                await context.SaveChangesAsync();
                return Json(new { begeniEklendi = false });
            }
            else
            {
                var begeni = new Begeni
                {
                    KullaniciId = kullaniciId,
                    IlanId = ilan.Id,
                    BegeniTarih = DateTime.Now
                };
                context.Begenis.Add(begeni);
                await context.SaveChangesAsync();
                return Json(new { begeniEklendi = true });
            }
        }
    }
}
