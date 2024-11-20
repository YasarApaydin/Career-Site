using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kariyer.Models;

public partial class KariyerContext : DbContext
{
    public KariyerContext()
    {
    }

    public KariyerContext(DbContextOptions<KariyerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Begeni> Begenis { get; set; }

    public virtual DbSet<CalismaSekli> CalismaSeklis { get; set; }

    public virtual DbSet<Cinsiyet> Cinsiyets { get; set; }

    public virtual DbSet<Cv> Cvs { get; set; }

    public virtual DbSet<Deneyim> Deneyims { get; set; }

    public virtual DbSet<Duyuru> Duyurus { get; set; }

    public virtual DbSet<Egitim> Egitims { get; set; }

    public virtual DbSet<Ilce> Ilces { get; set; }

    public virtual DbSet<IsArayan> IsArayans { get; set; }

    public virtual DbSet<IsBasvuru> IsBasvurus { get; set; }

    public virtual DbSet<IsIlanlar> IsIlanlars { get; set; }

    public virtual DbSet<IsIlanlarNitelik> IsIlanlarNiteliks { get; set; }

    public virtual DbSet<IsVeren> IsVerens { get; set; }

    public virtual DbSet<Kategori> Kategoris { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<KullaniciPozisyonlar> KullaniciPozisyonlars { get; set; }

    public virtual DbSet<KullaniciTur> KullaniciTurs { get; set; }

    public virtual DbSet<Kurslar> Kurslars { get; set; }

    public virtual DbSet<Meslek> Mesleks { get; set; }

    public virtual DbSet<MevcutPersonelSayi> MevcutPersonelSayis { get; set; }

    public virtual DbSet<Nitelik> Niteliks { get; set; }

    public virtual DbSet<Ozgecmi> Ozgecmis { get; set; }

    public virtual DbSet<OzgecmisBeceri> OzgecmisBeceris { get; set; }

    public virtual DbSet<OzgecmisEgitim> OzgecmisEgitims { get; set; }

    public virtual DbSet<OzgecmisIsDeneyimleri> OzgecmisIsDeneyimleris { get; set; }

    public virtual DbSet<OzgecmisReferan> OzgecmisReferans { get; set; }

    public virtual DbSet<OzgecmisSertifika> OzgecmisSertifikas { get; set; }

    public virtual DbSet<OzgecmisYabanciDil> OzgecmisYabanciDils { get; set; }

    public virtual DbSet<Pozisyon> Pozisyons { get; set; }

    public virtual DbSet<Profil> Profils { get; set; }

    public virtual DbSet<Sehir> Sehirs { get; set; }

    public virtual DbSet<Sektor> Sektors { get; set; }

    public virtual DbSet<SektorKategori> SektorKategoris { get; set; }

    public virtual DbSet<SektorPozisyon> SektorPozisyons { get; set; }

    public virtual DbSet<SosyalBaglanti> SosyalBaglantis { get; set; }

    public virtual DbSet<Srcbelge> Srcbelges { get; set; }

    public virtual DbSet<SurucuBelge> SurucuBelges { get; set; }

    public virtual DbSet<Uyruk> Uyruks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-98AUGDI\\SQLEXPRESS;Database=Kariyer;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Begeni>(entity =>
        {
            entity.ToTable("Begeni");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BegeniTarih).HasColumnType("datetime");
            entity.Property(e => e.IlanId).HasColumnName("IlanID");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");

            entity.HasOne(d => d.Ilan).WithMany(p => p.Begenis)
                .HasForeignKey(d => d.IlanId)
                .HasConstraintName("FK_Begeni_IsIlanlar");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.Begenis)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_Begeni_Kullanici");
        });

