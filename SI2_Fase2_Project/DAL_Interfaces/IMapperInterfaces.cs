using DAL_Abstraction;
using Entities;

namespace DAL_Interfaces
{
    public interface IMapperTriplo : IMapper<Triplo, TriploKey> { }
    public interface IMapperInstrumento: IMapper<Instrumento,string> { }
    public interface IMapperCliente: IMapper<Cliente, string> { }
    public interface IMapperMercado: IMapper<Mercado, string> { }
    public interface IMapperRegisto: IMapper<Registo, RegistoKey> { }
 }
