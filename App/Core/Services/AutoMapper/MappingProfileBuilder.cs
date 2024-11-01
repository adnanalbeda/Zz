using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Zz.Model;

namespace Zz.AutoMapper;

public abstract class MappingProfileBuilder : Profile
{
    protected MappingProfileBuilder()
    {
        InternalMaps();
    }

    private void InternalMaps()
    {
        //=========
        // 3rd deps
        CreateMap<Id22, Id22>();

        //=======
        // Common
        foreach (
            var t in typeof(ICommonType)
                .Assembly.GetTypes()
                .Where(x => !x.IsInterface && !x.IsEnum && !x.IsGenericType)
                .Where(x => x.GetInterfaces().Contains(typeof(ICommonType)))
        )
        {
            MapEntity(t).ToSelf().ToBases(withBasesToBases: false);

            if (t.GetCustomAttribute(typeof(ComplexTypeAttribute)) is not null)
                this.Internal()
                    .ForAllPropertyMaps(
                        p => p.SourceType == t,
                        (p, q) =>
                        {
                            q.DoNotAllowNull();
                        }
                    );
        }

        MapEntity(typeof(DFlag<>)).ToSelf();
        this.Internal()
            .ForAllPropertyMaps(
                p => p.SourceType == typeof(DFlag<>),
                (p, q) =>
                {
                    q.DoNotAllowNull();
                }
            );

        //=======
        // Models
        foreach (
            var t in typeof(IModelType)
                .Assembly.GetTypes()
                .Where(x => !x.IsInterface && !x.IsEnum && !x.IsGenericType)
                .Where(x => x.GetInterfaces().Contains(typeof(IModelType)))
                .Where(x => !x.GetInterfaces().Contains(typeof(IModelIdentityType)))
        )
        {
            MapEntity(t).ToSelf().ToBases(withBasesToBases: false);
        }

        MapEntities();
    }

    protected abstract void MapEntities();

    /// <summary>
    /// Define entity source type. Doesn't create any map.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public EntityMap MapEntity(Type t)
    {
        return new EntityMap(t) { Profile = this };
    }

    /// <summary>
    /// Define entity source type. Doesn't create any map.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public EntityMap<T> MapEntity<T>()
    {
        return new EntityMap<T> { Profile = this };
    }
}
