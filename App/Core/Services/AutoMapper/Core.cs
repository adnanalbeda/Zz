using AutoMapper;

namespace Zz.AutoMapper;

public class EntityMap<T>() : EntityMap(typeof(T))
{
    public EntityMap<T> From<TSource>(
        Func<IMappingExpression<TSource, T>, IMappingExpression<TSource, T>>? builder = null
    )
    {
        var map = Profile.CreateMap<TSource, T>();
        builder?.Invoke(map);
        return this;
    }

    public EntityMap<T> To<TDestination>(
        Func<IMappingExpression<T, TDestination>, IMappingExpression<T, TDestination>>? builder =
            null
    )
    {
        var map = Profile.CreateMap<T, TDestination>();
        builder?.Invoke(map);
        return this;
    }

    public EntityMap<T> ToSelf(
        Func<IMappingExpression<T, T>, IMappingExpression<T, T>>? builder = null
    ) => this.To(builder);

    /// <summary>
    /// Map profile up to 5 level of its bases. <br />
    /// <see cref="object"/> is excluded.
    /// </summary>
    /// <param name="withBasesToBases"></param>
    /// <returns></returns>
    public new EntityMap<T> ToBases(bool withBasesToBases = true)
    {
        base.ToBases(withBasesToBases);
        return this;
    }

    public EntityMap<T> FromAndTo<T2>(
        Func<IMappingExpression<T2, T>, IMappingExpression<T2, T>>? fromBuilder = null,
        Func<IMappingExpression<T, T2>, IMappingExpression<T, T2>>? toBuilder = null
    ) => From(fromBuilder).To(toBuilder);
}
