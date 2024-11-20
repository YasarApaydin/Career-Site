using DinkToPdf.Contracts;
using Kariyer.Helpers;
using Kariyer.Models;
using Kariyer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text.Json;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout; 
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font;
using iText.Kernel.Font;

namespace kariyer.Controllers
{
    public class IsArayanController : Controller
    {
        private readonly IConverter converter;
        private readonly KariyerContext context;
        private readonly IWebHostEnvironment env;
        public IsArayanController(KariyerContext _context, IWebHostEnvironment _env, IConverter _converter)
        {
            converter = _converter;
            context = _context;
            env = _env;
        }

        [HttpGet]
        public async Task<IActionResult> Is_Arayan_Kayit(string kullaniciTur)
        {
            var sehirler = await context.Sehirs
        .OrderBy(s => s.Ad)
        .Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.Ad
        })
        .ToListAsync();
            var pozisyonlar = await context.Pozisyons.OrderBy(x => x.Sira).ToListAsync();
            var selectPozisyonlar = pozisyonlar.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Ad
            }).ToList();
            ViewBag.KullaniciTur = kullaniciTur;
            var viewModel = new IsArayanViewModel()
            {
                Pozisyonlar = selectPozisyonlar,
                Meslekler = await context.Mesleks.OrderBy(x => x.Ad).ToListAsync(),
                Cinsiyetler = await context.Cinsiyets.OrderBy(x => x.Id).ToListAsync(),
                Deneyimler = await context.Deneyims.OrderBy(d => d.Sira).ToListAsync(),
                Egitimler = await context.Egitims.Where(x => !x.Sil).ToListAsync(),
                SurucuBelgeler = await context.SurucuBelges.OrderBy(x => x.Sira).ToListAsync(),
                Srcbelgeler = await context.Srcbelges.OrderBy(x => x.Ad).ToListAsync(),
                Uyruklar = await context.Uyruks.OrderBy(x => x.Ad == "Türkiye" ? 0 : 1).ThenBy(x => x.Ad).ToListAsync(),
                Sehirler = sehirler
            };
            if (!string.IsNullOrEmpty(kullaniciTur))
            {
                viewModel.KullaniciTur = kullaniciTur;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Is_Arayan_Kayit(IsArayanViewModel viewModel)
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

            var model = viewModel.MeslekId!.Value;
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
                    Tck = viewModel.Tck,
                    TelefonNo = viewModel.KullaniciBilgiler!.TelefonNo,
                    Eposta = viewModel.KullaniciBilgiler.Eposta,
                    Parola = PasswordHelper.HashPassword(viewModel.Parola!),
                    KullaniciTurId = kullaniciTur.Id,
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

            var isArayan = new IsArayan
            {
                KullaniciId = kullanici.Id,
                AdSoyad = viewModel.AdSoyad,
                DogumTar = viewModel.DogumTar,
                Adres = viewModel.Adres,
                CinsiyetId = viewModel.CinsiyetId,
                EgitimId = viewModel.EgitimId,
                MedeniHal = viewModel.MedeniHal == MedeniHal.Bekar,
                Engelli = viewModel.EngelDurum == EngelDurum.Evet,
                MeslekId = viewModel.MeslekId,
                DeneyimId = viewModel.DeneyimId,
                SrcbelgeId = viewModel.SrcbelgeId,
                SurucuBelgeId = viewModel.SurucuBelgeId,
                UyrukId = viewModel.UyrukId,
                İlceId = viewModel.IlceId,
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
                isArayan.Foto = fileName;
            }
            context.IsArayans.Add(isArayan);
            await context.SaveChangesAsync();
            var pozisyonIdArray = JsonSerializer.Deserialize<string[]>(viewModel.SelectedPozisyonIds![0]); // JSON string'i deseralize ediyoruz

            if (pozisyonIdArray != null && pozisyonIdArray.Length > 0)
            {
                foreach (var pozisyonIdString in pozisyonIdArray)
                {
                    if (Guid.TryParse(pozisyonIdString, out Guid pozisyonId))
                    {
                        var kullaniciPozisyon = new KullaniciPozisyonlar
                        {
                            KullaniciId = kullanici.Id,
                            PozisyonId = pozisyonId,
                            Sil = false
                        };
                        context.KullaniciPozisyonlars.Add(kullaniciPozisyon);
                    }
                    else
                    {
                    }
                }
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Giris", "Home");
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
        public async Task<IActionResult> PozisyonAra(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new List<SelectListItem>());
            }

            var pozsiyon = await context.Pozisyons
                .Where(m => m.Ad.ToLower().Contains(searchTerm.ToLower()) && !m.Sil)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Ad
                })
                .ToListAsync();

            return Json(pozsiyon);
        }



        private async Task LoadFormData(IsArayanViewModel viewModel)
        {
            viewModel.Egitimler = await context.Egitims.Where(x => !x.Sil).ToListAsync();
            viewModel.Meslekler = await context.Mesleks.OrderBy(x => x.Ad).ToListAsync();
            viewModel.Cinsiyetler = await context.Cinsiyets.OrderBy(x => x.Id).ToListAsync();
            viewModel.Deneyimler = await context.Deneyims.OrderBy(d => d.Sira).ToListAsync();
            viewModel.Srcbelgeler = await context.Srcbelges.OrderBy(x => x.Ad).ToListAsync();
            viewModel.SurucuBelgeler = await context.SurucuBelges.OrderBy(x => x.Sira).ToListAsync();
            viewModel.Uyruklar = await context.Uyruks.OrderBy(x => x.Ad).ToListAsync();
            viewModel.Sehirler = await context.Sehirs.OrderBy(x => x.Ad).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Ad }).ToListAsync();

        }



        [HttpGet]
        public async Task<JsonResult> GetIlceler(Guid sehirId)
        {
            var ilceler = await context.Ilces
                .Where(ilce => ilce.SehirId == sehirId).OrderBy(x => x.Ad)
                .Select(ilce => new { ilce.Id, ilce.Ad })
                .ToListAsync();

            return Json(ilceler);
        }

        [HttpGet]
        [Authorize(Roles = "IsArayan")]
        public async Task<IActionResult> Ozgecmis()
        {
            var viewModel = new OzgecmisViewModel()
            {
                Cinsiyetler = await context.Cinsiyets.OrderBy(x => x.Id).ToListAsync(),
                Egitimler = await context.Egitims.Where(x => !x.Sil).ToListAsync(),
                Meslekler = await context.Mesleks.OrderBy(x => x.Ad).ToListAsync(),
                Deneyimler = await context.Deneyims.OrderBy(d => d.Sira).ToListAsync(),
                SurucuBelgeler = await context.SurucuBelges.OrderBy(x => x.Sira).ToListAsync(),
                Srcbelgeler = await context.Srcbelges.OrderBy(x => x.Ad).ToListAsync(),
                MezunuyetBilgiler = await context.Egitims.Where(x => !x.Sil).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detay()
        {
            var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ozgecmis = await context.Ozgecmis
                .Include(o => o.Meslek)
                .Include(o => o.Cinsiyet)
                .Include(o => o.SurucuBelge)
                .Include(o => o.Srcbelge)
                .Include(o => o.Kullanici)
                .Include(o => o.Deneyim)
                .FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

            if (ozgecmis == null)
            {
                return NotFound();
            }

            var egitimler = await context.OzgecmisEgitims.Include(o => o.MezuniyetBigi).Where(e => e.OzgecmisId == ozgecmis.Id).ToListAsync();
            var deneyimler = await context.OzgecmisIsDeneyimleris.Where(d => d.OzgecmisId == ozgecmis.Id).ToListAsync();
            var beceriler = await context.OzgecmisBeceris.Where(b => b.OzgecmisId == ozgecmis.Id).ToListAsync();
            var referanslar = await context.OzgecmisReferans.Where(r => r.OzgecmisId == ozgecmis.Id).ToListAsync();
            var sertifikalar = await context.OzgecmisSertifikas.Where(s => s.OzgecmisId == ozgecmis.Id).ToListAsync();
            var yabanciDiller = await context.OzgecmisYabanciDils.Where(y => y.OzgecmisId == ozgecmis.Id).ToListAsync();

            var viewModel = new OzgecmisDetayViewModel
            { Id = ozgecmis.Id,
                Telefon = ozgecmis.Kullanici?.TelefonNo,
                Eposta = ozgecmis.Kullanici?.Eposta,
                AdSoyad = ozgecmis.AdSoyad,
                Sehir = ozgecmis.Sehir,
                Ilce = ozgecmis.Ilce,
                Adres = ozgecmis.Adres,
                DogumTarihi = ozgecmis.DogumTar,
                NitelikAd = ozgecmis.Hakkinda,
                MeslekAd = ozgecmis.Meslek?.Ad,
                Foto = ozgecmis.Foto,
                Egitimler = egitimler,
                Deneyimler = deneyimler,
                Beceriler = beceriler,
                Referanslar = referanslar,
                Sertifikalar = sertifikalar,
                YabanciDiller = yabanciDiller,
                SurucuBelge = ozgecmis.SurucuBelge?.Ad,
                SrcBelge = ozgecmis.Srcbelge?.Ad,
                Cinsiyet = ozgecmis.Cinsiyet?.Ad,
                Deneyim = ozgecmis.Deneyim?.Ad,
                MedeniHal = ozgecmis.MedeniHal.HasValue ?
                (ozgecmis.MedeniHal.Value ? MedeniHal.Evli : MedeniHal.Bekar)
                : MedeniHal.Bekar,
                EngelDurumu = false,


            };


            return View(viewModel);
        }






        [HttpGet]
        public async Task<JsonResult> GetMezuniyetBilgiler()
        {

            var mezuniyetBilgiler = await context.Egitims.Select(m => new
            {
                Value = m.Id.ToString(),
                Text = m.Ad
            }).ToListAsync();

            return Json(mezuniyetBilgiler);
        }



        [HttpGet]
        public async Task<IActionResult> DownloadPDF(Guid id)
        {
            var model = await Detay();
            if (model is NotFoundResult)
            {
                return NotFound();
            }
            var viewResult = model as ViewResult;
            var viewModel = viewResult?.Model as OzgecmisDetayViewModel;

            if (viewModel == null)
            {
                return NotFound();
            }

                 var pdfStream = new MemoryStream();
                  try
                  {
                      using (var writer = new PdfWriter(pdfStream))
                      {
                          using (var pdf = new PdfDocument(writer))
                          {
                              var document = new iText.Layout.Document(pdf);


                        document.Add(new Paragraph("Özgecmiş").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
                          document.Add(new Paragraph($"Eposta: {viewModel.Eposta}").SetFontSize(14));
                              document.Add(new Paragraph($"Telefon: {viewModel.Telefon}").SetFontSize(14));
                              document.Add(new Paragraph($"Adres: {viewModel.Adres}, {viewModel.Ilce}, {viewModel.Sehir}").SetFontSize(14));
                              document.Add(new Paragraph($"Doğum Tarihi: {viewModel.DogumTarihi?.ToShortDateString()}").SetFontSize(14));
                              document.Add(new Paragraph($"Cinsiyet {viewModel.Cinsiyet}").SetFontSize(14));

                              document.Add(new Paragraph($"Eğitim").SetBold().SetFontSize(18));


                              foreach (var egitim in viewModel.Egitimler)
                              {
                                  document.Add(new Paragraph($"{egitim.MezuniyetBigi?.Ad} - Mezuniyet Yılı: {egitim.MezuniyetYil}").SetFontSize(14));
                              }

                              document.Add(new Paragraph($"İş Deneyimi").SetBold().SetFontSize(18));
                              foreach (var deneyim in viewModel.Deneyimler)
                              {
                                  document.Add(new Paragraph($"{deneyim.Pozisyon} - {deneyim.BasBityil}")
                                      .SetFontSize(14));
                              }

                              document.Add(new Paragraph("Beceriler")
                        .SetBold()
                        .SetFontSize(18));

                              foreach (var beceri in viewModel.Beceriler)
                              {
                                  document.Add(new Paragraph(beceri.Beceri)
                                      .SetFontSize(14));
                              }


                              document.Add(new Paragraph("Referanslar")
                                  .SetBold()
                                  .SetFontSize(18));

                              foreach (var referans in viewModel.Referanslar)
                              {
                                  document.Add(new Paragraph($"{referans.Referans} - {referans.IletisimBilgi}")
                                      .SetFontSize(14));
                                  document.Add(new Paragraph($"Pozisyon: {referans.Pozisyon}")
                                     .SetFontSize(14));
                              }
  
                           // document.Close();
                      }    
                      }


             //   pdfStream.Position = 0;
             
                  var pdfFileName = "Ozgecmis.pdf";


                      return File(pdfStream.ToArray(), "application/pdf", pdfFileName);
                  }
                  catch (Exception ex)
                  {
                     
                      return BadRequest("Bir hata oluştu: " + ex.Message);
                  }
                  finally
                  {

                      pdfStream.Dispose(); 
                  }
      
        }








       

        [HttpPost]
        [Authorize(Roles = "IsArayan")]
        public async Task<IActionResult> Ozgecmis(Ozgecmi ozgecmi, OzgecmisViewModel viewModel)
        {
            var kullaniciId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            bool medeniHal;
            if(viewModel.MedeniHal1== "Bekar")
            {
                medeniHal = true;
            }
            else
            {
                medeniHal = false;
            }

            if (!ModelState.IsValid)
            {

                await FormData(viewModel);

                return View(viewModel);
            }

            var mevcutOzgecmis = await context.Ozgecmis
                .FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

            var mevcutOzgecmis1 = await context.Cvs.FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);


            int ozgecmisSay = mevcutOzgecmis1?.OzgecmisSay ?? 0;


            int yeniOzgecmisSira = ozgecmisSay + 1;



            if (mevcutOzgecmis != null)
            {
               
                mevcutOzgecmis.Hakkinda = viewModel.NitelikAd;
                mevcutOzgecmis.OzgecmisSay = yeniOzgecmisSira;
                mevcutOzgecmis.AdSoyad = viewModel.AdSoyad;
                mevcutOzgecmis.Sehir = viewModel.Sehir;
                mevcutOzgecmis.Ilce = viewModel.Ilce;
                mevcutOzgecmis.Adres = viewModel.Adres;
                mevcutOzgecmis.DogumTar = viewModel.DogumTarihi;
                mevcutOzgecmis.MedeniHal = medeniHal;
             
                mevcutOzgecmis.DeneyimId = viewModel.DeneyimId;
                mevcutOzgecmis.EgitimId = viewModel.EgitimId;
                mevcutOzgecmis.SurucuBelgeId = viewModel.SurucuBelgeId;
                mevcutOzgecmis.MeslekId = viewModel.MeslekId;
                mevcutOzgecmis.SrcbelgeId = viewModel.SrcbelgeId;
                mevcutOzgecmis.CinsiyetId = viewModel.CinsiyetId;
                mevcutOzgecmis.OlusturmaTarihi = DateTime.Now;

             
                if (viewModel.FotoDosya != null && viewModel.FotoDosya.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.FotoDosya.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.FotoDosya.CopyToAsync(stream);
                    }
                    mevcutOzgecmis.Foto = fileName;
                }

               
                context.OzgecmisIsDeneyimleris.RemoveRange(await context.OzgecmisIsDeneyimleris.Where(d => d.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());
                context.OzgecmisEgitims.RemoveRange(await context.OzgecmisEgitims.Where(e => e.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());
                context.OzgecmisBeceris.RemoveRange(await context.OzgecmisBeceris.Where(b => b.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());
                context.OzgecmisYabanciDils.RemoveRange(await context.OzgecmisYabanciDils.Where(y => y.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());
                context.OzgecmisSertifikas.RemoveRange(await context.OzgecmisSertifikas.Where(s => s.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());
                context.OzgecmisReferans.RemoveRange(await context.OzgecmisReferans.Where(r => r.OzgecmisId == mevcutOzgecmis.Id).ToListAsync());

                await context.SaveChangesAsync();

                foreach (var deneyim in viewModel.Deneyimlerim!)
                {
                    deneyim.OzgecmisId = mevcutOzgecmis.Id;
                    deneyim.Sil = false;
                    context.OzgecmisIsDeneyimleris.Add(deneyim);
                }

                foreach (var egitim in viewModel.Egitim!)
                {
                    egitim.OzgecmisId = mevcutOzgecmis.Id;
                    egitim.Sil = false;
                    context.OzgecmisEgitims.Add(egitim);
                }

                foreach (var beceri in viewModel.Beceriler!)
                {
                    beceri.OzgecmisId = mevcutOzgecmis.Id;
                    beceri.Sil = false;
                    context.OzgecmisBeceris.Add(beceri);
                }

                foreach (var dil in viewModel.YabnciDil!)
                {
                    dil.OzgecmisId = mevcutOzgecmis.Id;
                    dil.Sil = false;
                    context.OzgecmisYabanciDils.Add(dil);
                }

                foreach (var sertifika in viewModel.Sertifikalar!)
                {
                    sertifika.OzgecmisId = mevcutOzgecmis.Id;
                    sertifika.Sil = false;
                    context.OzgecmisSertifikas.Add(sertifika);
                }

                foreach (var referans in viewModel.Referanslar!)
                {
                    referans.OzgecmisId = mevcutOzgecmis.Id;
                    referans.Sil = false;
                    context.OzgecmisReferans.Add(referans);
                }

                await context.SaveChangesAsync();
            }
            else
            {


              


                var ozgecmis = new Ozgecmi
                {
                    KullaniciId = kullaniciId,
                    Hakkinda = viewModel.NitelikAd,
                    AdSoyad = viewModel.AdSoyad,
                    Sehir = viewModel.Sehir,
                    Ilce = viewModel.Ilce,
                    Adres = viewModel.Adres,
                    DogumTar = viewModel.DogumTarihi,
                    MedeniHal =  medeniHal,
                    OzgecmisSay = yeniOzgecmisSira,
                    DeneyimId = viewModel.DeneyimId,
                    EgitimId = viewModel.EgitimId,
                    SurucuBelgeId = viewModel.SurucuBelgeId,
                    MeslekId = viewModel.MeslekId,
                    SrcbelgeId = viewModel.SrcbelgeId,
                    CinsiyetId = viewModel.CinsiyetId,
                    Sira = await context.Ozgecmis.CountAsync() + 1,
                    OlusturmaTarihi = DateTime.Now,
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
                    ozgecmis.Foto = fileName;
                }
                context.Ozgecmis.Add(ozgecmis);
                await context.SaveChangesAsync();



                
                foreach (var deneyim in viewModel.Deneyimlerim!)
                {
                    deneyim.OzgecmisId = ozgecmi.Id;
                    deneyim.Sil = false;
                    context.OzgecmisIsDeneyimleris.Add(deneyim);
                }

                foreach (var egitim in viewModel.Egitim!)
                {
                    egitim.OzgecmisId = ozgecmi.Id;
                    egitim.Sil = false;
                    context.OzgecmisEgitims.Add(egitim);
                }

                foreach (var beceri in viewModel.Beceriler!)
                {
                    beceri.OzgecmisId = ozgecmi.Id;
                    beceri.Sil = false;
                    context.OzgecmisBeceris.Add(beceri);
                }

                foreach (var dil in viewModel.YabnciDil!)
                {
                    dil.OzgecmisId = ozgecmi.Id;
                    dil.Sil = false;
                    context.OzgecmisYabanciDils.Add(dil);
                }

                foreach (var sertifika in viewModel.Sertifikalar!)
                {
                    sertifika.OzgecmisId = ozgecmi.Id;
                    sertifika.Sil = false;
                    context.OzgecmisSertifikas.Add(sertifika);
                }

                foreach (var referans in viewModel.Referanslar!)
                {
                    referans.OzgecmisId = ozgecmi.Id;
                    referans.Sil = false;
                    context.OzgecmisReferans.Add(referans);
                }

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Profil", "Hesap");
        }
















        private async Task FormData(OzgecmisViewModel viewModel)
        {
            viewModel.Cinsiyetler = await context.Cinsiyets.OrderBy(x => x.Id).ToListAsync();
            viewModel.Egitimler = await context.Egitims.Where(x => !x.Sil).ToListAsync();
            viewModel.Meslekler = await context.Mesleks.OrderBy(x => x.Ad).ToListAsync();
            viewModel.Deneyimler = await context.Deneyims.OrderBy(d => d.Sira).ToListAsync();
            viewModel.SurucuBelgeler = await context.SurucuBelges.OrderBy(x => x.Sira).ToListAsync();
            viewModel.Srcbelgeler = await context.Srcbelges.OrderBy(x => x.Ad).ToListAsync();
            viewModel.MezunuyetBilgiler = await context.Egitims.Where(x => !x.Sil).ToListAsync();
        }
    }
}








