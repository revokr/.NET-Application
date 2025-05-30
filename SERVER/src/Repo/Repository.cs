using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.src.Repo
{
    public interface Repository<ID, E>
    {
        
        E findOne(ID id);

        E findByName(string name);

        List<E> findAll();

        E save(E entity);

        E delete(ID id);

        E update(E entity);
    }
}
