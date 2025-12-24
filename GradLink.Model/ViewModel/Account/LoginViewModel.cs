using System;                         // Provides basic types like DateTime, Exception, etc.
using System.Collections.Generic;     // Enables use of generic collections like List<T>, Dictionary<K,V>, etc.
using System.ComponentModel.DataAnnotations; // Provides attributes for validation (e.g., [Required], [StringLength])
using System.Linq;                    // Enables LINQ queries and operations on collections
using System.Text;                    // Contains classes for text encoding, manipulation, etc.
using System.Threading.Tasks;         // Supports asynchronous programming (async/await, Task)

namespace GradLink.Model.ViewModel.Account
{
    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
