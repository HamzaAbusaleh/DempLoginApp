using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models.ViewModel
{
    public class Result
    {
        public Result()
        {
            ErrorMessage = new List<string>();
        }
        public bool IsSuccessfull => !ErrorMessage.Any();

        public List<string> ErrorMessage { get; set; }

    }

    public class TResult<TEntity> : Result
    {
        public TEntity Data { get; set; }
    }
}