using System.Diagnostics.CodeAnalysis;

namespace Zz.AutoMapper;

public class EntityMap(Type entityType)
{
    protected static readonly Type _objectType = typeof(object);

    public Type EntityType { get; } = entityType;

    public required MappingProfileBuilder Profile { get; set; }

    public EntityMap From(Type sourceType)
    {
        Profile.CreateMap(sourceType, EntityType);
        return this;
    }

    public EntityMap From<TSource>() => From(typeof(TSource));

    public EntityMap To(Type destinationType)
    {
        Profile.CreateMap(EntityType, destinationType);
        return this;
    }

    public EntityMap To<TDestination>() => To(typeof(TDestination));

    public EntityMap ToSelf() => this.To(EntityType);

    public EntityMap ToBases(bool withBasesToBases = false)
    {
        var type = EntityType;

        // level 1
        if (ShouldEndMappingFor(type.BaseType))
        {
            return this;
        }
        Profile.CreateMap(type, type.BaseType);

        // level 2
        if (ShouldEndMappingFor(type.BaseType.BaseType))
        {
            return this;
        }

        Profile.CreateMap(type, type.BaseType.BaseType);
        if (withBasesToBases)
        {
            Profile.CreateMap(type.BaseType, type.BaseType.BaseType);
        }

        // level 3
        if (ShouldEndMappingFor(type.BaseType.BaseType.BaseType))
        {
            return this;
        }

        Profile.CreateMap(type, type.BaseType.BaseType.BaseType);
        if (withBasesToBases)
        {
            Profile.CreateMap(type.BaseType, type.BaseType.BaseType.BaseType);
            Profile.CreateMap(type.BaseType.BaseType, type.BaseType.BaseType.BaseType);
        }

        // level 4
        if (ShouldEndMappingFor(type.BaseType.BaseType.BaseType.BaseType))
        {
            return this;
        }

        Profile.CreateMap(type, type.BaseType.BaseType.BaseType.BaseType);
        if (withBasesToBases)
        {
            Profile.CreateMap(type.BaseType, type.BaseType.BaseType.BaseType.BaseType);
            Profile.CreateMap(type.BaseType.BaseType, type.BaseType.BaseType.BaseType.BaseType);
            Profile.CreateMap(
                type.BaseType.BaseType.BaseType,
                type.BaseType.BaseType.BaseType.BaseType
            );
        }

        // level 5
        if (ShouldEndMappingFor(type.BaseType.BaseType.BaseType.BaseType.BaseType))
        {
            return this;
        }

        Profile.CreateMap(type, type.BaseType.BaseType.BaseType.BaseType.BaseType);
        if (withBasesToBases)
        {
            Profile.CreateMap(type.BaseType, type.BaseType.BaseType.BaseType.BaseType.BaseType);
            Profile.CreateMap(
                type.BaseType.BaseType,
                type.BaseType.BaseType.BaseType.BaseType.BaseType
            );
            Profile.CreateMap(
                type.BaseType.BaseType.BaseType,
                type.BaseType.BaseType.BaseType.BaseType.BaseType
            );
            Profile.CreateMap(
                type.BaseType.BaseType.BaseType.BaseType,
                type.BaseType.BaseType.BaseType.BaseType.BaseType
            );
        }

        return this;

        bool ShouldEndMappingFor([NotNullWhen(false)] Type? type)
        {
            return type is null || type == _objectType;
        }
    }

    public EntityMap FromAndTo<T2>() => From<T2>().To<T2>();
}
