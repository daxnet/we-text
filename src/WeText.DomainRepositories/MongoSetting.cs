using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.DomainRepositories
{
    internal class WeTextMongoSetting
    {
        public string ConnectionString => "mongodb://localhost:27017";

        public string DatabaseName => "WeText";

        public string CollectionName => "DomainEvents";
    }
}
