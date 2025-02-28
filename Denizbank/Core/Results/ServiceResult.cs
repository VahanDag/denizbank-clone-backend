using System;

namespace Denizbank.Core.Results
{
    /// <summary>
    /// Servis katmanı operasyonları için sonuç sınıfı
    /// </summary>
    /// <typeparam name="T">Servis operasyonundan dönecek veri tipi</typeparam>
    public class ServiceResult<T>
    {
 
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Data { get; private set; }

        private ServiceResult(bool isSuccess, string errorMessage = null, T data = default)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>(true, null, data);
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Hata mesajı boş olamaz", nameof(errorMessage));

            return new ServiceResult<T>(false, errorMessage);
        }


        public static ServiceResult<T> Failure(Exception exception)
        {
            return new ServiceResult<T>(false, exception.Message);
        }
    }
}