        modelBuilder.Entity<CalismaSekli>(entity =>
        {
            entity.ToTable("CalismaSekli");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Cinsiyet>(entity =>
        {
            entity.ToTable("Cinsiyet");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Cv>(entity =>
        {
            entity.ToTable("CV");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cvpdf).HasColumnName("CVpdf");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.OlusturmaTarihi).HasColumnType("datetime");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.Cvs)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_CV_Kullanici");
        });

        modelBuilder.Entity<Deneyim>(entity =>
        {
            entity.ToTable("Deneyim");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Duyuru>(entity =>
        {
            entity.ToTable("Duyuru");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Egitim>(entity =>
        {
            entity.ToTable("Egitim");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Ilce>(entity =>
        {
            entity.ToTable("Ilce");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SehirId).HasColumnName("SehirID");

            entity.HasOne(d => d.Sehir).WithMany(p => p.Ilces)
                .HasForeignKey(d => d.SehirId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ilce_Sehir");
        });

        modelBuilder.Entity<IsArayan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_IsArayan_1");

            entity.ToTable("IsArayan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CinsiyetId).HasColumnName("CinsiyetID");
            entity.Property(e => e.DeneyimId).HasColumnName("DeneyimID");
            entity.Property(e => e.EgitimId).HasColumnName("EgitimID");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.MeslekId).HasColumnName("MeslekID");
            entity.Property(e => e.SrcbelgeId).HasColumnName("SRCBelgeID");
            entity.Property(e => e.SurucuBelgeId).HasColumnName("SurucuBelgeID");
            entity.Property(e => e.UyrukId).HasColumnName("UyrukID");
            entity.Property(e => e.İlceId).HasColumnName("İlceID");

            entity.HasOne(d => d.Deneyim).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.DeneyimId)
                .HasConstraintName("FK_IsArayan_Deneyim");

            entity.HasOne(d => d.Egitim).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.EgitimId)
                .HasConstraintName("FK_IsArayan_Egitim");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_IsArayan_Kullanici");

            entity.HasOne(d => d.Meslek).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.MeslekId)
                .HasConstraintName("FK_IsArayan_Meslek");

            entity.HasOne(d => d.Srcbelge).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.SrcbelgeId)
                .HasConstraintName("FK_IsArayan_SRCBelge");

            entity.HasOne(d => d.SurucuBelge).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.SurucuBelgeId)
                .HasConstraintName("FK_IsArayan_SurucuBelge");

            entity.HasOne(d => d.Uyruk).WithMany(p => p.IsArayans)
                .HasForeignKey(d => d.UyrukId)
                .HasConstraintName("FK_IsArayan_Uyruk");
        });

