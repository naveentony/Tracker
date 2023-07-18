using MongoDB.Driver.Linq;
using System.Text.Json.Serialization;

namespace Tracker.Application.Models
{
    public class OperationResult<T>
    {
        public T Payload { get; set; }
        public bool IsError { get; private set; }
        public List<Error> Errors { get; } = new List<Error>();

        /// <summary>
        /// Adds an error to the Error list and sets the IsError flag to true
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void AddError(ErrorCode code, string message)
        {
            HandleError(code, message);
        }
        public T SetResult(T data)
        {
            
            CurrentPage = 1;
            TotalPages = 1;
            PageSize = 10;
            TotalCount = 10;
            return Payload = data;
        }

        /// <summary>
        /// Adds a default error to the Error list with the error code UnknownError
        /// </summary>
        /// <param name="message"></param>
        public void AddUnknownError(string message)
        {
            HandleError(ErrorCode.UnknownError, message);
        }
        

        /// <summary>
        /// Sets the IsError flag to default (false)
        /// </summary>
        public void ResetIsErrorFlag()
        {
            IsError = false;
        }

        #region Private methods

        private void HandleError(ErrorCode code, string message)
        {
            Errors.Add(new Error { Code = code, Message = message });
            IsError = true;
        }

       
        public int CurrentPage { get;  set; }
      
        public int TotalPages { get;  set; }
      
        public int PageSize { get;  set; }
       
        public long TotalCount { get;  set; }
        
        public bool HasPrevious => CurrentPage > 1;
      
        public bool HasNext => CurrentPage < TotalPages;


        #endregion
    }
    public static class PagingExtensions
    {
        //used by LINQ to SQL
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        {
            
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //used by LINQ
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

    }
}
