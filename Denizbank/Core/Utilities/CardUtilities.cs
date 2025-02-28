namespace Denizbank.Core.Utilities
{
    public class CardUtilities
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// 16 haneli rastgele kart numarası oluşturur
        /// </summary>
        /// <param name="cardType">Kart tipi (Visa: 4, MasterCard: 5, vb.)</param>
        /// <returns>Rastgele kart numarası</returns>
        public static string GenerateCardNumber(int cardType = 4)
        {
            string cardNumber = cardType.ToString();

            // Geri kalan 15 haneyi rastgele oluştur
            for (int i = 0; i < 15; i++)
            {
                cardNumber += _random.Next(0, 10).ToString();
            }

            return cardNumber;
        }

        /// <summary>
        /// Rastgele 3 haneli CSV (güvenlik kodu) oluşturur
        /// </summary>
        /// <returns>Rastgele CSV değeri</returns>
        public static ushort GenerateCsv()
        {
            return (ushort)_random.Next(100, 1000);
        }

        /// <summary>
        /// Belirtilen yıl kadar sonrası için son kullanma tarihi oluşturur
        /// </summary>
        /// <param name="yearsFromNow">Şimdiki tarihten kaç yıl sonrası (varsayılan: 3)</param>
        /// <returns>MM/YY formatında son kullanma tarihi</returns>
        public static string GenerateExpirationDate(int yearsFromNow = 3)
        {
            var date = DateTime.Now.AddYears(yearsFromNow);
            return $"{date.Month:D2}/{date.Year % 100:D2}";
        }

        /// <summary>
        /// 1-28 arasında random bir sayı üretir
        /// 28 olma sebebi en düşük güne sahip Şubat ayından dolayı.
        /// </summary>
        /// <returns>int formatında son kullanma tarihi</returns>
        public static ushort GenerateCutOfDate()
        {
            var randomDay = _random.Next(1,28);
            return ((ushort)randomDay);
        }

        /// <summary>
        /// Rastgele Türkiye IBAN numarası oluşturur
        /// </summary>
        /// <param name="bankCode">Banka kodu (varsayılan: 00001)</param>
        /// <param name="accountType">Hesap tipi (0 = bireysel hesap)</param>
        /// <returns>TR ile başlayan IBAN numarası</returns>
        public static string GenerateIBAN(string bankCode = "00001", string accountType = "0")
        {
            // 16 basamaklı hesap numarası
            string accountNumber = "";
            for (int i = 0; i < 16; i++)
            {
                accountNumber += _random.Next(0, 10).ToString();
            }

            // Gerçek IBAN doğrulama algoritması kullanılmamıştır
            string checkDigits = "42";

            return $"TR{checkDigits}{bankCode}{accountType}{accountNumber}";
        }
    }
}