# ICrud
#### An Abstract Repository Implmentation for Entity Framework, with Entity2DTO / DTO2Entity conversion. 

>It makes possible to create both Repository Interface and Class by extending the AbstractRepository.cs Abstract Class and the ICrud.cs Interface.

## Example 

>Let's start by defining a data Class and it's DTO.

_Venta.cs_
```csharp
public class Venta
{
    public long VentaId { get; set; }
    public string Text { get; set; }
}
```
_VentaDTO.cs_
```csharp
public class VentaDTO
{
    public long VentaId { get; set; }
    public string Text { get; set; }
}
```

>We need to define two conversion functions by implmenting the inteface IConversor<TEntity, TDTO>

_VentaConversor.cs_
```csharp
using ICrud;
...
public class VentaConversor : IConversor<Venta, VentaDTO>
{
	public Venta DTO2Entity(VentaDTO dto)
    {
        Venta venta = new Venta
        {
            VentaId = dto.VentaId,
            Text = dto.Text,
        };
        return venta;
    }

    public VentaDTO Entity2DTO(Venta entity)
    {
        VentaDTO dto = new VentaDTO
        {
            VentaId = entity.VentaId,
            Text = entity.Text,
        };
        return dto;
    }
}
```
>We also need to create a Factory class for our context that Implements IDBFactory 

_DBFactory.cs_
```csharp
using ICrud;
...
public class DBFactory: IDBFactory<MyContextClass>
{
    public MyContextClass GetInstace()
    {
        return new MyContextClass();
    }
}
```
>Now we can start defining our **repository** by it's interface that extends ICrud<TEntity, TDTO>

_IVentaRepository.cs_
```csharp
using ICrud;
...
public interface IVentaRepository: ICrud<long, VentaDTO>
{
	// your methods.
}
```
>Finally we write our **repository** by inheriting from AbstractRepository<Key, TEntity, TDTO, TDB> and implementing your repository interface _(ex: IVentaRepository)_

_VentaRepository.cs_
```csharp
using ICrud;
...
public class VentaRepository : AbstractRepository<long, Venta, VentaDTO, MyContextClass>, IVentaRepository
{
	// you need to create a constructor with 2 args,
	// the factory and the conversor classes we made.
    public VentaRepository(IDBFactory<MyContextClass> dbContextFactory, IConversor<Venta,VentaDTO> conversor) :base(dbContextFactory, conversor)
    {

    }
    // and override the abstract Update(TDTO dto);
    public override VentaDTO Update(VentaDTO dto)
    {
    	// you could use your own logic here or use the protected Update,
    	// which takes the dto, and a lambda predicate to get the primary key
        return base.Update(dto, v => v.VentaId == dto.VentaId);
    }
}
```

Cheers!