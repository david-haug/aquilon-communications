using System;
using System.Collections.Generic;
using System.Text;

namespace Aes.Communication.Infrastructure.Json
{
    public interface IDomainObjectParser<T>
    {
        T Create(dynamic data);
    }
}
