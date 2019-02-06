using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_ID
{
    public interface ISpecification<in TEntity>
    {
        // Reads the reasons that cause Business ID dissatisfactions.
        IEnumerable<string> ReasonsForDissatisfaction { get; }

        // Checks the conditions for satisfaction. Returns a boolean.
        bool IsSatisfiedBy(TEntity entity);
    }
}
