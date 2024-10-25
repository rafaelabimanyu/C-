using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AplikasiManajemenTugasHarian
{
    class Program
    {
        public class Tugas
        {
            public string Nama { get; set; }
            public bool Selesai { get; set; }
            public DateTime Tanggal { get; set; }
            public DateTime? TanggalJatuhTempo { get; set; }
            public string Prioritas { get; set; }

            public Tugas(string nama, DateTime? tanggalJatuhTempo = null, string prioritas = "Rendah")
            {
                Nama = nama;
                Selesai = false;
                Tanggal = DateTime.Now.Date; // Menggunakan .Date untuk hanya menyimpan tanggal
                TanggalJatuhTempo = tanggalJatuhTempo;
                Prioritas = prioritas;
            }

            public void TandaiSelesai()
            {
                Selesai = true;
            }

            public void TandaiBelumSelesai()
            {
                Selesai = false;
            }

            public void EditTugas(string nama, DateTime? tanggalJatuhTempo, string prioritas)
            {
                Nama = nama;
                TanggalJatuhTempo = tanggalJatuhTempo;
                Prioritas = prioritas;
            }

            public override string ToString()
            {
                return $"{Nama} - {(Selesai ? "Selesai" : "Belum Selesai")} (Dibuat: {Tanggal:dd-MM-yyyy}, Jatuh Tempo: {TanggalJatuhTempo?.ToString("dd-MM-yyyy") ?? "Tidak ada"}, Prioritas: {Prioritas})";
            }
        }

        public class KategoriTugas
        {
            public string Judul { get; set; }
            public List<Tugas> TugasList { get; set; }

            public KategoriTugas(string judul)
            {
                Judul = judul;
                TugasList = new List<Tugas>();
            }

            public void TambahTugas(Tugas tugas)
            {
                TugasList.Add(tugas);
            }

            public void HapusTugas(string nama)
            {
                var tugas = TugasList.FirstOrDefault(t => t.Nama.Equals(nama, StringComparison.OrdinalIgnoreCase));
                if (tugas != null)
                {
                    TugasList.Remove(tugas);
                }
            }

            public Tugas CariTugas(string nama)
            {
                return TugasList.FirstOrDefault(t => t.Nama.Equals(nama, StringComparison.OrdinalIgnoreCase));
            }

            public void EditJudulKategori(string judulBaru)
            {
                Judul = judulBaru;
            }

            public override string ToString()
            {
                return Judul;
            }
        }

        public class ManajemenTugas
        {
            public List<KategoriTugas> KategoriList { get; set; }

            public ManajemenTugas()
            {
                KategoriList = new List<KategoriTugas>();
            }

            public void TambahKategori(KategoriTugas kategori)
            {
                KategoriList.Add(kategori);
            }

            public KategoriTugas CariKategori(string judul)
            {
                return KategoriList.FirstOrDefault(k => k.Judul.Equals(judul, StringComparison.OrdinalIgnoreCase));
            }

            public void HapusKategori(string judul)
            {
                var kategori = CariKategori(judul);
                if (kategori != null)
                {
                    KategoriList.Remove(kategori);
                }
            }
        }

        private static ManajemenTugas manajemenTugas = new ManajemenTugas();
        private const string FilePath = "tugas.json"; // Nama file JSON

        static void Main(string[] args)
        {
            MuatData(); // Muat data dari file saat aplikasi dimulai
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Manajemen Tugas Harian ===");
                Console.WriteLine("1. Tambah Kategori Tugas");
                Console.WriteLine("2. Edit Kategori Tugas");
                Console.WriteLine("3. Tambah Tugas Baru ke Kategori");
                Console.WriteLine("4. Lihat Semua Kategori");
                Console.WriteLine("5. Lihat Tugas dalam Kategori");
                Console.WriteLine("6. Hapus Tugas dari Kategori");
                Console.WriteLine("7. Edit Tugas");
                Console.WriteLine("8. Hapus Kategori");
                Console.WriteLine("9. Tandai Tugas Selesai");
                Console.WriteLine("10. Tandai Tugas Belum Selesai");
                Console.WriteLine("11. Keluar");
                Console.Write("Pilih opsi: ");
                
                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1":
                        TambahKategori();
                        break;
                    case "2":
                        EditKategori();
                        break;
                    case "3":
                        TambahTugasKeKategori();
                        break;
                    case "4":
                        LihatSemuaKategori();
                        break;
                    case "5":
                        LihatTugasDalamKategori();
                        break;
                    case "6":
                        HapusTugasDariKategori();
                        break;
                    case "7":
                        EditTugas();
                        break;
                    case "8":
                        HapusKategori();
                        break;
                    case "9":
                        TandaiTugasSelesai();
                        break;
                    case "10":
                        TandaiTugasBelumSelesai();
                        break;
                    case "11":
                        return;
                    default:
                        Console.WriteLine("Opsi tidak valid. Silakan coba lagi.");
                        break;
                }

                Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                Console.ReadLine();
            }
        }

        private static void MuatData()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                manajemenTugas.KategoriList = JsonConvert.DeserializeObject<List<KategoriTugas>>(json) ?? new List<KategoriTugas>();
            }
        }

        private static void SimpanData()
        {
            string json = JsonConvert.SerializeObject(manajemenTugas.KategoriList, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private static void TambahKategori()
        {
            Console.Write("Masukkan judul kategori: ");
            string judul = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(judul))
            {
                Console.WriteLine("Judul kategori tidak boleh kosong.");
                return;
            }

            var kategori = new KategoriTugas(judul);
            manajemenTugas.TambahKategori(kategori);
            SimpanData(); // Simpan data setelah menambahkan kategori
            Console.WriteLine("Kategori berhasil ditambahkan.");
        }

        private static void EditKategori()
        {
            Console.Write("Masukkan judul kategori yang ingin diedit: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan judul kategori baru: ");
                string judulBaru = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(judulBaru))
                {
                    Console.WriteLine("Judul kategori tidak boleh kosong.");
                    return;
                }

                kategori.EditJudulKategori(judulBaru);
                SimpanData(); // Simpan data setelah mengedit kategori
                Console.WriteLine("Kategori berhasil diedit.");
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void TambahTugasKeKategori()
        {
            Console.Write("Masukkan judul kategori untuk menambah tugas: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan nama tugas: ");
                string namaTugas = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(namaTugas))
                {
                    Console.WriteLine("Nama tugas tidak boleh kosong.");
                    return;
                }

                Console.Write("Masukkan tanggal jatuh tempo (dd-MM-yyyy) atau tekan Enter jika tidak ada: ");
                string inputTanggal = Console.ReadLine();
                DateTime? tanggalJatuhTempo = null;

                if (DateTime.TryParseExact(inputTanggal, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var tempTanggal))
                {
                    tanggalJatuhTempo = tempTanggal.Date; // Menggunakan .Date untuk hanya menyimpan tanggal
                }

                Console.Write("Masukkan prioritas (Tinggi/Sedang/Rendah): ");
                string prioritas = Console.ReadLine();

                kategori.TambahTugas(new Tugas(namaTugas, tanggalJatuhTempo, prioritas));
                SimpanData(); // Simpan data setelah menambahkan tugas
                Console.WriteLine("Tugas berhasil ditambahkan ke kategori.");
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void LihatSemuaKategori()
        {
            Console.WriteLine("\nSemua Kategori:");
            if (manajemenTugas.KategoriList.Count == 0)
            {
                Console.WriteLine("Tidak ada kategori yang tersedia.");
            }
            else
            {
                foreach (var kategori in manajemenTugas.KategoriList)
                {
                    Console.WriteLine($"- {kategori}");
                }
            }
        }

        private static void LihatTugasDalamKategori()
        {
            Console.Write("Masukkan judul kategori untuk melihat tugas: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.WriteLine($"\nTugas dalam kategori {kategori.Judul}:");
                if (kategori.TugasList.Count == 0)
                {
                    Console.WriteLine("Tidak ada tugas dalam kategori ini.");
                }
                else
                {
                    foreach (var tugas in kategori.TugasList)
                    {
                        Console.WriteLine($"- {tugas}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void HapusTugasDariKategori()
        {
            Console.Write("Masukkan judul kategori untuk menghapus tugas: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan nama tugas yang ingin dihapus: ");
                string namaTugas = Console.ReadLine();
                kategori.HapusTugas(namaTugas);
                SimpanData(); // Simpan data setelah menghapus tugas
                Console.WriteLine("Tugas berhasil dihapus dari kategori.");
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void EditTugas()
        {
            Console.Write("Masukkan judul kategori untuk mengedit tugas: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan nama tugas yang ingin diedit: ");
                string namaTugas = Console.ReadLine();
                var tugas = kategori.CariTugas(namaTugas);

                if (tugas != null)
                {
                    Console.Write("Masukkan nama tugas baru: ");
                    string namaBaru = Console.ReadLine();
                    Console.Write("Masukkan tanggal jatuh tempo baru (dd-MM-yyyy) atau tekan Enter jika tidak ada: ");
                    string inputTanggal = Console.ReadLine();
                    DateTime? tanggalJatuhTempo = null;

                    if (DateTime.TryParseExact(inputTanggal, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var tempTanggal))
                    {
                        tanggalJatuhTempo = tempTanggal.Date; // Menggunakan .Date untuk hanya menyimpan tanggal
                    }

                    Console.Write("Masukkan prioritas baru (Tinggi/Sedang/Rendah): ");
                    string prioritasBaru = Console.ReadLine();
                    tugas.EditTugas(namaBaru, tanggalJatuhTempo, prioritasBaru);
                    SimpanData(); // Simpan data setelah mengedit tugas
                    Console.WriteLine("Tugas berhasil diedit.");
                }
                else
                {
                    Console.WriteLine("Tugas tidak ditemukan.");
                }
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void HapusKategori()
        {
            Console.Write("Masukkan judul kategori yang ingin dihapus: ");
            string judulKategori = Console.ReadLine();
            manajemenTugas.HapusKategori(judulKategori);
            SimpanData(); // Simpan data setelah menghapus kategori
            Console.WriteLine("Kategori berhasil dihapus.");
        }

        private static void TandaiTugasSelesai()
        {
            Console.Write("Masukkan judul kategori untuk menandai tugas selesai: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan nama tugas yang ingin ditandai selesai: ");
                string namaTugas = Console.ReadLine();
                var tugas = kategori.CariTugas(namaTugas);

                if (tugas != null)
                {
                    tugas.TandaiSelesai();
                    SimpanData(); // Simpan data setelah menandai tugas selesai
                    Console.WriteLine("Tugas berhasil ditandai selesai.");
                }
                else
                {
                    Console.WriteLine("Tugas tidak ditemukan.");
                }
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }

        private static void TandaiTugasBelumSelesai()
        {
            Console.Write("Masukkan judul kategori untuk menandai tugas belum selesai: ");
            string judulKategori = Console.ReadLine();
            var kategori = manajemenTugas.CariKategori(judulKategori);

            if (kategori != null)
            {
                Console.Write("Masukkan nama tugas yang ingin ditandai belum selesai: ");
                string namaTugas = Console.ReadLine();
                var tugas = kategori.CariTugas(namaTugas);

                if (tugas != null)
                {
                    tugas.TandaiBelumSelesai();
                    SimpanData(); // Simpan data setelah menandai tugas belum selesai
                    Console.WriteLine("Tugas berhasil ditandai belum selesai.");
                }
                else
                {
                    Console.WriteLine("Tugas tidak ditemukan.");
                }
            }
            else
            {
                Console.WriteLine("Kategori tidak ditemukan.");
            }
        }
    }
}
