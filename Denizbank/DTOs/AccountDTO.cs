using System.ComponentModel.DataAnnotations;
using Denizbank.Core.Enums;

namespace Denizbank.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "TC Kimlik numarası zorunludur")]
        [Range(10000000000, 99999999999, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır")]
        public required ulong TcNo { get; set; }

        [Required(ErrorMessage = "Şifre Zorunludur")]
        [Range(100000, 999999, ErrorMessage = "Şifre 6 haneli olmak zorundadır")]
        public required int Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public uint AccountId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "TC Kimlik numarası zorunludur")]
        [Range(10000000000, 99999999999, ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır")]
        public ulong TcNo { get; set; }

        [Required(ErrorMessage = "Şifre Zorunludur")]
        [Range(100000, 999999, ErrorMessage = "Şifre 6 haneli olmak zorundadır")]
        public int Password { get; set; }

        [Required(ErrorMessage = "İsim Zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "İsim 2-100 karakter arasında olmalıdır")]
        public string Name { get; set; } = null!;
    }

    public class RegisterResponse
    {
        public uint AccountId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AccountDto
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public ulong TcNo { get; set; }
        public List<CardDto> Cards { get; set; }
    }


}