        modelBuilder.Entity<IsBasvuru>(entity =>
        {
            entity.ToTable("IsBasvuru");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsArayanId).HasColumnName("IsArayanID");
            entity.Property(e => e.IsIlanlarId).HasColumnName("IsIlanlarID");
            entity.Property(e => e.IsVerenId).HasColumnName("IsVerenID");

            entity.HasOne(d => d.IsArayan).WithMany(p => p.IsBasvurus)
                .HasForeignKey(d => d.IsArayanId)
                .HasConstraintName("FK_IsBasvuru_IsArayan");

            entity.HasOne(d => d.IsIlanlar).WithMany(p => p.IsBasvurus)
                .HasForeignKey(d => d.IsIlanlarId)
                .HasConstraintName("FK_IsBasvuru_IsIlanlar");

            entity.HasOne(d => d.IsVeren).WithMany(p => p.IsBasvurus)
                .HasForeignKey(d => d.IsVerenId)
                .HasConstraintName("FK_IsBasvuru_IsVeren");
        });

        modelBuilder.Entity<IsIlanlar>(entity =>
        {
            entity.ToTable("IsIlanlar");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BasTarih).HasColumnType("datetime");
            entity.Property(e => e.CalismaSekliId).HasColumnName("CalismaSekliID");
            entity.Property(e => e.CinsiyetId).HasColumnName("CinsiyetID");
            entity.Property(e => e.DeneyimId).HasColumnName("DeneyimID");
            entity.Property(e => e.EgitimId).HasColumnName("EgitimID");
            entity.Property(e => e.IsVerenId).HasColumnName("IsVerenID");
            entity.Property(e => e.PosizyonId).HasColumnName("PosizyonID");

            entity.HasOne(d => d.CalismaSekli).WithMany(p => p.IsIlanlars)
                .HasForeignKey(d => d.CalismaSekliId)
                .HasConstraintName("FK_IsIlanlar_CalismaSekli");

            entity.HasOne(d => d.Deneyim).WithMany(p => p.IsIlanlars)
                .HasForeignKey(d => d.DeneyimId)
                .HasConstraintName("FK_IsIlanlar_Deneyim");

            entity.HasOne(d => d.Egitim).WithMany(p => p.IsIlanlars)
                .HasForeignKey(d => d.EgitimId)
                .HasConstraintName("FK_IsIlanlar_Egitim");

            entity.HasOne(d => d.IsVeren).WithMany(p => p.IsIlanlars)
                .HasForeignKey(d => d.IsVerenId)
                .HasConstraintName("FK_IsIlanlar_IsVeren");

            entity.HasOne(d => d.Posizyon).WithMany(p => p.IsIlanlars)
                .HasForeignKey(d => d.PosizyonId)
                .HasConstraintName("FK_IsIlanlar_Pozisyon");
        });

        modelBuilder.Entity<IsIlanlarNitelik>(entity =>
        {
            entity.ToTable("IsIlanlarNitelik");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsIlanlarId).HasColumnName("IsIlanlarID");
            entity.Property(e => e.NitelikId).HasColumnName("NitelikID");

            entity.HasOne(d => d.IsIlanlar).WithMany(p => p.IsIlanlarNiteliks)
                .HasForeignKey(d => d.IsIlanlarId)
                .HasConstraintName("FK_IsIlanlarNitelik_IsIlanlar1");

            entity.HasOne(d => d.Nitelik).WithMany(p => p.IsIlanlarNiteliks)
                .HasForeignKey(d => d.NitelikId)
                .HasConstraintName("FK_IsIlanlarNitelik_Nitelik1");
        });

        modelBuilder.Entity<IsVeren>(entity =>
        {
            entity.ToTable("IsVeren");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IlceId).HasColumnName("IlceID");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.MevcutPersonelSayiId).HasColumnName("MevcutPersonelSayiID");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.IsVerens)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_IsVeren_Kullanici");

            entity.HasOne(d => d.MevcutPersonelSayi).WithMany(p => p.IsVerens)
                .HasForeignKey(d => d.MevcutPersonelSayiId)
                .HasConstraintName("FK_IsVeren_MevcutPersonelSayi");
        });

        modelBuilder.Entity<Kategori>(entity =>
        {
            entity.ToTable("Kategori");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.ToTable("Kullanici");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.KullaniciTurId).HasColumnName("KullaniciTurID");
            entity.Property(e => e.Tck)
                .HasMaxLength(11)
                .HasColumnName("TCK");

            entity.HasOne(d => d.KullaniciTur).WithMany(p => p.Kullanicis)
                .HasForeignKey(d => d.KullaniciTurId)
                .HasConstraintName("FK_Kullanici_KullaniciTur");
        });

        modelBuilder.Entity<KullaniciPozisyonlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_KullaniciMeslekler");

            entity.ToTable("KullaniciPozisyonlar");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.PozisyonId).HasColumnName("PozisyonID");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciPozisyonlars)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_KullaniciPozisyonlar_Kullanici");

            entity.HasOne(d => d.Pozisyon).WithMany(p => p.KullaniciPozisyonlars)
                .HasForeignKey(d => d.PozisyonId)
                .HasConstraintName("FK_KullaniciPozisyonlar_Pozisyon");
        });

        modelBuilder.Entity<KullaniciTur>(entity =>
        {
            entity.ToTable("KullaniciTur");
        });

        modelBuilder.Entity<Kurslar>(entity =>
        {
            entity.ToTable("Kurslar");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IlceId).HasColumnName("IlceID");
            entity.Property(e => e.IsVerenId).HasColumnName("IsVerenID");

            entity.HasOne(d => d.Ilce).WithMany(p => p.Kurslars)
                .HasForeignKey(d => d.IlceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kurslar_Ilce");

            entity.HasOne(d => d.IsVeren).WithMany(p => p.Kurslars)
                .HasForeignKey(d => d.IsVerenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kurslar_IsVeren");
        });

        modelBuilder.Entity<Meslek>(entity =>
        {
            entity.ToTable("Meslek");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<MevcutPersonelSayi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Table_1");

            entity.ToTable("MevcutPersonelSayi");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Nitelik>(entity =>
        {
            entity.ToTable("Nitelik");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Ozgecmi>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CinsiyetId).HasColumnName("CinsiyetID");
            entity.Property(e => e.DeneyimId).HasColumnName("DeneyimID");
            entity.Property(e => e.EgitimId).HasColumnName("EgitimID");
            entity.Property(e => e.Hakkinda).HasColumnType("text");
            entity.Property(e => e.KullaniciId).HasColumnName("KUllaniciID");
            entity.Property(e => e.MeslekId).HasColumnName("MeslekID");
            entity.Property(e => e.OlusturmaTarihi).HasColumnType("datetime");
            entity.Property(e => e.SrcbelgeId).HasColumnName("SRCBelgeID");
            entity.Property(e => e.SurucuBelgeId).HasColumnName("SurucuBelgeID");

            entity.HasOne(d => d.Cinsiyet).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.CinsiyetId)
                .HasConstraintName("FK_Ozgecmis_Cinsiyet");

            entity.HasOne(d => d.Deneyim).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.DeneyimId)
                .HasConstraintName("FK_Ozgecmis_Deneyim");

            entity.HasOne(d => d.Egitim).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.EgitimId)
                .HasConstraintName("FK_Ozgecmis_Egitim");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_Ozgecmis_Kullanici");

            entity.HasOne(d => d.Meslek).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.MeslekId)
                .HasConstraintName("FK_Ozgecmis_Meslek");

            entity.HasOne(d => d.Srcbelge).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.SrcbelgeId)
                .HasConstraintName("FK_Ozgecmis_SRCBelge");

            entity.HasOne(d => d.SurucuBelge).WithMany(p => p.Ozgecmis)
                .HasForeignKey(d => d.SurucuBelgeId)
                .HasConstraintName("FK_Ozgecmis_SurucuBelge");
        });

        modelBuilder.Entity<OzgecmisBeceri>(entity =>
        {
            entity.ToTable("OzgecmisBeceri");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisBeceris)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisBeceri_Ozgecmis");
        });

        modelBuilder.Entity<OzgecmisEgitim>(entity =>
        {
            entity.ToTable("OzgecmisEgitim");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.MezuniyetBigiId).HasColumnName("MezuniyetBigiID");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.MezuniyetBigi).WithMany(p => p.OzgecmisEgitims)
                .HasForeignKey(d => d.MezuniyetBigiId)
                .HasConstraintName("FK_OzgecmisEgitim_Egitim");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisEgitims)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisEgitim_Ozgecmis");
        });

        modelBuilder.Entity<OzgecmisIsDeneyimleri>(entity =>
        {
            entity.ToTable("OzgecmisIsDeneyimleri");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Acıiklama).HasColumnType("text");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisIsDeneyimleris)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisIsDeneyimleri_Ozgecmis");
        });

        modelBuilder.Entity<OzgecmisReferan>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisReferans)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisReferans_Ozgecmis");
        });

        modelBuilder.Entity<OzgecmisSertifika>(entity =>
        {
            entity.ToTable("OzgecmisSertifika");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisSertifikas)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisSertifika_Ozgecmis");
        });

        modelBuilder.Entity<OzgecmisYabanciDil>(entity =>
        {
            entity.HasKey(e => e.ıd);

            entity.ToTable("OzgecmisYabanciDil");

            entity.Property(e => e.ıd).HasDefaultValueSql("(newid())");
            entity.Property(e => e.OzgecmisId).HasColumnName("OzgecmisID");

            entity.HasOne(d => d.Ozgecmis).WithMany(p => p.OzgecmisYabanciDils)
                .HasForeignKey(d => d.OzgecmisId)
                .HasConstraintName("FK_OzgecmisYabanciDil_Ozgecmis");
        });

        modelBuilder.Entity<Pozisyon>(entity =>
        {
            entity.ToTable("Pozisyon");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Profil>(entity =>
        {
            entity.ToTable("Profil");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IlceId).HasColumnName("IlceID");
            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.MeslekId).HasColumnName("MeslekID");
            entity.Property(e => e.SosyalBaglantiId).HasColumnName("SosyalBaglantiID");
            entity.Property(e => e.UyrukId).HasColumnName("UyrukID");

            entity.HasOne(d => d.Ilce).WithMany(p => p.Profils)
                .HasForeignKey(d => d.IlceId)
                .HasConstraintName("FK_Profil_Ilce");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.Profils)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("FK_Profil_Kullanici");

            entity.HasOne(d => d.Meslek).WithMany(p => p.Profils)
                .HasForeignKey(d => d.MeslekId)
                .HasConstraintName("FK_Profil_Meslek");

            entity.HasOne(d => d.SosyalBaglanti).WithMany(p => p.Profils)
                .HasForeignKey(d => d.SosyalBaglantiId)
                .HasConstraintName("FK_Profil_SosyalBaglanti");

            entity.HasOne(d => d.Uyruk).WithMany(p => p.Profils)
                .HasForeignKey(d => d.UyrukId)
                .HasConstraintName("FK_Profil_Uyruk");
        });

        modelBuilder.Entity<Sehir>(entity =>
        {
            entity.ToTable("Sehir");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Sektor>(entity =>
        {
            entity.ToTable("Sektor");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SektorKategori>(entity =>
        {
            entity.ToTable("SektorKategori");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.KategoriId).HasColumnName("KategoriID");
            entity.Property(e => e.SektorId).HasColumnName("SektorID");

            entity.HasOne(d => d.Kategori).WithMany(p => p.SektorKategoris)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK_SektorKategori_Kategori");

            entity.HasOne(d => d.Sektor).WithMany(p => p.SektorKategoris)
                .HasForeignKey(d => d.SektorId)
                .HasConstraintName("FK_SektorKategori_Sektor");
        });

        modelBuilder.Entity<SektorPozisyon>(entity =>
        {
            entity.ToTable("SektorPozisyon");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PozisyonId).HasColumnName("PozisyonID");
            entity.Property(e => e.SektorId).HasColumnName("SektorID");

            entity.HasOne(d => d.Pozisyon).WithMany(p => p.SektorPozisyons)
                .HasForeignKey(d => d.PozisyonId)
                .HasConstraintName("FK_SektorPozisyon_Pozisyon");

            entity.HasOne(d => d.Sektor).WithMany(p => p.SektorPozisyons)
                .HasForeignKey(d => d.SektorId)
                .HasConstraintName("FK_SektorPozisyon_Sektor");
        });

        modelBuilder.Entity<SosyalBaglanti>(entity =>
        {
            entity.ToTable("SosyalBaglanti");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Srcbelge>(entity =>
        {
            entity.ToTable("SRCBelge");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<SurucuBelge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Surucu");

            entity.ToTable("SurucuBelge");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Uyruk>(entity =>
        {
            entity.ToTable("Uyruk");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
